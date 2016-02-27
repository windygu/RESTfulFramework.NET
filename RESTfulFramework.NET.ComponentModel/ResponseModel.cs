using System.Runtime.Serialization;

namespace RESTfulFramework.NET.ComponentModel
{
    [DataContract]
    public class ResponseModel
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public object Msg { get; set; }
    }
}
