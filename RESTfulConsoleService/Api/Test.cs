using RESTfulFramework.NET.ComponentModel;

namespace RESTfulConsoleService.Api
{
    /*
    通过这样的地址访问，{api}=类名
    http://www.xxx.com/post?token={token}&api={api}&timestamp={timestamp}&sign={sign}    
     */
    public class Test : ITokenApi<RequestModel, ResponseModel>
    {
        public ResponseModel RunApi(RequestModel source)
        {
            return new ResponseModel { Code = Code.Sucess, Msg = source.Tag.ToString() };
        }
    }
}
