using System.IO;

namespace RESTfulFramework.NET.ComponentModel
{
    public interface IStreamApi<TRequestModel, TServiceContext, TUserInfoModel>
     where TServiceContext : IServiceContext<TUserInfoModel>
     where TUserInfoModel : BaseUserInfo, new()
    {
        Stream RunApi(TRequestModel source, TServiceContext service);
    }
}
