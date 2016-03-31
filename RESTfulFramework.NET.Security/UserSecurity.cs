using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units.Model;
using PluginPackage;
using System;

namespace RESTfulFramework.NET.Security
{
    public class UserSecurity : ISecurity<RequestModel>
    {
        public UserSecurity() { }
        private static IUserCache<UserInfo> UserCache { get; set; }
        static UserSecurity()
        {
            UserCache = Factory.GetInstance<IUserCache<UserInfo>>();
        }
        public bool SecurityCheck(RequestModel requestModel)
        {
            try
            {
                var userInfo = UserCache.GetUserInfo(requestModel.Token);
                if (userInfo == null) return false;
                requestModel.UserInfo = userInfo;
            }
            catch(Exception ex)
            {
                throw new System.Exception($"查询不到指定的Token数据。{ex.Message}");
            }

            return true;
        }
    }
}
