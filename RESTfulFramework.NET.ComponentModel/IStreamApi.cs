using System.IO;

namespace RESTfulFramework.NET.ComponentModel
{
    public interface IStreamApi<TRequestModel, TService>
        where TService : IService
    {
        Stream RunApi(TRequestModel source, TService service);
    }
}
