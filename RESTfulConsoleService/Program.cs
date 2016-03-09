using RESTfulConsoleService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulConsoleService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "RESTfulFramework.NET 控制台服务";

            var userService = new HostUserServer();
            userService.Start();

            Console.WriteLine();
            Console.WriteLine("================================================");
            Console.WriteLine();


            var dataService = new HostDataServer();
            dataService.Start();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("如需关闭服务，请输入exit按回车键退出！");

            while (Console.ReadLine() != "exit") { }


            userService.Close();
            dataService.Close();
        }
    }
}
