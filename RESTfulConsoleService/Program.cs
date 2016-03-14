using RESTfulConsoleService.Service;
using System;

namespace RESTfulConsoleService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "RESTfulFramework.NET 控制台服务";
            try
            {
                var userService = new HostUserServer();
                if (userService.Start()) Console.WriteLine("已启动 UserService RESTful服务。");
                else Console.WriteLine("启动 UserService RESTful服务失败。");

                var dataService = new HostDataServer();
                if (dataService.Start()) Console.WriteLine("已启动 DataService RESTful服务。");
                else Console.WriteLine("启动 DataService RESTful服务失败。");

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
            }
        }
    }
}
