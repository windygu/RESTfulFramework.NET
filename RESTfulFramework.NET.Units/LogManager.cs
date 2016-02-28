using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTfulFramework.NET.Units
{
    public class LogManager : ILogManager
    {
        private delegate void DltWrite(string msg);
        private DltWrite Dlt { get; set; }
        private object LockObject { get; set; } = new object();
        public LogManager()
        {
            Dlt = new DltWrite((s) =>
            {
                lock (LockObject)
                {
                    Console.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {s}");
                }
            });
        }
        public void WriteLog(string msg)
        {
            Dlt.BeginInvoke(msg, null, null);
        }

    }
}
