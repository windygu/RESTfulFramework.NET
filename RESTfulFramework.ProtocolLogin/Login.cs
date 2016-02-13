using RESTfulFramework.IProtocolPlugin.Model;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System;
using System.Configuration;
using MySqlModel;
using RESTfulFramework.Common;
using System.Linq;
using RESTfulFramework.ProtocolAccount.Model;

namespace RESTfulFramework.ProtocolAccount
{
    [Export(typeof(IProtocolPlugin.IProtocol))]
    public class Account : IProtocolPlugin.IProtocol
    {
        public Result<object> SetupProtocol(object body, string api, string type, object user = null)
        {
            if (type == "Account")
            {
                var result = body as Json2KeyValue.JsonObject;
                DbSql.ConnectionString = ConfigurationManager.ConnectionStrings["RESTfulFrameworkConnection"].ConnectionString; ;

                #region 登陆
                if (api == "Login")
                {
                    var account = result.GetValue<string>("account");
                    var password = result.GetValue<string>("password");
                    var resultDictionary = DbSql.QuerySql<List<Dictionary<string, object>>>(
                        $"SELECT * FROM users WHERE account='{account}' AND password='{password}';");
                    if (resultDictionary != null && resultDictionary.Count > 0)
                    {
                        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        SetUserState(token, new UserInfo { Account = account, UserId = (int)resultDictionary[0]["id"] });
                        return new Result<object>
                        {
                            Code = CodeEnum.Sucess,
                            Msg = token
                        };
                    }
                    else
                    {
                        return new Result<object>
                        {
                            Code = CodeEnum.AccountException,
                            Msg = "登陆失败"
                        };
                    }
                }
                #endregion

                #region 注册
                if (api == "Register")
                {
                    var account = result.GetValue<string>("account");
                    var password = result.GetValue<string>("password");
                    var smscode = result.GetValue<string>("smscode");
                    //验证短信短信码是否一致
                    if (UserSmsCode.ContainsKey(account) && UserSmsCode[account] == smscode)
                    {
                        //为用户表添加新用户
                        var resultInt = DbSql.ExcuteSql($"INSERT INTO users (account,password) VALUES ('{account}','{password}');");
                        if (resultInt > 0)
                        {
                            //注册成功
                            //删除短信验证码
                            UserSmsCode.Remove(account);
                            return new Result<object>
                            {
                                Code = CodeEnum.Sucess,
                                Msg = "注册成功"
                            };
                        }
                        else
                        {
                            return new Result<object>
                            {
                                Code = CodeEnum.SystemException,
                                Msg = "系统维护，注册失败"
                            };
                        }
                    }
                    else
                    {
                        //短信验证码不存在或不正确
                        return new Result<object>
                        {
                            Code = CodeEnum.SmsCodeFail,
                            Msg = "短信验证码有误"
                        };
                    }


                }
                #endregion

                #region 忘记密码 修改密码
                if (api == "ForgetPassword" || api == "ModifyPassword")
                {
                    var account = result.GetValue<string>("account");
                    var newpassword = result.GetValue<string>("newpassword");
                    var smscode = result.GetValue<string>("smscode");
                    //验证短信短信码是否一致
                    if (UserSmsCode.ContainsKey(account) && UserSmsCode[account] == smscode)
                    {
                        //为用户表添加新用户
                        var resultInt = DbSql.ExcuteSql($"UPDATE users SET password='{newpassword}' WHERE account='{account}'; ");
                        if (resultInt > 0)
                        {
                            //修改成功
                            //删除短信验证码
                            UserSmsCode.Remove(account);
                            return new Result<object>
                            {
                                Code = CodeEnum.Sucess,
                                Msg = "修改成功"
                            };
                        }
                        else
                        {
                            return new Result<object>
                            {
                                Code = CodeEnum.SystemException,
                                Msg = "系统维护，修改失败"
                            };
                        }
                    }
                    else
                    {
                        //短信验证码不存在或不正确
                        return new Result<object>
                        {
                            Code = CodeEnum.SmsCodeFail,
                            Msg = "短信验证码有误"
                        };
                    }

                }
                #endregion

                #region 请求发送短信验证码
                if (api == "SmsCode")
                {
                    var account = result.GetValue<string>("account");
     
                    //产生随机6位数数字
                    var randomCode = RandomEx.CreateSmsCode();
                    //保存
                    SetUserSmsCode(account, randomCode);
                    //发送短信

                    //发送序列号短信给用户
                    var sms = new SmsApi.WsAPIs();
                    var content = ConfigurationManager.AppSettings["VerificationSmsTemplate"].Replace("{code}", randomCode);
                    var resultXml = sms.Send("rest", "123456_abc", account, content, null);
                    var xmlResult = resultXml.GetObjectByXmlString<XMLResult>();
                    if (xmlResult.Result == "1")
                    {
                        return new Result<object>
                        {
                            Code = CodeEnum.Sucess,
                            Msg = xmlResult.Description
                        };
                    }
                    else {
                        return new Result<object>
                        {
                            Code = CodeEnum.Sucess,
                            Msg = xmlResult.Description
                        };
                    }

                }
                #endregion
            }
            return null;


        }

        /// <summary>
        /// 保存用户状态信息
        /// </summary>
        private static Dictionary<string, UserInfo> UsersState { get; set; } = new Dictionary<string, UserInfo>();


        public static void SetUserState(string token, UserInfo userinfo)
        {
            if (!UsersState.ContainsKey(token))
                UsersState.Add(token, userinfo);
        }

        public static object GetUserState(string token)
        {
            try
            {
                return UsersState[token];
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        /// <summary>
        /// 保存短信验证码
        /// </summary>
        private static Dictionary<string, string> UserSmsCode { get; set; } = new Dictionary<string, string>();

        public static void SetUserSmsCode(string account, string randomCode)
        {
            if (!UserSmsCode.ContainsKey(account))
                UserSmsCode.Add(account, randomCode);
        }

        public static object GetUserSmsCode(string account)
        {
            try
            {
                return UserSmsCode[account];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }



}
