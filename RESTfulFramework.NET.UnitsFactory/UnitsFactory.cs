
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
        public virtual ITokenApi<RequestModel<TUserInfoModel>, ResponseModel, TServiceContext, TUserInfoModel> GetTokenApi<TServiceContext>(string api) where TServiceContext : IServiceContext<TUserInfoModel> => (new PluginPackage.PluginContainer()).GetPluginInstance<ITokenApi<RequestModel<TUserInfoModel>, ResponseModel, TServiceContext, TUserInfoModel>>(api);
        public virtual IInfoApi<RequestModel<TUserInfoModel>, ResponseModel, TServiceContext, TUserInfoModel> GetInfoApi<TServiceContext>(string api) where TServiceContext : IServiceContext<TUserInfoModel> => (new PluginPackage.PluginContainer()).GetPluginInstance<IInfoApi<RequestModel<TUserInfoModel>, ResponseModel, TServiceContext, TUserInfoModel>>(api);
        public virtual IStreamApi<RequestModel<TUserInfoModel>, TServiceContext, TUserInfoModel> GetStreamApi<TServiceContext>(string api) where TServiceContext : IServiceContext<TUserInfoModel> => (new PluginPackage.PluginContainer()).GetPluginInstance<IStreamApi<RequestModel<TUserInfoModel>, TServiceContext, TUserInfoModel>>(api);
    }
}
