
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units;


namespace RESTfulFramework.NET.Factory
{

    /// <summary>
    /// 组件工厂
    /// </summary>
    public class UnitsFactory<TUserInfoModel>
        where TUserInfoModel : BaseUserInfo
    {
        //public virtual IDBHelper GetDBHelper() => new DBHelper();
        //public virtual IJsonSerialzer GetJsonSerialzer() => new JsonSerialzer();
        //public virtual IConfigManager<SysConfigModel> GetConfigManager() => new ConfigManager();
        //public virtual IUserCache<BaseUserInfo> GetUserCache() => new LocalUserCache();
        //public virtual ISmsManager GetSmsMamager() => new SmsManager();
        //public virtual ILogManager GetLogManager() => new LogManager();
        public virtual ITokenApi<RequestModel<TUserInfoModel>, ResponseModel, TService> GetTokenApi<TService>(string api) where TService : IService
        {
            var itokenApi = (new PluginPackage.PluginContainer()).GetPluginInstance<ITokenApi<RequestModel<TUserInfoModel>, ResponseModel, TService>>(api);
            return itokenApi;
        }

        public virtual IInfoApi<RequestModel<TUserInfoModel>, ResponseModel, TService> GetInfoApi<TService>(string api) where TService : IService => (new PluginPackage.PluginContainer()).GetPluginInstance<IInfoApi<RequestModel<TUserInfoModel>, ResponseModel, TService>>(api);
        public virtual IStreamApi<RequestModel<TUserInfoModel>, TService> GetStreamApi<TService>(string api) where TService : IService => (new PluginPackage.PluginContainer()).GetPluginInstance<IStreamApi<RequestModel<TUserInfoModel>, TService>>(api);
    }
}
