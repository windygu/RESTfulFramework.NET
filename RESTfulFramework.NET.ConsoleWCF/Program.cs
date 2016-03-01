using System;
namespace RESTfulFramework.NET.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "RESTfulFramework.NET 控制台服务";

            var userService = new UserServer();
            userService.Start();


            Console.WriteLine();
            Console.WriteLine("================================================");
            Console.WriteLine();


            var dataService = new DataServer();
            dataService.Start();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("  官网：  www.RESTfulFramework.net");

            Console.ReadKey();

            userService.Close();
            dataService.Close();
        }
    }
}
