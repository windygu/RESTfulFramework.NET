using PluginPackage;
using RESTfulFramework.NET.Common.Model;
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units.Model;

namespace RESTfulFramework.NET.Common
{
    public class UnitsFactory
    {

        public static IDBHelper DBHelper { get; set; }
        public static IJsonSerialzer JsonSerialzer { get; set; }
        public static IConfigManager<SysConfigModel> ConfigManager { get; set; }
        public static IUserCache<UserInfo> UserCache { get; set; }
        public static ISmsManager SmsMamager { get; set; }
        public static ILogManager LogManager { get; set; }
        public static IPushManager<PushInfo> PushManager { get; set; }

        static UnitsFactory()
        {
            DBHelper = Factory.GetInstance<IDBHelper>();
            JsonSerialzer = Factory.GetInstance<IJsonSerialzer>();
            ConfigManager = Factory.GetInstance<IConfigManager<SysConfigModel>>();
            UserCache = Factory.GetInstance<IUserCache<UserInfo>>();
            SmsMamager = Factory.GetInstance<ISmsManager>();
            PushManager = Factory.GetInstance<IPushManager<PushInfo>>();
            LogManager = Factory.GetInstance<ILogManager>();
        }
    }
}
