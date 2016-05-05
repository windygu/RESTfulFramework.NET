using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Business
{
    public class selectinfo<TServer> : IInfoApi<RequestModel<BaseUserInfo>, ResponseModel, TServer>
        where TServer : IService
    {
        public ResponseModel RunApi(RequestModel<BaseUserInfo> source, TServer service)
        {
            return new ResponseModel { Code = 1, Msg = "测试" };
        }
    }
}
