using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Factory
{
    public class SecurityFactory
    {

        public ISecurity<RequestModel> SecurityService { get; set; }

        public SecurityFactory()
        {
            SecurityService = new Security.SecurityService();
        }
    }
}
