using System;
using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Business
{
    public class select : BaseTokenApi<RequestModel<UserInfo>, ResponseModel, UserInfo>
    {
        public override ResponseModel RunApi(RequestModel<UserInfo> source)
        {
            return null;
        }
    }
}
