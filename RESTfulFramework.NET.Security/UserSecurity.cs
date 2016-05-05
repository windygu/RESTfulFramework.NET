using RESTfulFramework.NET.ComponentModel;

using System;

namespace RESTfulFramework.NET.Security
{
    public class UserSecurity<TUserCache, TUserInfoModel> : ISecurity<RequestModel<BaseUserInfo>>
        where TUserCache : IUserCache<TUserInfoModel>, new()
        where TUserInfoModel : BaseUserInfo, new()
    {
        private TUserCache UserCache { get; set; }
        public UserSecurity()
        {
            UserCache = new TUserCache();
        }
        public bool SecurityCheck(RequestModel<BaseUserInfo> requestModel)
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
