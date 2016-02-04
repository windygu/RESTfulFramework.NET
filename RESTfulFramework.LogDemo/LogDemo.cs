using System;
using System.ComponentModel.Composition;
using System.IO;

namespace RESTfulFramework.LogDemo
{
    [Export(typeof(ILogPlugin.ILog))]
    public class LogDemo : ILogPlugin.ILog
    {
        private delegate void WriteString();
        private static object LockObj { get; set; } = new object();
        public void WriteLog(object logInfo)
        {
            /*您可以定义您的高性能日志记录器，此例子仅做参考*/
            var str = Json2KeyValue.JsonConvert.SerializeObject(logInfo);

            var ay = new WriteString(() =>
            {
                lock (LockObj)
                {
                    using (var sw = new StreamWriter(
                 Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"log{DateTime.Now.ToString("yyyyMMdd")}.txt"), true))
                    {
                        sw.WriteLine(str);

                    }
                    Console.WriteLine(str);
                }

            });
            ay.BeginInvoke(new AsyncCallback((o) => { }), ay);

        }
    }
}
