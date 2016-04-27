using RESTfulFramework.NET.Units.Model;
using System.Runtime.Serialization;

namespace RESTfulFramework.NET.ComponentModel
{
    [DataContract]
    public class RequestModel
    {
        [DataMember]
        public object Body { get; set; }
        [DataMember]
        public string Token { get; set; }
        [DataMember]
        public string Api { get; set; }
        [DataMember]
        public string Timestamp { get; set; }
        [DataMember]
        public string Sign { get; set; }
        [DataMember]
        public object Tag { get; set; }
        [DataMember]
        public UserInfo UserInfo { get; set; }
    }
}
