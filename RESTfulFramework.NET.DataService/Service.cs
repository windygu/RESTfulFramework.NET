﻿using RESTfulFramework.NET.ComponentModel;
using System.IO;
using System;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace RESTfulFramework.NET.DataService
{
    /// <summary>
    /// WCF服务通用接口
    /// </summary>
    /// <typeparam name="TConfigurator">组件配置信息</typeparam>
    /// <typeparam name="TRequestModel">请求模型</typeparam>
    /// <typeparam name="TResponseModel">输出模型</typeparam>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public abstract class Service<TConfigManager, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager>
        : IService<TUserInfoModel>, IServiceContext<TUserInfoModel>
         where TConfigManager : IConfigManager<SysConfigModel>, new()
         where TUserCache : IUserCache<TUserInfoModel>, new()
         where TUserInfoModel : BaseUserInfo, new()
         where TJsonSerialzer : IJsonSerialzer, new()
         where TDBHelper : IDBHelper, new()
         where TSmsManager : ISmsManager, new()
         where TLogManager : ILogManager, new()

    {
        #region 基础组件
        /// <summary>
        /// API接口转换为实例
        /// </summary>
        public Factory.UnitsFactory<TUserInfoModel> UnitsFactoryContext { get; set; }
        /// <summary>
        /// 安全校验
        /// </summary>
        public Factory.SecurityFactory<TConfigManager, TUserCache, TUserInfoModel> SecurityFactoryContext { get; set; }

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


        /// <summary>
        /// 用于配置管理
        /// </summary>
        public TConfigManager ConfigManager { get; set; }

        public TUserInfoModel CurrUserInfo { get; set; }

        public string Token { get; set; }

        #endregion

        private ApiContext<TUserInfoModel> ApiContext
        {
            get
            {
                return new ApiContext<TUserInfoModel>
                {
                    ConfigInfo = ConfigInfo,
                    ConfigManager = ConfigManager,
                    CurrUserInfo = CurrUserInfo,
                    DbHelper = DbHelper,
                    SmsManager = SmsManager,
                    Token = Token,
                    UserCache = UserCache,
                    JsonSerialzer = Serialzer,
                    LogManager = LogManager
                };

            }
        }



        public Service()
        {
            Serialzer = new TJsonSerialzer();
            DbHelper = new TDBHelper();
            UserCache = new TUserCache();
            SmsManager = new TSmsManager();
            ConfigManager = new TConfigManager();
            LogManager = new TLogManager();
            UnitsFactoryContext = new Factory.UnitsFactory<TUserInfoModel>();
            SecurityFactoryContext = new Factory.SecurityFactory<TConfigManager, TUserCache, TUserInfoModel>();

            #region 获取用户基本信息
            try
            {
                var token = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters["token"];
                if (!string.IsNullOrEmpty(token))
                {
                    CurrUserInfo = UserCache?.GetUserInfo(token);
                    Token = token;
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
                var requestModel = new RequestModel<BaseUserInfo>
                {
                    Body = StringToObject(body),
                    Token = token,
                    Api = api,
                    Timestamp = timestamp,
                    Sign = sign,
                    BodyString = body
                };

                var securityResult = SecurityCheck(requestModel);
                if (!securityResult) return ResponseModelToStream(new ResponseModel { Code = Code.NoAllow, Msg = "权限不足" });

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
        public abstract bool SecurityCheck(RequestModel<BaseUserInfo> requestModel);

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
            var tokenApi = UnitsFactoryContext.GetTokenApi(requestModel.Api);
            tokenApi.Context = ApiContext;
            return tokenApi.RunApi(requestModel);
        }


        /// <summary>
        /// 取信息请求(不用验证)
        /// </summary>
        protected virtual ResponseModel InfoApiHandler(RequestModel<TUserInfoModel> requestModel)
        {
            return UnitsFactoryContext.GetInfoApi(requestModel.Api).RunApi(requestModel);
        }


        /// <summary>
        /// 获 取流数据
        /// </summary>
        protected virtual Stream StreamApiHandler(RequestModel<TUserInfoModel> requestModel)
        {
            return UnitsFactoryContext.GetStreamApi(requestModel.Api).RunApi(requestModel);
        }


        public ApiContext<TUserInfoModel> Context
        {
            get
            {
                return ApiContext;
            }
        }
    }
}
