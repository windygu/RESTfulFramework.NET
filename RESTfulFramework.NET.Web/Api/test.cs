using RESTfulFramework.NET.ComponentModel;
using System;

namespace RESTfulFramework.NET.Web.Api
{
    //http://url/DataService.svc/getinfo?body={json}&api=test

    public class test : BaseInfoApi<RequestModel<BaseUserInfo>, ResponseModel, BaseUserInfo>
    {
        public override ResponseModel RunApi(RequestModel<BaseUserInfo> request)
        {
            throw new NotImplementedException();
        }
    }
}
