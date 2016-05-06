using RESTfulFramework.NET.Units;
using RESTfulFramework.NET.ComponentModel;
using System.ServiceModel.Activation;

namespace RESTfulFramework.NET.UserService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class UserService : BaseUserService<ConfigManager, LocalUserCache, BaseUserInfo, JsonSerialzer, DBHelper, SmsManager, LogManager>
    {
    }
}
