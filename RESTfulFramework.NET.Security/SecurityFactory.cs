using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Security
{
    /// <summary>
    /// 安全相关工厂
    /// </summary>
    public class SecurityFactory<TConfigManager, TConfigModel, TUserCache, TUserInfoModel>
         where TConfigManager   : IConfigManager<TConfigModel>, new()
         where TConfigModel     : IConfigModel,new()
         where TUserCache       : IUserCache<TUserInfoModel>, new()
         where TUserInfoModel   : IBaseUserInfo, new()
    {
        public ISecurity<RequestModel<TUserInfoModel>> GetSecurityService() => new Security.SecurityService<TConfigManager, TConfigModel, TUserCache, TUserInfoModel>();
    }
}
