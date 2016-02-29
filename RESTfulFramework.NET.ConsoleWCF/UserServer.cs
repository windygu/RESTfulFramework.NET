using RESTfulFramework.NET.ComponentModel;
using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using ServiceStack.Redis;

namespace RESTfulFramework.NET.Service
{
    public class UserServer
    {
        private WebServiceHost WHost { get; set; }
        private string UserServiceBaseUrl { get; set; }
        private string UserServiceRelativeUrl { get; set; }
        public UserServer()
        {
            try
            {
                UserServiceBaseUrl = ConfigurationManager.AppSettings["UserServiceBaseUrl"];
                UserServiceRelativeUrl = ConfigurationManager.AppSettings["UserServiceRelativeUrl"];

                if (string.IsNullOrEmpty(UserServiceBaseUrl))
                    ConsoleWriteWarinInfo($"配置文件UserServiceBaseUrl未设置，例如：<add key=\"UserServiceBaseUrl\" value=\"http://localhost:8737\"/>");
                else
                    Console.WriteLine($"UserService基地址：{UserServiceBaseUrl}");


                if (string.IsNullOrEmpty(UserServiceRelativeUrl))
                    ConsoleWriteWarinInfo($"配置文件UserServiceRelativeUrl未设置，例如：<add key=\"UserServiceRelativeUrl\" value=\"/UserService\" />");
                else
                    Console.WriteLine($"UserService相对地址：{UserServiceRelativeUrl}");

                WHost = new WebServiceHost(typeof(UserService.UserService), new Uri(UserServiceBaseUrl));
            }
            catch (Exception ex)
            {
                ConsoleWriteWarinInfo($"启动UserService失败,错误信息：{ex.Message}");
            }

        }

        public void Start()
        {
            CheckCondition();

            try
            {
                var httpBinding = new WebHttpBinding();
                WHost.AddServiceEndpoint(typeof(IUserService), httpBinding, UserServiceRelativeUrl);
            
                if (WHost.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                {
                    //行为 
                    var behavior = new ServiceMetadataBehavior { HttpGetEnabled = true };
                    //元数据地址 
                    WHost.Description.Behaviors.Add(behavior);
                }
                //启动 
                if (WHost.State != CommunicationState.Opened)
                {
                    WHost.Open();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("已启动 UserService RESTful服务。");
                    Console.ForegroundColor = ConsoleColor.White;
                   
                }
                else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("启动 UserService RESTful服务失败。");
                    Console.ForegroundColor = ConsoleColor.White;
               
                }
            }
            catch (Exception ex)
            {
                ConsoleWriteWarinInfo(ex.Message);
            }
        }


        public void Close()
        {
            WHost.Close();
        }

        private static void ConsoleWriteWarinInfo(string str)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(str);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void CheckCondition()
        {
            try
            {
                try
                {
                    //检测配置文件数据库配置
                    if (string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["RESTfulFrameworkConnection"].ToString()))
                        ConsoleWriteWarinInfo($"配置文件连接字符串未设置，例如： <add name=\"RESTfulFrameworkConnection\" connectionString=\"server=127.0.0.1;port=3308;user id=root;password=123456abc;persistsecurityinfo=True;database=restfulframework\"/>");
                    else
                        Console.WriteLine($"数据库连接字符串：{ConfigurationManager.ConnectionStrings["RESTfulFrameworkConnection"].ToString()}");
                }
                catch (Exception)
                {
                    ConsoleWriteWarinInfo($"配置文件连接字符串未设置，例如： <add name=\"RESTfulFrameworkConnection\" connectionString=\"server=127.0.0.1;port=3308;user id=root;password=123456abc;persistsecurityinfo=True;database=restfulframework\"/>");
                }

                //检测redis配置
                var configManager = PluginPackage.Factory.GetInstance<IConfigManager<SysConfigModel>>();
                string redisAddress = null;
                string redisPort = null;

                try
                {
                    redisAddress = configManager.GetValue("redis_address").value;
                    Console.WriteLine($"Redis地址：{redisAddress}");
                    redisPort = configManager.GetValue("redis_port").value;
                    Console.WriteLine($"Redis端口：{redisPort}");
                }
                catch (Exception ex)
                {
                    ConsoleWriteWarinInfo($"无法获取redis地址及端口,错误信息:{ex.Message}");
                }

                try
                {
                    var redisClient = new RedisClient(redisAddress, int.Parse(redisPort));
                    redisClient.SetValue("test", "test");
                    Console.WriteLine($"Redis服务正常。");
                }
                catch (Exception ex)
                {
                    ConsoleWriteWarinInfo($"Redis未启动或Redis地址、端口错误,错误信息:{ex.Message}");
                }
                TipInfo();
            }
            catch (Exception ex)
            {
                ConsoleWriteWarinInfo(ex.Message);
            }

        }
        public void TipInfo()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("UserService接口如下：");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"用户登陆(GET)：    {UserServiceBaseUrl}{UserServiceRelativeUrl}/login2?username={{username}}&sign={{sign}}&timestamp={{timestamp}}&clientid={{clientid}}");
            Console.WriteLine($"用户登陆(GET)：    {UserServiceBaseUrl}{UserServiceRelativeUrl}/login?username={{username}}&sign={{sign}}&timestamp={{timestamp}}");
            Console.WriteLine($"用户注销(GET)：    {UserServiceBaseUrl}{UserServiceRelativeUrl}/loginout?token={{token}}");
            Console.WriteLine($"用户注册(GET)：    {UserServiceBaseUrl}{UserServiceRelativeUrl}/register?username={{username}}&password={{password}}&smscode={{smscode}}&realname={{realname}}");
            Console.WriteLine($"获取用户信息(GET)：{UserServiceBaseUrl}{UserServiceRelativeUrl}/getuserinfo?token={{token}}");
            Console.WriteLine($"请求短信验证码(GET)：{UserServiceBaseUrl}{UserServiceRelativeUrl}/sendsmscode?phone={{phone}}");
            Console.WriteLine($"短信验证码是否存在(GET)：{UserServiceBaseUrl}{UserServiceRelativeUrl}/smscodeexist?code={{code}}");
            Console.WriteLine();
        }

    }
}
