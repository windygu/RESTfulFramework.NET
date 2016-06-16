using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.Web.Controller
{
    //http://url/DataService.svc/getinfo?body={json}&api=test
    public class MyBusiness : Controller<ConfigManager, ConfigModel, LocalUserCache, UserInfo, JsonSerialzer, DBHelper, SmsManager, LogManager>
    {
        public ResponseModel select(RequestModel<UserInfo> requestModel)
        {
            return null;
        }
    }
}
