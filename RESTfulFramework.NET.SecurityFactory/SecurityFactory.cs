using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Factory
{
    /// <summary>
    /// 安全相关工厂
    /// </summary>
    public class SecurityFactory<TConfigManager, TUserCache, TUserInfoModel>
         where TConfigManager : IConfigManager<SysConfigModel>, new()
         where TUserCache : IUserCache<TUserInfoModel>, new()
         where TUserInfoModel : BaseUserInfo, new()
    {
        public ISecurity<RequestModel<BaseUserInfo>> GetSecurityService() => new Security.SecurityService<TConfigManager, TUserCache, TUserInfoModel>();
    }
}
