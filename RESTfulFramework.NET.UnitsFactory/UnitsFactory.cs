
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units;


namespace RESTfulFramework.NET.Factory
{

    /// <summary>
    /// 组件工厂
    /// </summary>
    public class UnitsFactory<TUserInfoModel>
        where TUserInfoModel : BaseUserInfo, new()
    {
        public virtual ITokenApi<RequestModel<TUserInfoModel>, ResponseModel, TUserInfoModel> GetTokenApi(string api) => (new PluginPackage.PluginContainer()).GetPluginInstance<ITokenApi<RequestModel<TUserInfoModel>, ResponseModel, TUserInfoModel>>(api);
        public virtual IInfoApi<RequestModel<TUserInfoModel>, ResponseModel, TUserInfoModel> GetInfoApi(string api) => (new PluginPackage.PluginContainer()).GetPluginInstance<IInfoApi<RequestModel<TUserInfoModel>, ResponseModel, TUserInfoModel>>(api);
        public virtual IStreamApi<RequestModel<TUserInfoModel>, TUserInfoModel> GetStreamApi(string api) => (new PluginPackage.PluginContainer()).GetPluginInstance<IStreamApi<RequestModel<TUserInfoModel>, TUserInfoModel>>(api);
    }
}
