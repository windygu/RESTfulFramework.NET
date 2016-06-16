
using System.Runtime.Serialization;

namespace RESTfulFramework.NET.ComponentModel
{
    [DataContract]
    public class RequestModel<TUserInfo>
        where TUserInfo : IBaseUserInfo
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
        public TUserInfo UserInfo { get; set; }
        [DataMember]
        public string BodyString { get; set; }
    }
}
