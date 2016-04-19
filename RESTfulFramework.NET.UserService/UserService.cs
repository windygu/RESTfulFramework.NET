using RESTfulFramework.NET.Common;
using RESTfulFramework.NET.Common.Model;
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units.Model;
using PluginPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Activation;

namespace RESTfulFramework.NET.UserService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UserService : IUserService
    {
        private static IDBHelper DbHelper { get; set; }
        private static ISmsManager SmsManager { get; set; }
        private static IUserCache<UserInfo> UserCache { get; set; }
        private static IPushManager<PushInfo> PushManager { get; set; }

        public UserService()
        {
            #region 设置头部信息
            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json;charset=utf-8";
            }
            #endregion 
        }
        static UserService()
        {
            try
            {
                DbHelper = Factory.GetInstance<IDBHelper>();
                SmsManager = Factory.GetInstance<ISmsManager>();
                UserCache = Factory.GetInstance<IUserCache<UserInfo>>();
                PushManager = Factory.GetInstance<IPushManager<PushInfo>>();
                ConfigInfo.SmsCodeDictionary = new Dictionary<string, string>();
            }
            catch (Exception){}

        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>返回用户信息</returns>
        public UserResponseModel<UserInfo> GetUserInfo(string token)
        {
            return new UserResponseModel<UserInfo> { Code = Code.Sucess, Msg = UserCache.GetUserInfo(token) };
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="sign">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <returns>登陆结果</returns>
        public UserResponseModel<TokenModel> Login(string username, string sign, string timestamp)
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
        public UserResponseModel<TokenModel> Login2(string username, string sign, string timestamp, string clientid)
        {
            //从数据库取用户
            List<Dictionary<string, object>> user = DbHelper.QuerySql<List<Dictionary<string, object>>>($"SELECT * FROM `user` WHERE account_name='{username}';");
            if (user == null || !user.Any()) return new UserResponseModel<TokenModel> { Code = Code.AccountException, Msg = new TokenModel { Token = "用户不存在", UserId = "" } };

            //校验签名
            var _sign = Md5.GetMd5(username + user[0]["passwrod"].ToString() + timestamp + ConfigInfo.AccountSecretKey, Encoding.UTF8);
            if (sign != _sign) return new UserResponseModel<TokenModel> { Code = Code.SignErron, Msg = new TokenModel { Token = "签名不正确", UserId = "" } };


            //产生TOKEN
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(Md5.GetMd5(sign + Guid.NewGuid(), Encoding.UTF8)));

            //写入redis服务
            var redisuser = new UserInfo
            {
                account_name = user[0]["account_name"].ToString(),
                account_type_id = user[0]["account_type"].ToString(),
                create_time = Convert.ToDateTime(user[0]["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                id = Guid.Parse(user[0]["id"].ToString()),
                passwrod = user[0]["passwrod"].ToString(),
                real_name = user[0]["realname"].ToString(),
                client_id = clientid
            };

            if (UserCache.SetUserInfo(redisuser, token)) return new UserResponseModel<TokenModel> { Code = Code.Sucess, Msg = new TokenModel { Token = token, UserId = redisuser.id.ToString() } };
            else return new UserResponseModel<TokenModel> { Code = Code.SystemException, Msg = new TokenModel { Token = token, UserId = redisuser.id.ToString() } };
        }

        /// <summary>
        /// 退出登陆
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>退出结果</returns>
        public UserResponseModel<string> LoginOut(string token)
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
        public UserResponseModel<string> Register(string username, string password, string smscode, string realname)
        {
            ////判断验证码
            if (!ConfigInfo.SmsCodeDictionary.Contains(new KeyValuePair<string, string>(username, smscode)))
                return new UserResponseModel<string> { Code = Code.ValCodeError, Msg = "验证码错误" };
            var result = DbHelper.QuerySql<List<Dictionary<string, object>>>($"SELECT * FROM `user` WHERE account_name='{username}'");
            if (result != null && result.Any()) return new UserResponseModel<string> { Code = Code.AccountExsit, Msg = "帐号已存在" };

            var userid = Guid.NewGuid();
            var resultInt = DbHelper.ExcuteSql($"INSERT INTO `user` (id,account_name,passwrod,account_type,realname) VALUES ('{userid}','{username}','{password}','手机','{realname}')");
            if (resultInt > 0) return new UserResponseModel<string> { Code = Code.Sucess, Msg = "注册成功" };
            return new UserResponseModel<string> { Code = Code.SystemException, Msg = "注册失败" };
        }

        /// <summary>
        /// 请求短信验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns>返回请求结果</returns>
        public UserResponseModel<string> SendSmsCode(string phone)
        {
            ////产生验证码
            var rendomCode = Common.Random.CreateSmsCode();

            var content = ConfigInfo.SmsCodeContent.Replace("{code}", rendomCode);

            if (ConfigInfo.SmsCodeDictionary.ContainsKey(phone))
                ConfigInfo.SmsCodeDictionary[phone] = rendomCode;
            else
                ConfigInfo.SmsCodeDictionary.Add(phone, rendomCode);

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
        public UserResponseModel<string> SmsCodeExist(string code) => ConfigInfo.SmsCodeDictionary.ContainsValue(code) ? new UserResponseModel<string> { Code = Code.Sucess, Msg = "验证码存在" } : new UserResponseModel<string> { Code = Code.ValCodeError, Msg = "验证码错误" };

        protected virtual bool SendSms(string phone, string content)
        {

            var result = PushManager.PushInfo(new PushInfo
            {
                CliendId = "28b10506ba167e0ab4e9fba606dd035e",
                Title = "注册验证码",
                Content = content,
                Descript = content
            });
            return result;
        }

        protected virtual bool ValidateSmsCode(string phone, string smscode) => ConfigInfo.SmsCodeDictionary.Contains(new KeyValuePair<string, string>(phone, smscode));


    }
}
