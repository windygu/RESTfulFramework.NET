using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Factory
{
    /// <summary>
    /// 安全相关工厂
    /// </summary>
    public class SecurityFactory
    {
        public ISecurity<RequestModel> GetSecurityService() => new Security.SecurityService();
    }
}
