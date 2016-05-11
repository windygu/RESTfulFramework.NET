using RESTfulFramework.NET.ComponentModel;
using System;
using System.IO;
using System.ServiceModel.Web;
using System.Text;
using System.Collections.Generic;
using System.ServiceModel.Activation;

namespace RESTfulFramework.NET.DataService
{
    /// <summary>
    /// 基础数据服务
    /// </summary>
    /// <typeparam name="TConfigManager">配置管理</typeparam>
    /// <typeparam name="TUserCache">用户缓存</typeparam>
    /// <typeparam name="TUserInfoModel">用户信息模型</typeparam>
    /// <typeparam name="TJsonSerialzer">序列化与反序列化器</typeparam>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BaseDataService<TConfigManager, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager>
        : Service<TConfigManager, TUserCache, TUserInfoModel, TJsonSerialzer, TDBHelper, TSmsManager, TLogManager>
         where TConfigManager : IConfigManager<SysConfigModel>, new()
         where TUserCache : IUserCache<TUserInfoModel>, new()
         where TUserInfoModel : BaseUserInfo, new()
         where TJsonSerialzer : IJsonSerialzer, new()
         where TDBHelper : IDBHelper, new()
         where TSmsManager : ISmsManager, new()
         where TLogManager : ILogManager, new()
    {

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
        /// 安全检查
        /// </summary>
        public override bool SecurityCheck(RequestModel<BaseUserInfo> requestModel) => SecurityFactoryContext.GetSecurityService().SecurityCheck(requestModel);


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
            try
            {
                WriteLog($"输出结果：{resultStr}", "输出");
            }
            catch { }

            return new MemoryStream(Encoding.UTF8.GetBytes(resultStr));
        }

        protected override ResponseModel ApiHandler(RequestModel<TUserInfoModel> requestModel)
        {
            var responseModel = base.ApiHandler(requestModel);
            try
            {
                WriteLog($"接收请求：body={requestModel?.BodyString}&token={requestModel?.Token}&api={requestModel?.Api}&timestamp={requestModel?.Timestamp}&sign={requestModel?.Sign}", "请求");
            }
            catch { }

            return responseModel;
        }

        protected virtual void WriteLog(string logInfo, string title) => LogManager?.WriteLog(logInfo);

    }
}
