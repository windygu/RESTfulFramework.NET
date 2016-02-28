﻿using RESTfulFramework.NET.Common;
using RESTfulFramework.NET.Common.Model;
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units.Model;
using PluginPackage;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Text;

namespace RESTfulFramework.NET.UserService
{
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
            DbHelper = Factory.GetInstance<IDBHelper>();
            SmsManager = Factory.GetInstance<ISmsManager>();
            UserCache = Factory.GetInstance<IUserCache<UserInfo>>();
            PushManager = Factory.GetInstance<IPushManager<PushInfo>>();

            ConfigInfo.SmsCodeDictionary = new Dictionary<string, string>();
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>返回用户信息</returns>
        public ResponseModel GetUserInfo(string token)
        {
            var redis = new RedisClient(ConfigInfo.RedisAddress, int.Parse(ConfigInfo.RedisPort));
            var user = redis.Get<UserInfo>(token);
            return new ResponseModel { Code = Code.Sucess, Msg = user };
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="sign">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <returns>登陆结果</returns>
        public ResponseModel Login(string username, string sign, string timestamp) => Login2(username, sign, timestamp, null);


        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="sign">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="clientid">客户端ID</param>
        /// <returns>登陆结果</returns>
        public ResponseModel Login2(string username, string sign, string timestamp, string clientid)
        {
            //从数据库取用户
            Dictionary<string, object> user = DbHelper.QuerySql<Dictionary<string, object>>($"SELECT * FROM `user` WHERE account_name='{username}';");
            if (user == null) return new ResponseModel { Code = Code.AccountException, Msg = "用户不存在。" };

            //校验签名
            var _sign = Md5.GetMd5(username + user["passwrod"].ToString() + timestamp + ConfigInfo.AccountSecretKey, Encoding.UTF8);
            if (sign != _sign) return new ResponseModel { Code = Code.SignErron, Msg = "签名不正确。" };

            //产生TOKEN
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(Md5.GetMd5(sign + Guid.NewGuid(), Encoding.UTF8)));

            //写入redis服务
            var redisuser = new UserInfo
            {
                account_name = user["account_name"].ToString(),
                account_type_id = user["account_type"].ToString(),
                create_time = Convert.ToDateTime(user["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                id = Guid.Parse(user["id"].ToString()),
                passwrod = user["passwrod"].ToString(),
                real_name = user["realname"].ToString(),
                client_id = clientid
            };

            if (UserCache.SetUserInfo(redisuser, token)) return new ResponseModel
            {
                Code = Code.Sucess,
                Msg = token
            };
            else return new ResponseModel
            {
                Code = Code.SystemException,
                Msg = token
            };
        }

        /// <summary>
        /// 退出登陆
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>退出结果</returns>
        public ResponseModel LoginOut(string token)
        {
 
            if (UserCache.RemoveUserInfo(token)) return new ResponseModel { Code = Code.Sucess, Msg = "您已退出。" };
            if (UserCache.ContainsUserInfo(token)) return new ResponseModel { Code = Code.SystemException, Msg = "退出失败，请重试。" };
            else return new ResponseModel { Code = Code.TokenError, Msg = "token不存在，或已退出。" };
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="smscode">短信验证码</param>
        /// <param name="realname">真实姓名</param>
        /// <returns>返回注册结果</returns>
        public ResponseModel Register(string username, string password, string smscode, string realname)
        {
            ////判断验证码
            if (!ConfigInfo.SmsCodeDictionary.Contains(new KeyValuePair<string, string>(username, smscode)))
                return new ResponseModel { Code = Code.ValCodeError, Msg = "验证码错误" };

            if (DbHelper.QuerySql<Dictionary<string, object>>($"SELECT * FROM `user` WHERE account_name='{username}'") != null) return new ResponseModel { Code = Code.AccountExsit, Msg = "帐号已存在" };

            var userid = Guid.NewGuid();
            var resultInt = DbHelper.ExcuteSql($"INSERT INTO `user` (id,account_name,passwrod,account_type,realname) VALUES ('{userid}','{username}','{password}','手机','{realname}')");
            if (resultInt > 0) return new ResponseModel { Code = Code.Sucess, Msg = "注册成功" };
            return new ResponseModel { Code = Code.SystemException, Msg = "注册失败" };
        }

        /// <summary>
        /// 请求短信验证码
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns>返回请求结果</returns>
        public ResponseModel SendSmsCode(string phone)
        {
            ////产生验证码
            var rendomCode = Common.Random.CreateSmsCode();

            var content = ConfigInfo.SmsCodeContent.Replace("{code}", rendomCode);

            if (ConfigInfo.SmsCodeDictionary.ContainsKey(phone))
                ConfigInfo.SmsCodeDictionary[phone] = rendomCode;
            else
                ConfigInfo.SmsCodeDictionary.Add(phone, rendomCode);

            //发送验证码
            //var reslut = SmsManager.SendSms(phone, content);
            var result = PushManager.PushInfo(new PushInfo
            {
                CliendId = "28b10506ba167e0ab4e9fba606dd035e",
                Title = "注册验证码",
                Content = content,
                Descript = content
            });


            //返回结果
            if (result)
                return new ResponseModel { Code = Code.Sucess, Msg = "已发送验证码。" };
            else
                return new ResponseModel { Code = Code.SmsCodeFail, Msg = "短信验证码发送失败。" }; //发送失败
        }

        /// <summary>
        /// 判断短信验证码是否存在
        /// </summary>
        /// <param name="code">短信验证码</param>
        /// <returns>返回结果</returns>
        public ResponseModel SmsCodeExist(string code) => ConfigInfo.SmsCodeDictionary.ContainsValue(code) ? new ResponseModel { Code = Code.Sucess, Msg = "验证码存在" } : new ResponseModel { Code = Code.ValCodeError, Msg = "验证码错误" };


    }
}