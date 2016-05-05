using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Business
{
    public class select<TServer> : ITokenApi<RequestModel<BaseUserInfo>, ResponseModel,TServer>
        where TServer:IService
    {
        public ResponseModel RunApi(RequestModel<BaseUserInfo> source,TServer service)
        {
            return null;

        }
    }
}
