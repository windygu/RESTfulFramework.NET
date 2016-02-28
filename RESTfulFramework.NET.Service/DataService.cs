﻿using RESTfulFramework.NET.ComponentModel;
using ServiceStack.Redis;
using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace RESTfulFramework.NET.Service
{
    public class DataServer
    {
        private WebServiceHost WHost { get; set; }
        private string DataServiceBaseUrl { get; set; }
        private string DataServiceRelativeUrl { get; set; }
        public DataServer()
        {
            try
            {
                DataServiceBaseUrl = ConfigurationManager.AppSettings["DataServiceBaseUrl"];
                DataServiceRelativeUrl = ConfigurationManager.AppSettings["DataServiceRelativeUrl"];

                if (string.IsNullOrEmpty(DataServiceBaseUrl))
                    ConsoleWriteWarinInfo($"配置文件DataServiceBaseUrl未设置，例如：<add key=\"DataServiceBaseUrl\" value=\"http://localhost:8736\"/>");
                else
                    Console.WriteLine($"DataService基地址：{DataServiceBaseUrl}");


                if (string.IsNullOrEmpty(DataServiceRelativeUrl))
                    ConsoleWriteWarinInfo($"配置文件DataServiceRelativeUrl未设置，例如：<add key=\"DataServiceRelativeUrl\" value=\"/DataService\" />");
                else
                    Console.WriteLine($"DataService相对地址：{DataServiceRelativeUrl}");

                WHost = new WebServiceHost(typeof(DataService.DataService), new Uri(DataServiceBaseUrl));
            }
            catch (Exception ex)
            {
                ConsoleWriteWarinInfo($"启动DataService失败,错误信息：{ex.Message}");
            }

        }

        public void Start()
        {
            CheckCondition();

            try
            {
                var httpBinding = new WebHttpBinding();
                WHost.AddServiceEndpoint(typeof(RESTfulFramework.NET.ComponentModel.IService), httpBinding, DataServiceRelativeUrl);

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
                    Console.WriteLine("已启动 DataService RESTful服务。");
                    Console.ForegroundColor = ConsoleColor.White;

                }
                else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("启动 DataService RESTful服务失败。");
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
                        ConsoleWriteWarinInfo($"配置文件连接字符串未设置，例如： <add name=\"RESTfulFrameworkConnection\" connectionString=\"server=127.0.0.1;port=3308;Data id=root;password=123456abc;persistsecurityinfo=True;database=restfulframework\"/>");
                    else
                        Console.WriteLine($"数据库连接字符串：{ConfigurationManager.ConnectionStrings["RESTfulFrameworkConnection"].ToString()}");
                }
                catch (Exception)
                {
                    ConsoleWriteWarinInfo($"配置文件连接字符串未设置，例如： <add name=\"RESTfulFrameworkConnection\" connectionString=\"server=127.0.0.1;port=3308;Data id=root;password=123456abc;persistsecurityinfo=True;database=restfulframework\"/>");
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
            Console.WriteLine("DataService接口如下：");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"通用接口(POST)：    {DataServiceBaseUrl}{DataServiceRelativeUrl}/post?token={{token}}&api={{api}}&timestamp={{timestamp}}&sign={{sign}}");
            Console.WriteLine($"通用接口(GET)：    {DataServiceBaseUrl}{DataServiceRelativeUrl}/get?body={{body}}&token={{token}}&api={{api}}&timestamp={{timestamp}}&sign={{sign}}");
            Console.WriteLine();
        }

    }
}
