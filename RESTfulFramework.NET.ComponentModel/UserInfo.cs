using System;
using System.Runtime.Serialization;

namespace RESTfulFramework.NET.Units.Model
{
    [DataContract]
    [Serializable]
    public class UserInfo
    {
        private string _password;
        public UserInfo() { }

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

        [DataMember]
        public string company_name_id { get; set; }

        [DataMember]
        public string company_name { get; set; }

        [DataMember]
        public string data_library_conntection { get; set; }

        [DataMember]
        public string data_library_name { get; set; }
    }
}
