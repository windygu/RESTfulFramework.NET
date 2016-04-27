
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units;
using RESTfulFramework.NET.Units.Model;

namespace RESTfulFramework.NET.Factory
{

    /// <summary>
    /// 组件工厂
    /// </summary>
    public class UnitsFactory<TRequestModel, TResponseModel>
        where TRequestModel : RequestModel
        where TResponseModel : ResponseModel
    {
        public IDBHelper GetDBHelper() => new DBHelper();
        public IJsonSerialzer GetJsonSerialzer() => new JsonSerialzer();
        public IConfigManager<SysConfigModel> GetConfigManager() => new ConfigManager();
        public IUserCache<UserInfo> GetUserCache() => new LocalUserCache();
        public ISmsManager GetSmsMamager() => new SmsManager();
        public ILogManager GetLogManager() => new LogManager();
        public ITokenApi<TRequestModel, TResponseModel> GetTokenApi(string api) => (new PluginPackage.PluginContainer()).GetPluginInstance<ITokenApi<TRequestModel, TResponseModel>>(api);
        public IInfoApi<TRequestModel, TResponseModel> GetInfoApi(string api) => (new PluginPackage.PluginContainer()).GetPluginInstance<IInfoApi<TRequestModel, TResponseModel>>(api); 
        public IStreamApi<TRequestModel> GetStreamApi(string api) => (new PluginPackage.PluginContainer()).GetPluginInstance<IStreamApi<TRequestModel>>(api);
    }
}
