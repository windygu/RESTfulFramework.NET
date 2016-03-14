using RESTfulFramework.NET.DataService;
using System;
using RESTfulFramework.NET.ComponentModel;
using System.Reflection;

namespace RESTfulConsoleService.Service
{
    /*调用Api相关,无需更改或谨慎更改。*/
    public class RESTfulDataService : DataService
    {
        protected override ResponseModel ApiHandler(RequestModel requestModel)
        {
            var ass = Assembly.GetExecutingAssembly();
            var apiType = ass.GetType($"{ConfigInfo.ApiNamespace}.{requestModel.Api}");
            var tokenApi = Activator.CreateInstance(apiType) as ITokenApi<RequestModel, ResponseModel>;
            return tokenApi.RunApi(requestModel);
        }
 
    }
}
