using System.Runtime.Serialization;

namespace RESTfulFramework.IProtocolPlugin.Model
{
    /// <summary>
    /// 状态结果
    /// </summary>
    /// <typeparam name="T">状态详细信息</typeparam>
    [DataContract]
    public class Result<T>
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public T Msg { get; set; }
    }
}
