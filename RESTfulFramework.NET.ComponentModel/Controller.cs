using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.ComponentModel
{
    public class Controller<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager>
          where TConfigManager : IConfigManager<TConfigModel>, new()
         where TConfigModel : IConfigModel, new()
         where TUserCache : IUserCache<TUserInfoModel>, new()
         where TUserInfoModel : IBaseUserInfo, new()
         where TJsonSerialzer : IJsonSerialzer, new()
         where TDBHelper : IDBHelper, new()
         where TSmsManager : ISmsManager, new()
         where TLogManager : ILogManager, new()
    {
        public ApiContext<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager> Context { get; set; }
    }
}
