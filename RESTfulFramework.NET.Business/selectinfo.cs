using System;
using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Business
{
    public class selectinfo : BaseInfoApi<RequestModel<UserInfo>, ResponseModel, UserInfo>
    {
        public override ResponseModel RunApi(RequestModel<UserInfo> request)
        {
            throw new NotImplementedException();
        }
    }
}
