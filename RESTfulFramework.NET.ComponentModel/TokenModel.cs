using System.Runtime.Serialization;

namespace RESTfulFramework.NET.ComponentModel
{
    [DataContract]
    public class TokenModel
    {
        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string Token { get; set; }
    }
}
