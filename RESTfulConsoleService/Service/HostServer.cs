using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.UserService;
using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace RESTfulConsoleService.Service
{
    /*控制台宿主服务，无需更改。*/
    public class HostDataServer
    {
        private WebServiceHost WHost { get; set; }
        private string DataServiceBaseUrl { get; set; }
    
        public HostDataServer()
        {
            DataServiceBaseUrl = ConfigInfo.DataServiceBaseUrl;
      
            if (string.IsNullOrEmpty(DataServiceBaseUrl)) throw new Exception($"配置文件DataServiceBaseUrl未设置，例如：<add key=\"DataServiceBaseUrl\" value=\"http://localhost:8736/DataService\"/>"); 
            WHost = new WebServiceHost(typeof(RESTfulDataService), new Uri(DataServiceBaseUrl));
        }

        public bool Start()
        {
            //检测配置文件数据库配置
            if (string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["RESTfulFrameworkConnection"].ToString())) throw new Exception($"配置文件连接字符串未设置（支持MySql、Sqlite），例如： <add name=\"RESTfulFrameworkConnection\" connectionString=\"server=127.0.0.1;port=3308;Data id=root;password=123456abc;persistsecurityinfo=True;database=restfulframework\"/>");
            var httpBinding = new WebHttpBinding();
            WHost.AddServiceEndpoint(typeof(IService), httpBinding, DataServiceBaseUrl);
            if (WHost.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
            {
                var behavior = new ServiceMetadataBehavior { HttpGetEnabled = true };
                WHost.Description.Behaviors.Add(behavior);
            }
            //启动 
            if (WHost.State != CommunicationState.Opened)
            {
                WHost.Open();
                return true;
            }
            return false;
        }
        public void Close() => WHost.Close();
    }

    public class HostUserServer
    {
        private WebServiceHost WHost { get; set; }
        private string UserServiceBaseUrl { get; set; }
  
        public HostUserServer()
        {
            UserServiceBaseUrl = ConfigInfo.UserServiceBaseUrl;
            if (string.IsNullOrEmpty(UserServiceBaseUrl)) throw new Exception($"配置文件UserServiceBaseUrl未设置，例如：<add key=\"UserServiceBaseUrl\" value=\"http://localhost:8737/UserService\"/>");      
            WHost = new WebServiceHost(typeof(UserService), new Uri(UserServiceBaseUrl));
        }

        public bool Start()
        {
            //检测配置文件数据库配置
            if (string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["RESTfulFrameworkConnection"].ToString())) throw new Exception($"配置文件连接字符串未设置（支持MySql、Sqlite），例如： <add name=\"RESTfulFrameworkConnection\" connectionString=\"server=127.0.0.1;port=3308;user id=root;password=123456abc;persistsecurityinfo=True;database=restfulframework\"/>");

            var httpBinding = new WebHttpBinding();
            WHost.AddServiceEndpoint(typeof(IUserService), httpBinding, UserServiceBaseUrl);

            if (WHost.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
            {
                var behavior = new ServiceMetadataBehavior { HttpGetEnabled = true };
                WHost.Description.Behaviors.Add(behavior);
            }

            if (WHost.State != CommunicationState.Opened)
            {
                WHost.Open();
                return true;
            }
            return false;
        }

        public void Close() => WHost.Close();

    }
}
