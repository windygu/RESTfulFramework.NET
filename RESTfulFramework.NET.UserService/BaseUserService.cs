using RESTfulFramework.NET.Common;
using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Activation;

namespace RESTfulFramework.NET.UserService
{
    /// <summary>
    /// 用户权限的基础服务
    /// </summary>
    /// <typeparam name="TDBHelper">数据库操作</typeparam>
    /// <typeparam name="TSmsManager">短信收发操作</typeparam>
    /// <typeparam name="TUserCache">用户缓存</typeparam>
    /// <typeparam name="TConfigManager">配置管理</typeparam>
    /// <typeparam name="TUserInfoModel">用户信息</typeparam>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BaseUserService<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager>
        : IUserService, IServiceContext<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager>
          where TConfigManager : IConfigManager<TConfigModel>, new()
         where TConfigModel : IConfigModel, new()
         where TUserCache : IUserCache<TUserInfoModel>, new()
         where TUserInfoModel : IBaseUserInfo, new()
         where TJsonSerialzer : IJsonSerialzer, new()
         where TDBHelper : IDBHelper, new()
         where TSmsManager : ISmsManager, new()
         where TLogManager : ILogManager, new()
    {

        #region ======  基础组件  ======

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

        public RESTfulFramework.NET.ComponentModel.ConfigInfo ConfigInfo { get; set; }



        /// <summary>
        /// 用于配置管理
        /// </summary>
        public TConfigManager ConfigManager { get; set; }


        public TUserInfoModel CurrUserInfo { get; set; }

        public string Token { get; set; }

        public Dictionary<string, string> RequestHeader { get; set; }

        #endregion

        #region ======   初始化   ======
        public BaseUserService()
        {
            #region 实例化基础组件
            UserCache = new TUserCache();
            SmsManager = new TSmsManager();
            ConfigManager = new TConfigManager();
            Serialzer = new TJsonSerialzer();
            LogManager = new TLogManager();
            DbHelper = new TDBHelper();
            DbHelper.ConnectionString = ConfigManager.GetConnectionString();
            #endregion

            #region 设置头部信息
            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                //WebOperationContext.Current.OutgoingResponse.ContentType = "application/json;charset=utf-8";
            }
            #endregion

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

            #region 获取基础配置信息
            try
            {
                ConfigInfo = new ConfigInfo
                {
                    AccountSecretKey = ConfigManager?.GetValue("account_secret_key")?.value,
                    SmsAccount = ConfigManager?.GetValue("sms_account")?.value,
                    SmsPassword = ConfigManager?.GetValue("sms_password")?.value,
                    SmsCodeContent = ConfigManager?.GetValue("sms_code_content")?.value,
                    RedisAddress = ConfigManager?.GetValue("redis_address")?.value,
                    RedisPort = ConfigManager?.GetValue("redis_port")?.value,
                };
            }
            catch (Exception ex)
            {
            }
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

        #region ====== 用户上下文 ======
        public ApiContext<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager> ApiContext
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
            private set { }
        }
        public ApiContext<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager> Context
        {
            get
            {
                return ApiContext;
            }
        }
        #endregion

        #region ======  用户接口  ======


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>返回用户信息</returns>
        public virtual UserResponseModel<BaseUserInfo> GetUserInfo(string token)
        {
            if (CurrUserInfo == null)
            {
                return new UserResponseModel<BaseUserInfo>
                {
                    Code = Code.TokenError,
                    Msg = new BaseUserInfo
                    {
                        account_name = CurrUserInfo?.account_name,
                        account_type_id = CurrUserInfo?.account_type_id,
                        client_id = CurrUserInfo?.client_id,
                        create_time = CurrUserInfo?.create_time,
                        id = CurrUserInfo == null ? Guid.Empty : CurrUserInfo.id,
                        real_name = CurrUserInfo?.real_name
                    }
                };
            }
            //var userInfo = (BaseUserInfo)CurrUserInfo;
            //userInfo.passwrod = "";
            //userInfo.client_id = "";
            return new UserResponseModel<BaseUserInfo>
            {
                Code = Code.Sucess,
                Msg = new BaseUserInfo
                {
                    account_name = CurrUserInfo?.account_name,
                    account_type_id = CurrUserInfo?.account_type_id,
                    client_id = CurrUserInfo?.client_id,
                    create_time = CurrUserInfo?.create_time,
                    id = CurrUserInfo == null ? Guid.Empty : CurrUserInfo.id,
                    real_name = CurrUserInfo?.real_name
                }
            };
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="sign">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <returns>登陆结果</returns>
        public virtual UserResponseModel<TokenModel> Login(string username, string sign, string timestamp)
        {
            return Login2(username, sign, timestamp, null);
        }


        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="sign">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="clientid">客户端ID</param>
        /// <returns>登陆结果</returns>
        public virtual UserResponseModel<TokenModel> Login2(string username, string sign, string timestamp, string clientid)
        {
            //从数据库取用户
            List<Dictionary<string, object>> user = DbHelper.QuerySql<List<Dictionary<string, object>>>($"SELECT * FROM `user` WHERE account_name='{username}';");
            if (user == null || !user.Any()) return new UserResponseModel<TokenModel> { Code = Code.AccountException, Msg = new TokenModel { Token = "用户不存在", UserId = "" } };

            //校验签名
            var _sign = Md5.GetMd5(username + user[0]["passwrod"].ToString() + timestamp + ConfigInfo.AccountSecretKey, Encoding.UTF8);
            if (sign != _sign) return new UserResponseModel<TokenModel> { Code = Code.SignErron, Msg = new TokenModel { Token = "签名不正确", UserId = "" } };


            //产生TOKEN
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(Md5.GetMd5(sign + Guid.NewGuid(), Encoding.UTF8))).Replace("=", "");
            Token = token;

            //写入redis服务
            var redisuser = UserCache.GetUserInfo(user[0]["id"].ToString());
            redisuser.client_id = clientid;
            CurrUserInfo = redisuser;

            if (UserCache.SetUserInfo(redisuser, token)) return new UserResponseModel<TokenModel> { Code = Code.Sucess, Msg = new TokenModel { Token = token, UserId = redisuser.id.ToString() } };
            else return new UserResponseModel<TokenModel> { Code = Code.SystemException, Msg = new TokenModel { Token = token, UserId = redisuser.id.ToString() } };
        }

        /// <summary>
        /// 退出登陆
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>退出结果</returns>
        public virtual UserResponseModel<string> LoginOut(string token)
        {

            if (UserCache.RemoveUserInfo(token)) return new UserResponseModel<string> { Code = Code.Sucess, Msg = "您已退出。" };
            if (UserCache.ContainsUserInfo(token)) return new UserResponseModel<string> { Code = Code.SystemException, Msg = "退出失败，请重试。" };
            else return new UserResponseModel<string> { Code = Code.TokenError, Msg = "token不存在，或已退出。" };
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="smscode">短信验证码</param>
        /// <param name="realname">真实姓名</param>
        /// <returns>返回注册结果</returns>
        public virtual UserResponseModel<string> Register(string username, string password, string smscode, string realname)
        {
            ////判断验证码
            if (!(UserCache.Contains(username) && UserCache.GetValue(username) == smscode))
                return new UserResponseModel<string> { Code = Code.ValCodeError, Msg = "验证码错误" };
            var result = DbHelper.QuerySql<List<Dictionary<string, object>>>($"SELECT * FROM `user` WHERE account_name='{username}'");
            if (result != null && result.Any()) return new UserResponseModel<string> { Code = Code.AccountExsit, Msg = "帐号已存在" };

            var userid = Guid.NewGuid();
            var resultInt = DbHelper.ExcuteSql($"INSERT INTO `user` (id,account_name,passwrod,account_type,realname) VALUES ('{userid}','{username}','{password}','手机','{realname}')");
            if (resultInt > 0)
            {
                UserCache.RefreshCache();
                return new UserResponseModel<string> { Code = Code.Sucess, Msg = "注册成功" };
            }
            return new UserResponseModel<string> { Code = Code.SystemException, Msg = "注册失败" };
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="smscode">短信验证码</param>
        /// <returns>返回结果</returns>
        public virtual UserResponseModel<string> ForgetPassword(string username, string password, string smscode)
        {
            ////判断验证码
            if (!(UserCache.Contains(username) && UserCache.GetValue(username) == smscode))
                return new UserResponseModel<string> { Code = Code.ValCodeError, Msg = "验证码错误" };
            var result = DbHelper.QuerySql<List<Dictionary<string, object>>>($"SELECT * FROM `user` WHERE account_name='{username}'");
            if (result == null) return new UserResponseModel<string> { Code = Code.AccountExsit, Msg = "帐号不存在" };

            var userid = Guid.NewGuid();
            var resultInt = DbHelper.ExcuteSql($"UPDATE `user` SET passwrod='{password.Replace("'", "''")}' WHERE  account_name='{username.Replace("'", "''")}'; ");
            if (resultInt > 0)
            {
                UserCache.RefreshCache();
                return new UserResponseModel<string> { Code = Code.Sucess, Msg = "更改成功" };
            }
            return new UserResponseModel<string> { Code = Code.SystemException, Msg = "更改失败" };
        }


        /// <summary>
        /// 请求短信验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns>返回请求结果</returns>
        public virtual UserResponseModel<string> SendSmsCode(string phone)
        {
            ////产生验证码
            var rendomCode = Common.Random.CreateSmsCode();

            var content = ConfigInfo.SmsCodeContent.Replace("{code}", rendomCode);
            UserCache.SetValue(rendomCode, phone);

            //发送验证码
            //var result = SmsManager.SendSms(phone, content);

            //返回结果
            if (SendSms(phone, content))
                return new UserResponseModel<string> { Code = Code.Sucess, Msg = "已发送验证码。" };
            else
                return new UserResponseModel<string> { Code = Code.SmsCodeFail, Msg = "短信验证码发送失败。" }; //发送失败
        }

        /// <summary>
        /// 判断短信验证码是否存在
        /// </summary>
        /// <param name="code">短信验证码</param>
        /// <returns>返回结果</returns>
        public virtual UserResponseModel<string> SmsCodeExist(string code)
        {

            return UserCache.GetAll().ContainsValue(code) ? new UserResponseModel<string> { Code = Code.Sucess, Msg = "验证码存在" } : new UserResponseModel<string> { Code = Code.ValCodeError, Msg = "验证码错误" };
        }

        protected virtual bool SendSms(string phone, string content)
        {
            return true;
        }

        protected virtual bool ValidateSmsCode(string phone, string smscode)
        {
            return UserCache.GetValue(phone) == smscode;
        }

        public UserResponseModel<string> IsLogin(string token)
        {
            if (CurrUserInfo != null) return new UserResponseModel<string> { Code = Code.Sucess, Msg = "您已登陆。" };
            return new UserResponseModel<string> { Code = Code.TokenError, Msg = "登陆已失效。" };
        }

        #endregion

        #region ======  刷新缓存  ======
        public virtual void RefreshCurrUserInfo()
        {
            try
            {
                UserCache.RefreshCache();
                var redisuser = UserCache.GetUserInfo(CurrUserInfo.id.ToString());
                CurrUserInfo = redisuser;
                UserCache.SetUserInfo(redisuser, Token);
            }
            catch { }
        }
        #endregion

    }
}
