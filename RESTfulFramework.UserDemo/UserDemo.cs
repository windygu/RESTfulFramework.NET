using System;
using System.ComponentModel.Composition;

namespace RESTfulFramework.UserDemo
{
    [Export(typeof(IUserPlugin.IUser))]
    public class UserDemo : IUserPlugin.IUser
    {
        public object GetUser(string token)
        {
            /*根据token取用户信息，当分布式时，您可以使用redis，此处仅作参考*/
            return ProtocolAccount.Account.GetUserState(token);
        }
    }
}
