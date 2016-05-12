using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.ComponentModel
{
    public class ApiContext<TUserInfo>
        where TUserInfo : BaseUserInfo, new()
    {

        /// <summary>
        /// 数据库操作
        /// </summary>
        public IDBHelper DbHelper { get; set; }

        /// <summary>
        /// 用于短信的收发
        /// </summary>
        public ISmsManager SmsManager { get; set; }



        /// <summary>
        /// 用户缓存应该是被多个实例共享的
        /// </summary>
        public IUserCache<TUserInfo> UserCache { get; set; }

        /// <summary>
        /// 用于配置管理
        /// </summary>
        public IConfigManager<SysConfigModel> ConfigManager { get; set; }


        /// <summary>
        /// 序列化器组件
        /// </summary>
        public IJsonSerialzer JsonSerialzer { get; set; }

        public ILogManager LogManager { get; set; }
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public TUserInfo CurrUserInfo { get; set; }

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
