using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Units
{
    public class LogManager : ILogManager
    {
        public void WriteLog(string msg) => NLog.LogManager.GetCurrentClassLogger().Info(msg);
    }
}
