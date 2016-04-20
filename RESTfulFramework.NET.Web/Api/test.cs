using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.Web.Api
{
    //http://url/DataService.svc/getinfo?body={json}&api=test

    public class test : IInfoApi<RequestModel, ResponseModel>
    {
        public ResponseModel RunApi(RequestModel source)
        {
            throw new NotImplementedException();
        }
    }
}
