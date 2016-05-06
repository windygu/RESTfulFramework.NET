using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units;
using System.ServiceModel.Activation;

namespace RESTfulFramework.NET.DataService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DataService : BaseDataService<ConfigManager, LocalUserCache, BaseUserInfo, JsonSerialzer, DBHelper, SmsManager, LogManager>
    {
    }
}
