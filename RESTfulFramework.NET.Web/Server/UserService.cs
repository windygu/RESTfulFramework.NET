using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using RESTfulFramework.NET.UserService;
using RESTfulFramework.NET.Units;
using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Web.Server
{
    /// <summary>
    /// 正式的用户接口基础服务
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UserService : BaseUserService<ConfigManager, ConfigModel, LocalUserCache, UserInfo, JsonSerialzer, DBHelper, SmsManager, LogManager>
    {
    }
}
