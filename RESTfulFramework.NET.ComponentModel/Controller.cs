using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.ComponentModel
{
    /// <summary>
    /// 控制器，http请求均会被继承的控制器接收
    /// </summary>
    /// <typeparam name="TConfigManager">配置管理器，用于设置和读取配置。即所有与配置相关的都使用该TConfigManager管理、读取、设置。</typeparam>
    /// <typeparam name="TConfigModel">配置模型，配置管理器依赖于此模型。该模型定义了配置信息的字段，例如：时间、地址、人物。</typeparam>
    /// <typeparam name="TUserCache">缓存管理，用户信息、token及自定义信息都可以缓存下来。该泛型定义了缓存的方式，或内存或数据库或redis</typeparam>
    /// <typeparam name="TUserInfoModel">用户信息模型，如用户名、用户性别</typeparam>
    /// <typeparam name="TJsonSerialzer">序列化器，可以使用性能更佳的序列化器</typeparam>
    /// <typeparam name="TDBHelper">数据库操作</typeparam>
    /// <typeparam name="TSmsManager">短信管理</typeparam>
    /// <typeparam name="TLogManager">日志</typeparam>
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
        /// <summary>
        /// 数据上下文
        /// </summary>
        public ApiContext<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager> Context { get; set; }
    }
}
