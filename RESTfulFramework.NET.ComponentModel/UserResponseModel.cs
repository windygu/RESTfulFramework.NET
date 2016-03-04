using System.Runtime.Serialization;

namespace RESTfulFramework.NET.ComponentModel
{
    [DataContract]
    public class UserResponseModel<T>
    {
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public T Msg { get; set; }
    }
}
