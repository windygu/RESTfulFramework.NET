//using RESTfulFramework.NET.Business;
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units;
using System.ServiceModel.Activation;
using System;
using RESTfulFramework.NET.Web.Controller;
using RESTfulFramework.NET.DataService;

namespace RESTfulFramework.NET.Web.Server
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataService : BaseDataService<ConfigManager, ConfigModel, LocalUserCache, UserInfo, JsonSerialzer, DBHelper, SmsManager, LogManager, MyBusiness>
    {
    }

     
}
