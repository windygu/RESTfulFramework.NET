using RESTfulFramework.NET.ComponentModel;
using System;

namespace RESTfulConsoleService.Api
{
    public class Test : ITokenApi<RequestModel, ResponseModel>
    {
        public ResponseModel RunApi(RequestModel source)
        {
            return new ResponseModel { Code = Code.Sucess, Msg = source.Tag.ToString() };
        }
    }
}
