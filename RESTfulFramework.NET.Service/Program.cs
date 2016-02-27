using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using RESTfulFramework.NET.DataService;
namespace RESTfulFramework.NET.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            //检测配置文件数据库配置
            //检测redis配置
            //
            var wHost = new System.ServiceModel.Web.WebServiceHost(typeof(RESTfulFramework.NET.DataService.DataService), new Uri(ConfigurationManager.AppSettings["baseUrl"]));
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
