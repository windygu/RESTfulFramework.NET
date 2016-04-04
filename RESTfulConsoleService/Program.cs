using RESTfulConsoleService.Service;
using System;
using System.Diagnostics;

namespace RESTfulConsoleService
{
    public class Program
    {
        
        static void Main(string[] args)
        {
            Console.Title = "RESTfulFramework.NET 控制台服务";
            try
            {
                //全局异常捕获
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
                var userService = new HostUserServer();
                if (userService.Start()) Console.WriteLine("已启动 UserService RESTful服务。");
                else Console.WriteLine("启动 UserService RESTful服务失败。");

                var dataService = new HostDataServer();
                if (dataService.Start()) Console.WriteLine("已启动 DataService RESTful服务。");
                else Console.WriteLine("启动 DataService RESTful服务失败。");

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("UserService接口如下：");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"用户登陆(GET)：    {ConfigInfo.UserServiceBaseUrl}/login2?username={{username}}&sign={{sign}}&timestamp={{timestamp}}&clientid={{clientid}}");
                Console.WriteLine($"用户登陆(GET)：    {ConfigInfo.UserServiceBaseUrl}/login?username={{username}}&sign={{sign}}&timestamp={{timestamp}}");
                Console.WriteLine($"用户注销(GET)：    {ConfigInfo.UserServiceBaseUrl}/loginout?token={{token}}");
                Console.WriteLine($"用户注册(GET)：    {ConfigInfo.UserServiceBaseUrl}/register?username={{username}}&password={{password}}&smscode={{smscode}}&realname={{realname}}");
                Console.WriteLine($"获取用户信息(GET)：{ConfigInfo.UserServiceBaseUrl}/getuserinfo?token={{token}}");
                Console.WriteLine($"请求短信验证码(GET)：{ConfigInfo.UserServiceBaseUrl}/sendsmscode?phone={{phone}}");
                Console.WriteLine($"短信验证码是否存在(GET)：{ConfigInfo.UserServiceBaseUrl}/smscodeexist?code={{code}}");
                Console.WriteLine();




                Console.WriteLine("如需关闭服务，请输入exit按回车键退出！");

                while (Console.ReadLine() != "exit") { }
                userService.Close();
                dataService.Close();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"调用失败,异常信息：{ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("请排除错误再重新启动服务！\r\n请输入exit按回车键退出！");
                while (Console.ReadLine() != "exit") { }
            }
            finally
            {

            }
        }

        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            var ex = e.Exception;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"错误：{ex.Message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"错误：{ex.Message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
