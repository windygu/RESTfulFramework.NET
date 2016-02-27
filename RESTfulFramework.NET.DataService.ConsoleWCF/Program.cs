using RESTfulFramework.NET.ComponentModel;
using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace RESTfulFramework.NET.DataService.ConsoleWCF
{
    class Program
    {
        static void Main(string[] args)
        {
            var wHost = new System.ServiceModel.Web.WebServiceHost(typeof(DataService), new Uri(ConfigurationManager.AppSettings["baseUrl"]));
            var httpBinding = new WebHttpBinding();
            wHost.AddServiceEndpoint(typeof(IService), httpBinding, ConfigurationManager.AppSettings["relativeUrl"]);

            if (wHost.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
            {
                //行为 
                var behavior = new ServiceMetadataBehavior { HttpGetEnabled = true };
                //元数据地址 
                wHost.Description.Behaviors.Add(behavior);
            }

            //启动 
            if (wHost.State != CommunicationState.Opened)
            {
                wHost.Open();
                Console.WriteLine("已启动控制台RESTful服务。");
            }
            else {
                Console.WriteLine("启动控制台RESTful服务失败。");
            }
            System.Console.ReadKey();
            wHost.Close();
        }
    }
}
