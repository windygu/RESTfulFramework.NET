using System;
using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Business
{
    public class selectinfo : IInfoApi<RequestModel<UserInfo>, ResponseModel, IServiceContext<UserInfo>, UserInfo>
    {
        public ResponseModel RunApi(RequestModel<UserInfo> source, IServiceContext<UserInfo> service)
        {
            throw new NotImplementedException();
        }
    }
}
