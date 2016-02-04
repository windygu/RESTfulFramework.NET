using RESTfulFramework.IProtocolPlugin.Model;
using System.ComponentModel.Composition;
using System.Collections.Generic;
using System;
namespace RESTfulFramework.ProtocolLogin
{
    [Export(typeof(IProtocolPlugin.IProtocol))]
    public class Login : IProtocolPlugin.IProtocol
    {
        public Result<object> SetupProtocol(object body, string protocol, object user = null)
        {
            var result = body as Json2KeyValue.JsonObject;
            //判断登陆的帐号密码,仅用于演示
            var account = result.GetValue<string>("account");
            var password = result.GetValue<string>("password");
            if (account == "demo" && password == "demo888")
            {
                Login.SetUserState("demotoken", new UserInfo { Account = "demo", UserId = "demouserid" });
                return new Result<object>
                {
                    Code = 1,
                    Msg = "登陆成功!作为例子，仅供参考。url地址protocol参数决定着该调用哪个dll,此处的protocol的值是Login,因此调用的是Login.dll的SetupProtocol方法。更多RESTful接口你应该实现 IProtocolPlugin.IProtocol接口。"
                };
            }
            else {
                return new Result<object>
                {
                    Code = -1,
                    Msg = "登陆失败"
                };
            }
        }


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
            catch (Exception)
            {
                return null;
            }



        }

    }


    public class UserInfo
    {
        public string UserId { get; set; }
        public string Account { get; set; }

    }
}
