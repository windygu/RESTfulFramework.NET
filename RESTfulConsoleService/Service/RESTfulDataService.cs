using RESTfulFramework.NET.DataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RESTfulFramework.NET.ComponentModel;
using System.Reflection;

namespace RESTfulConsoleService.Service
{
    public class RESTfulDataService : DataService
    {
        protected override ResponseModel ApiHandler(RequestModel requestModel)
        {
            var ass = Assembly.GetExecutingAssembly();
            var apiType = ass.GetType($"{ConfigInfo.ApiNamespace}.{requestModel.Api}");
            var tokenApi = Activator.CreateInstance(apiType) as ITokenApi<RequestModel, ResponseModel>;
            return tokenApi.RunApi(requestModel);
            //return base.ApiHandler(requestModel);
        }
    }
}
