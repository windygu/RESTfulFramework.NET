using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.Units
{
    public class SmsManager : ISmsManager
    {
        public string ReadSms(string phone)
        {
            return "短信内容";
        }

        public bool SendSms(string phone, string content)
        {
            return true;
        }
    }
}
