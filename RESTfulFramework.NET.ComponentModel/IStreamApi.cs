using System.IO;

namespace RESTfulFramework.NET.ComponentModel
{
    public interface IStreamApi<TRequestModel>
    {
        Stream RunApi(TRequestModel source);
    }
}
