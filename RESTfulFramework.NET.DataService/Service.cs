﻿using RESTfulFramework.NET.ComponentModel;
using System.IO;
using System;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Collections.Generic;
using System.Linq;
using RESTfulFramework.NET.Security;

namespace RESTfulFramework.NET.DataService
{
    /// <summary>
    /// WCF服务通用接口
    /// </summary>
    /// <typeparam name="TConfigurator">组件配置信息</typeparam>
    /// <typeparam name="TRequestModel">请求模型</typeparam>
    /// <typeparam name="TResponseModel">输出模型</typeparam>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public abstract class Service<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager, TController>
        : IService, IServiceContext<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager>
         where TConfigManager : IConfigManager<TConfigModel>, new()
         where TConfigModel : IConfigModel, new()
         where TUserCache : IUserCache<TUserInfoModel>, new()
         where TUserInfoModel : IBaseUserInfo, new()
         where TJsonSerialzer : IJsonSerialzer, new()
         where TDBHelper : IDBHelper, new()
         where TSmsManager : ISmsManager, new()
         where TLogManager : ILogManager, new()
         where TController : Controller<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager>, new()
    {


        #region 基础组件
        /// <summary>
        /// API接口转换为实例
        /// </summary>
        //public Factory.UnitsFactory<TUserInfoModel> UnitsFactoryContext { get; set; }
        /// <summary>
        /// 安全校验
        /// </summary>
        public  SecurityFactory<TConfigManager,TConfigModel, TUserCache, TUserInfoModel> SecurityFactoryContext { get; set; }

        /// <summary>
        /// 日志
        /// </summary>
        public TLogManager LogManager { get; set; }

        /// <summary>
        /// 序列化器组件
        /// </summary>
        public TJsonSerialzer Serialzer { get; set; }

        /// <summary>
        /// 数据库操作
        /// </summary>
        public TDBHelper DbHelper { get; set; }

        /// <summary>
        /// 用于短信的收发
        /// </summary>
        public TSmsManager SmsManager { get; set; }

        /// <summary>
        /// 用户缓存应该是被多个实例共享的
        /// </summary>
        public TUserCache UserCache { get; set; }

        public ConfigInfo ConfigInfo { get; set; }

        public Dictionary<string, string> RequestHeader { get; set; }

        /// <summary>
        /// 用于配置管理
        /// </summary>
        public TConfigManager ConfigManager { get; set; }

        public TUserInfoModel CurrUserInfo { get; set; }

        public string Token { get; set; }

        #endregion



        private ApiContext<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager> ApiContext
        {
            get
            {
                return new ApiContext<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager>
                {
                    ConfigInfo = ConfigInfo,
                    ConfigManager = ConfigManager,
                    CurrUserInfo = CurrUserInfo,
                    DbHelper = DbHelper,
                    SmsManager = SmsManager,
                    Token = Token,
                    UserCache = UserCache,
                    JsonSerialzer = Serialzer,
                    LogManager = LogManager,
                    RequestHeader = RequestHeader
                };
            }
        }

        private static System.Reflection.MethodInfo[] Methods { get; set; }

        static Service()
        {
            var business = new TController();
            Methods = business.GetType().GetMethods();
        }

        public Service()
        {
            Serialzer = new TJsonSerialzer();
            DbHelper = new TDBHelper();
            UserCache = new TUserCache();
            SmsManager = new TSmsManager();
            ConfigManager = new TConfigManager();
            LogManager = new TLogManager();

            SecurityFactoryContext = new SecurityFactory<TConfigManager, TConfigModel, TUserCache, TUserInfoModel>();
            RequestHeader = new Dictionary<string, string>();
            #region 获取用户基本信息
            try
            {
                var token = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["token"];
                if (!string.IsNullOrEmpty(token))
                {
                    CurrUserInfo = UserCache.GetUserInfo(token);
                    Token = token;
                }
            }
            catch { }

            #endregion

            #region 获取请求的包头信息
            try
            {

                //取header
                foreach (var item in WebOperationContext.Current.IncomingRequest.Headers.AllKeys)
                {
                    try
                    {
                        RequestHeader.Add(item, WebOperationContext.Current.IncomingRequest.Headers[item]);
                    }
                    catch { }
                }

            }
            catch { }
            #endregion
        }

        /// <summary>
        /// GET通用接口
        /// </summary>
        /// <param name="body">主要信息</param>
        /// <param name="token">token</param>
        /// <param name="api">api</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="sign">签名</param>
        /// <returns>返回流</returns>
        public virtual Stream Get(string body, string token, string api, string timestamp, string sign)
        {

            try
            {
                LogManager?.Info($"接收请求：body={body}&token={token}&api={api}&timestamp={timestamp}&sign={sign}");
                object bodyObejct;
                try
                {
                    bodyObejct = StringToObject(body);
                }
                catch (Exception)
                {
                    return ResponseModelToStream(new ResponseModel { Code = Code.JsonInvalid, Msg = "无效的JSON。请检查JSON格式是否正确" });
                }


                var requestModel = new RequestModel<TUserInfoModel>
                {
                    Body = bodyObejct,
                    Token = token,
                    Api = api,
                    Timestamp = timestamp,
                    Sign = sign,
                    BodyString = body
                };

                var securityResult = SecurityCheck(requestModel);
                if (!securityResult.Item1) return ResponseModelToStream(new ResponseModel { Code = securityResult.Item3, Msg = securityResult.Item2 });

                var _requestModel = new RequestModel<TUserInfoModel>
                {
                    Body = requestModel.Body,
                    Token = requestModel.Token,
                    Api = requestModel.Api,
                    Timestamp = requestModel.Timestamp,
                    Sign = requestModel.Sign,
                    BodyString = requestModel.BodyString
                };

                ResponseModel result = ApiHandler(_requestModel);

                return ResponseModelToStream(result);
            }
            catch (Exception ex)
            {

                return ResponseModelToStream(new ResponseModel { Code = Code.SystemException, Msg = ex.Message });

            }
        }

        /// <summary>
        /// POST通用接口
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="token">token</param>
        /// <param name="api">api</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="sign">签名</param>
        /// <returns>返回流</returns>
        public virtual Stream Post(Stream stream, string token, string api, string timestamp, string sign)
        {
            var body = StreamToString(stream);
            return Get(body, token, api, timestamp, sign);
        }

        /// <summary>
        /// 获取信息通用接口(不用token)
        /// </summary>
        public Stream PostInfo(Stream stream, string api)
        {
            var body = StreamToString(stream);
            return GetInfo(body, api);
        }


        /// <summary>
        /// 获取信息通用接口(不用token)
        /// </summary>
        public Stream GetInfo(string body, string api)
        {
            var requestModel = new RequestModel<TUserInfoModel>
            {
                Body = StringToObject(body),
                Api = api,
                BodyString = body
            };

            ResponseModel result = InfoApiHandler(requestModel);
            return ResponseModelToStream(result);
        }


        public Stream GetStream(string body, string api)
        {
            var requestModel = new RequestModel<TUserInfoModel>
            {
                Body = StringToObject(body),
                Api = api,
                BodyString = body
            };

            Stream result = StreamApiHandler(requestModel);
            return result;
        }

        /// <summary>
        /// 安全检查
        /// </summary>
        /// <param name="requestModel">请求的模型</param>
        /// <returns>验证成功返回true,失败返回false</returns>
        public abstract Tuple<bool, string, int> SecurityCheck(RequestModel<TUserInfoModel> requestModel);

        /// <summary>
        /// 将要输出的对像转为流
        /// </summary>
        /// <param name="responseModel">输出的对像</param>
        /// <returns>流</returns>
        public abstract Stream ResponseModelToStream(ResponseModel responseModel);

        /// <summary>
        /// 将接收的二进制转为请求的对像
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>对像模型</returns>
        public abstract string StreamToString(Stream stream);

        /// <summary>
        /// 对像转为字符串，重写这个方法时，需要注意的是与StringToObject方法能互转
        /// </summary>
        public abstract string ObjectToString(object obj);

        /// <summary>
        /// 字符串转为对像，重写这个方法时，需要注意的是与ObjectToString方法能互转
        /// </summary>
        public abstract object StringToObject(string str);

        /// <summary>
        /// 处理TOKEN请求
        /// </summary>
        protected virtual ResponseModel ApiHandler(RequestModel<TUserInfoModel> requestModel)
        {
            var business = new TController();
            business.Context = Context;
            var method = Methods.FirstOrDefault(m => m.Name == requestModel.Api);
            if (method != null)
            {
                var requestModels = new RequestModel<TUserInfoModel>[1];
                requestModels[0] = requestModel;
                return (ResponseModel)method.Invoke(business, requestModels);
            }
            throw new Exception($"未找到合适的方法 {requestModel.Api}");
        }

        /// <summary>
        /// 取信息请求(不用验证)
        /// </summary>
        protected virtual ResponseModel InfoApiHandler(RequestModel<TUserInfoModel> requestModel)
        {
            var business = new TController();
            business.Context = Context;
            var method = Methods.FirstOrDefault(m => m.Name == requestModel.Api);
            if (method != null)
            {
                var requestModels = new RequestModel<TUserInfoModel>[1];
                requestModels[0] = requestModel;
                return (ResponseModel)method.Invoke(business, requestModels);
            }
            throw new Exception($"未找到合适的方法 {requestModel.Api}");
        }

        /// <summary>
        /// 获 取流数据
        /// </summary>
        protected virtual Stream StreamApiHandler(RequestModel<TUserInfoModel> requestModel)
        {
            var business = new TController();
            business.Context = Context;
            var method = Methods.FirstOrDefault(m => m.Name == requestModel.Api);
            if (method != null)
            {
                var requestModels = new RequestModel<TUserInfoModel>[1];
                requestModels[0] = requestModel;
                return (Stream)method.Invoke(business, requestModels);
            }
            throw new Exception($"未找到合适的方法 {requestModel.Api}");
        }

        public ApiContext<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager> Context
        {
            get
            {
                return ApiContext;
            }
        }
    }
}
