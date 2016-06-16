using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.ComponentModel
{
    public class ApiContext<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager>
          where TConfigManager : IConfigManager<TConfigModel>, new()
         where TConfigModel : IConfigModel, new()
         where TUserCache : IUserCache<TUserInfoModel>, new()
         where TUserInfoModel : IBaseUserInfo, new()
         where TJsonSerialzer : IJsonSerialzer, new()
         where TDBHelper : IDBHelper, new()
         where TSmsManager : ISmsManager, new()
         where TLogManager : ILogManager, new()
    {

        /// <summary>
        /// 数据库操作
        /// </summary>
        public TDBHelper DbHelper { get; set; }

        /// <summary>
        /// 用于短信的收发
        /// </summary>
        public TSmsManager SmsManager { get; set; }

        /// <summary>
        /// 用户缓存应该是被多个实例共享的
        /// </summary>
        public TUserCache UserCache { get; set; }

        /// <summary>
        /// 用于配置管理
        /// </summary>
        public TConfigManager ConfigManager { get; set; }


        /// <summary>
        /// 序列化器组件
        /// </summary>
        public TJsonSerialzer JsonSerialzer { get; set; }

        public TLogManager LogManager { get; set; }

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public TUserInfoModel CurrUserInfo { get; set; }

        /// <summary>
        /// 当前Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 基础的配置
        /// </summary>
        public ConfigInfo ConfigInfo { get; set; }

        public Dictionary<string, string> RequestHeader { get; set; }
    }
}
