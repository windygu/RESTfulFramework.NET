using System;
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units;
using System.ServiceModel.Activation;
using RESTfulFramework.NET.Web.Controller;
using RESTfulFramework.NET.DataService;

namespace RESTfulFramework.NET.Web.Server
{
    /// <summary>
    /// 正式的数据接口服务，请在MyBusiness控制器处理Http请求
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataService : BaseDataService<ConfigManager, ConfigModel, TableUserCache<ConfigManager,ConfigModel>, UserInfo, JsonSerialzer, DBHelper, SmsManager, LogManager, MyBusiness>
    {
    }

     
}
