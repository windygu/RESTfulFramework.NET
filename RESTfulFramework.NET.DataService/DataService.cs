using RESTfulFramework.NET.ComponentModel;
using PluginPackage.Core;
using System;
using System.IO;
using System.ServiceModel.Web;
using System.Text;

namespace RESTfulFramework.NET.DataService
{
    public class DataService : Service<RequestModel, ResponseModel>
    {

        /// <summary>
        /// 组件容器
        /// </summary>
        private static IPluginContainer Container { get; set; }

        /// <summary>
        /// 序列化器组件
        /// </summary>
        private static IJsonSerialzer Serialzer { get; set; }

        public DataService()
        {
            if (Container == null)
            {
                Container = GetContainer();
                Serialzer = Container.GetPluginInstance<IJsonSerialzer>();
                LogManager = Container.GetPluginInstance<ILogManager>();
            }

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
                return Serialzer.DeserializeObject<object>(str);

            throw new Exception("没有可用的反序列化器组件。");
        }

        /// <summary>
        /// 安全检查
        /// </summary>
        public override bool SecurityCheck(RequestModel requestModel) => Container.GetPluginInstance<ISecurity<RequestModel>>().SecurityCheck(requestModel);


        /// <summary>
        /// 必须的组件容器
        /// </summary>
        public override IPluginContainer GetContainer() => new PluginPackage.PluginContainer();


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
            LogManager.WriteLog($"接收请求：body={RequestModel?.Tag}&token={RequestModel?.Token}&api={RequestModel?.Api}&timestamp={RequestModel?.Timestamp}&sign={RequestModel?.Sign} 输出结果：{resultStr}");
            return new MemoryStream(Encoding.UTF8.GetBytes(resultStr));
        }


    }
}
