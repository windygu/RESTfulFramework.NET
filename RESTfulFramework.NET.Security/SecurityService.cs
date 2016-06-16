using RESTfulFramework.NET.ComponentModel;
using System;

namespace RESTfulFramework.NET.Security
{
    public class SecurityService<TConfigManager, TConfigModel, TUserCache, TUserInfoModel> : ISecurity<RequestModel<TUserInfoModel>>
        where TConfigManager : IConfigManager<TConfigModel>, new()
        where TConfigModel : IConfigModel, new()
        where TUserCache : IUserCache<TUserInfoModel>, new()
        where TUserInfoModel : IBaseUserInfo, new()
    {
        public Tuple<bool, string, int> SecurityCheck(RequestModel<TUserInfoModel> requestModel)
        {
            var dataCheck = new DataCheck<TUserInfoModel>();
            var securityCheckResult = dataCheck.SecurityCheck(requestModel);
            if (!securityCheckResult.Item1) return securityCheckResult;

            var userSecurity = new UserSecurity<TUserCache, TUserInfoModel>();
            var userSecurityCheckResult = userSecurity.SecurityCheck(requestModel);
            if (!userSecurityCheckResult.Item1) return userSecurityCheckResult;

            var dataSecurity = new DataSecurity<TConfigManager, TConfigModel, TUserInfoModel>();
            var dataSecurityCheckResult = dataSecurity.SecurityCheck(requestModel);
            if (!dataSecurityCheckResult.Item1) return dataSecurityCheckResult;

            return new Tuple<bool, string, int>(true, "通过安全检查", Code.Sucess);
        }
    }
}
