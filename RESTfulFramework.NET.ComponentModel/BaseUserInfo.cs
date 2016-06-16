using System;
using System.Runtime.Serialization;

namespace RESTfulFramework.NET.ComponentModel
{
    [DataContract]
    public class BaseUserInfo : IBaseUserInfo
    {
        [DataMember]
        public Guid id { get; set; }

        [DataMember]
        public string account_name { get; set; }
        [DataMember]
        public string passwrod { get; set; }

        [DataMember]
        public string account_type_id { get; set; }

        [DataMember]
        public string create_time { get; set; }
        [DataMember]
        public string real_name { get; set; }

        [DataMember]
        public string client_id { get; set; }
      
    }


    public interface IBaseUserInfo
    {
        Guid id { get; set; }

        string account_name { get; set; }

        string passwrod { get; set; }

        string account_type_id { get; set; }

        string create_time { get; set; }

        string real_name { get; set; }

        string client_id { get; set; }
    }
}
