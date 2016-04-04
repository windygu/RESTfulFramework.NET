using System.Configuration;

namespace RESTfulConsoleService
{
    public struct ConfigInfo
    {
        public static string ApiNamespace { get; } = "RESTfulConsoleService.Api";
        public static string DataServiceBaseUrl { get; }= ConfigurationManager.AppSettings["DataServiceBaseUrl"];
        public static string UserServiceBaseUrl { get; }=ConfigurationManager.AppSettings["UserServiceBaseUrl"];
    }
}
