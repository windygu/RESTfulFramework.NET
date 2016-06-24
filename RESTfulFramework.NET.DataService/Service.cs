using RESTfulFramework.NET.ComponentModel;
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
    /// RESTful服务
    /// </summary>
    /// <typeparam name="TConfigManager">配置管理器，用于设置和读取配置。即所有与配置相关的都使用该TConfigManager管理、读取、设置。</typeparam>
    /// <typeparam name="TConfigModel">配置模型，配置管理器依赖于此模型。该模型定义了配置信息的字段，例如：时间、地址、人物。</typeparam>
    /// <typeparam name="TUserCache">缓存管理，用户信息、token及自定义信息都可以缓存下来。该泛型定义了缓存的方式，或内存或数据库或redis</typeparam>
    /// <typeparam name="TUserInfoModel">用户信息模型，如用户名、用户性别</typeparam>
    /// <typeparam name="TJsonSerialzer">序列化器，可以使用性能更佳的序列化器</typeparam>
    /// <typeparam name="TDBHelper">数据库操作</typeparam>
    /// <typeparam name="TSmsManager">短信管理</typeparam>
    /// <typeparam name="TLogManager">日志</typeparam>
    /// <typeparam name="TController">控制器，该泛型决定了http请求调用哪个控制器。</typeparam>
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


        #region ======  基础组件  ======

        /// <summary>
        /// 安全校验
        /// </summary>
        public SecurityFactory<TConfigManager, TConfigModel, TUserCache, TUserInfoModel> SecurityFactoryContext { get; set; }

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

        /// <summary>
        /// 基础的必须的配置信息
        /// </summary>
        public ConfigInfo ConfigInfo { get; set; }


        /// <summary>
        /// http请求的头部信息
        /// </summary>
        public Dictionary<string, string> RequestHeader { get; set; }

        /// <summary>
        /// 用于配置管理
        /// </summary>
        public TConfigManager ConfigManager { get; set; }

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public TUserInfoModel CurrUserInfo { get; set; }

        /// <summary>
        /// 当前用户token
        /// </summary>
        public string Token { get; set; }

        #endregion

        #region  ====== 用户上下文 ======
        /// <summary>
        /// 当前用户请求的上下文信息，包括了一些基础组件
        /// </summary>
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

        /// <summary>
        /// 当前用户请求的上下文信息，包括了一些基础组件
        /// </summary>
        public ApiContext<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager> Context
        {
            get
            {
                return ApiContext;
            }
        }
        #endregion

        #region ======   初始化   ======
        /// <summary>
        /// TController控制器定义的所有方法
        /// </summary>
        private static System.Reflection.MethodInfo[] Methods { get; set; }


        /// <summary>
        /// 静态初始化
        /// </summary>
        static Service()
        {
            var business = new TController();
            Methods = business.GetType().GetMethods();
        }



        /// <summary>
        /// 初始化
        /// </summary>
        public Service()
        {
            Serialzer = new TJsonSerialzer();
            UserCache = new TUserCache();
            SmsManager = new TSmsManager();
            ConfigManager = new TConfigManager();
            LogManager = new TLogManager();
            DbHelper = new TDBHelper();
            DbHelper.ConnectionString = ConfigManager.GetConnectionString();
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

        #endregion

        #region ======  接口服务  ======

        /// <summary>
        /// GET通用接口
        /// </summary>
        /// <param name="body">主要信息</param>
        /// <param name="token">token</param>
        /// <param name="api">api，即与控制器内的方法名对应</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="sign">签名</param>
        /// <returns>返回流</returns>
        [RecordLog]
        public virtual Stream Get(string body, string token, string api, string timestamp, string sign)
        {
            try
            {
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
        /// <param name="stream">数据流</param>
        /// <param name="api">api，即与控制器内的方法名对应</param>
        /// <returns></returns>
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

        /// <summary>
        /// 一般用于下载二进制文件
        /// </summary>
        /// <param name="body">请求的内容</param>
        /// <param name="api">api，即与控制器内的方法名对应</param>
        /// <returns></returns>
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
        public virtual Tuple<bool, string, int> SecurityCheck(RequestModel<TUserInfoModel> requestModel) => SecurityFactoryContext.GetSecurityService().SecurityCheck(requestModel);

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
        #endregion

    }
}
