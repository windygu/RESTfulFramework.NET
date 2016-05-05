using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Security
{
    public class SecurityService<TConfigManager, TUserCache, TUserInfoModel> : ISecurity<RequestModel<BaseUserInfo>>
        where TConfigManager : IConfigManager<SysConfigModel>, new()
       where TUserCache : IUserCache<TUserInfoModel>, new()
        where TUserInfoModel : BaseUserInfo, new()
    {
        public bool SecurityCheck(RequestModel<BaseUserInfo> requestModel)
        {
            var dataCheck = new DataCheck();
            if (!dataCheck.SecurityCheck(requestModel)) return false;

            var userSecurity = new UserSecurity<TUserCache,TUserInfoModel>();
            if (!userSecurity.SecurityCheck(requestModel)) return false;

            var dataSecurity = new DataSecurity<TConfigManager>();
            if (!dataSecurity.SecurityCheck(requestModel)) return false;

            return true;
        }
    }
}
