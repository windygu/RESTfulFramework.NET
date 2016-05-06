using System;
using RESTfulFramework.NET.ComponentModel;
using System.Reflection;

namespace RESTfulFramework.NET.Web.Server
{
    /*调用Api相关,无需更改或谨慎更改。*/
    public class RESTfulDataService : RESTfulFramework.NET.DataService.DataService
    {
        protected override ResponseModel ApiHandler(RequestModel<BaseUserInfo> requestModel)
        {
            var apiType = Assembly.GetExecutingAssembly().GetType($"{Config.ConfigInfo.ApiNamespace}.{requestModel.Api}");
            var tokenApi = Activator.CreateInstance(apiType) as ITokenApi<RequestModel<BaseUserInfo>, ResponseModel, BaseUserInfo>;
            return tokenApi.RunApi(requestModel);
        }

        protected override ResponseModel InfoApiHandler(RequestModel<BaseUserInfo> requestModel)
        {
            var apiType = Assembly.GetExecutingAssembly().GetType($"{Config.ConfigInfo.ApiNamespace}.{requestModel.Api}");
            var infoApi = Activator.CreateInstance(apiType) as IInfoApi<RequestModel<BaseUserInfo>, ResponseModel,BaseUserInfo>;
            return infoApi.RunApi(requestModel);
        }
    }
}
