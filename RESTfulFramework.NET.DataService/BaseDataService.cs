using RESTfulFramework.NET.ComponentModel;
using System;
using System.IO;
using System.ServiceModel.Web;
using System.Text;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using System.Linq;
using System.Dynamic;
using System.Linq.Expressions;


namespace RESTfulFramework.NET.DataService
{
    /// <summary>
    /// RESTful服务
    /// </summary>
    /// <typeparam name="TConfigManager">配置管理器，用于设置和读取配置。即所有与配置相关的都使用该TConfigManager管理、读取、设置。</typeparam>
    /// <typeparam name="TConfigModel">配置模型，配置管理器依赖于此模型。该模型定义了配置信息的字段，例如：时间、地址、人物。</typeparam>
    /// <typeparam name="TUserCache">缓存管理，用户信息、token及自定义信息都可以缓存下来。该泛型定义了缓存的方式，或内存或数据库或redis</typeparam>
    /// <typeparam name="TUserInfoModel">用户信息模型，如用户名、用户性别</typeparam>
    /// <typeparam name="TJsonSerialzer">序列化器，可以使用性能更佳的序列化器</typeparam>
    /// <typeparam name="TDBHelper">数据库操作</typeparam>
    /// <typeparam name="TSmsManager">短信管理</typeparam>
    /// <typeparam name="TLogManager">日志</typeparam>
    /// <typeparam name="TController">控制器，该泛型决定了http请求调用哪个控制器。</typeparam>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BaseDataService<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager, TController>
        : Service<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager, TController>
          where TConfigManager : IConfigManager<TConfigModel>, new()
         where TConfigModel : IConfigModel, new()
         where TUserCache : IUserCache<TUserInfoModel>, new()
         where TUserInfoModel : IBaseUserInfo, new()
         where TJsonSerialzer : IJsonSerialzer, new()
         where TDBHelper : IDBHelper, new()
         where TSmsManager : ISmsManager, new()
         where TLogManager : ILogManager, new()
         where TController : Controller<TConfigManager, TConfigModel, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager>, new()
    {
        #region ======  初始化  ======

        public BaseDataService()
        {
            #region 设置头部信息
            if (WebOperationContext.Current != null)
            {
                WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                WebOperationContext.Current.OutgoingResponse.ContentType = "application/json;charset=utf-8";
            }
            #endregion
        }

        #endregion

        #region ====== 对像转换 ======
        /// <summary>
        /// 对像序列化为字符串
        /// </summary>
        public override string ObjectToString(object obj)
        {
            if (Serialzer != null)
                return Serialzer.SerializeObject(obj);
            throw new Exception("没有可用的序列化器组件。");
        }

        /// <summary>
        /// 字符串反序列化为对像
        /// </summary>
        public override object StringToObject(string str)
        {
            if (Serialzer != null)
                return Serialzer.DeserializeObject<Dictionary<string, object>>(str);
            throw new Exception("没有可用的反序列化器组件。");
        }

        /// <summary>
        /// 接收的请求流转为对像
        /// </summary>
        public override string StreamToString(Stream stream) => new StreamReader(stream).ReadToEnd();

        /// <summary>
        /// 输出的对像转换成流
        /// </summary>

        public override Stream ResponseModelToStream(ResponseModel responseModel)
        {
            var resultStr = ObjectToString(responseModel);
            try { WriteLog($"输出结果：{resultStr}", "输出"); } catch { }
            return new MemoryStream(Encoding.UTF8.GetBytes(resultStr));
        }
        #endregion

        #region ====== 日志记录 ======
        [RecordLog]
        protected override ResponseModel ApiHandler(RequestModel<TUserInfoModel> requestModel)
        {
            try
            {
                WriteLog($"接收请求：body={requestModel?.BodyString}&token={requestModel?.Token}&api={requestModel?.Api}&timestamp={requestModel?.Timestamp}&sign={requestModel?.Sign}", "请求");
            }
            catch { }
            return base.ApiHandler(requestModel);
        }

        public virtual void WriteLog(string logInfo, string title) => LogManager?.WriteLog(logInfo);

        #endregion

        #region ====== 用于AOP ======

        #endregion
    }
}
