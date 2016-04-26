using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units.Model;
using System;

namespace RESTfulFramework.NET.Security
{
    public class UserSecurity : ISecurity<RequestModel>
    {
        private IUserCache<UserInfo> UserCache { get; set; }
        public UserSecurity()
        {
            UserCache = new Factory.UnitsFactory<RequestModel, ResponseModel>().UserCache;
        }
        public bool SecurityCheck(RequestModel requestModel)
        {
            try
            {
                var userInfo = UserCache.GetUserInfo(requestModel.Token);
                if (userInfo == null) return false;
                requestModel.UserInfo = userInfo;
            }
            catch (Exception ex)
            {
                throw new System.Exception($"查询不到指定的Token数据。{ex.Message}");
            }

            return true;
        }
    }
}
