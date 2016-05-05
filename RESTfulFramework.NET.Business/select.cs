using System;
using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Business
{
    public class select : ITokenApi<RequestModel<UserInfo>, ResponseModel, IServiceContext<UserInfo>,UserInfo>
    {
        public ResponseModel RunApi(RequestModel<UserInfo> source, IServiceContext<UserInfo> service)
        {
            
            throw new NotImplementedException();
        }

        
    }
}
