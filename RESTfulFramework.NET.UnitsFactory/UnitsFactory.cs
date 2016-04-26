
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units;
using RESTfulFramework.NET.Units.Model;

namespace RESTfulFramework.NET.Factory
{
    public class UnitsFactory<TRequestModel, TResponseModel>
        where TRequestModel : RequestModel
        where TResponseModel : ResponseModel
    {
        public IDBHelper DBHelper { get; set; }
        public IJsonSerialzer JsonSerialzer { get; set; }
        public IConfigManager<SysConfigModel> ConfigManager { get; set; }
        public IUserCache<UserInfo> UserCache { get; set; }
        public ISmsManager SmsMamager { get; set; }
        public ILogManager LogManager { get; set; }
        public ITokenApi<TRequestModel, TResponseModel> TokenApi { get; set; }
        public IInfoApi<TRequestModel, TResponseModel> InfoApi { get; set; }
        public IStreamApi<TRequestModel> StreamApi { get; set; }
        public UnitsFactory()
        {
            DBHelper = new DBHelper();
            JsonSerialzer = new JsonSerialzer();
            ConfigManager = new ConfigManager();
            UserCache = new UserCache();
            SmsMamager = new SmsManager();
            LogManager = new LogManager();
            TokenApi = PluginPackage.Factory.GetInstance<ITokenApi<TRequestModel, TResponseModel>>();
            InfoApi = PluginPackage.Factory.GetInstance<IInfoApi<TRequestModel, TResponseModel>>();
            StreamApi = PluginPackage.Factory.GetInstance<IStreamApi<TRequestModel>>();
        }
    }
}
