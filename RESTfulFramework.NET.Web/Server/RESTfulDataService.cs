using System;
using RESTfulFramework.NET.ComponentModel;
using System.Reflection;
using RESTfulFramework.NET.Web.Config;

namespace RESTfulFramework.NET.Web.Server
{
    /*调用Api相关,无需更改或谨慎更改。*/
    public class RESTfulDataService : RESTfulFramework.NET.DataService.DataService
    {
        protected override ResponseModel ApiHandler(RequestModel requestModel)
        {
            var apiType = Assembly.GetExecutingAssembly().GetType($"{ConfigInfo.ApiNamespace}.{requestModel.Api}");
            var tokenApi = Activator.CreateInstance(apiType) as ITokenApi<RequestModel, ResponseModel>;
            return tokenApi.RunApi(requestModel);
        }

        protected override ResponseModel InfoApiHandler(RequestModel requestModel)
        {
            var apiType = Assembly.GetExecutingAssembly().GetType($"{ConfigInfo.ApiNamespace}.{requestModel.Api}");
            var infoApi = Activator.CreateInstance(apiType) as IInfoApi<RequestModel, ResponseModel>;
            return infoApi.RunApi(requestModel);
        }
    }
}
