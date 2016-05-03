
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units;
using RESTfulFramework.NET.Units.Model;

namespace RESTfulFramework.NET.Factory
{

    /// <summary>
    /// 组件工厂
    /// </summary>
    public class UnitsFactory
    {
        public IDBHelper GetDBHelper() => new DBHelper();
        public IJsonSerialzer GetJsonSerialzer() => new JsonSerialzer();
        public IConfigManager<SysConfigModel> GetConfigManager() => new ConfigManager();
        public IUserCache<UserInfo> GetUserCache() => new LocalUserCache();
        public ISmsManager GetSmsMamager() => new SmsManager();
        public ILogManager GetLogManager() => new LogManager();
        public ITokenApi<RequestModel, ResponseModel> GetTokenApi(string api) => (new PluginPackage.PluginContainer()).GetPluginInstance<ITokenApi<RequestModel, ResponseModel>>(api);
        public IInfoApi<RequestModel, ResponseModel> GetInfoApi(string api) => (new PluginPackage.PluginContainer()).GetPluginInstance<IInfoApi<RequestModel, ResponseModel>>(api); 
        public IStreamApi<RequestModel> GetStreamApi(string api) => (new PluginPackage.PluginContainer()).GetPluginInstance<IStreamApi<RequestModel>>(api);
    }
}
