using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Business
{
    public class selectinfo : IInfoApi<RequestModel, ResponseModel>
    {
        public ResponseModel RunApi(RequestModel source)
        {
            return new ResponseModel { Code = 1, Msg = "测试" };
        }
    }
}
