using System;
using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Units
{
    public class LogManager : ILogManager
    {
        public void Debug(string msg) => NLog.LogManager.GetCurrentClassLogger().Debug(msg);


        public void Error(string msg) => NLog.LogManager.GetCurrentClassLogger().Error(msg);


        public void Fatal(string msg) => NLog.LogManager.GetCurrentClassLogger().Fatal(msg);


        public void Info(string msg) => NLog.LogManager.GetCurrentClassLogger().Info(msg);


        public void Trace(string msg) => NLog.LogManager.GetCurrentClassLogger().Trace(msg);


        public void Warn(string msg) => NLog.LogManager.GetCurrentClassLogger().Warn(msg);


        public void WriteLog(string msg) => NLog.LogManager.GetCurrentClassLogger().Info(msg);


        private static LogManager logManager { get; set; }
        public static LogManager GetCurrLogManager()
        {
            if (logManager == null) logManager = new LogManager();
            return logManager;
        }


    }
}
