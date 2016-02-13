using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.ProtocolAccount.Model
{

    public class XMLResult
    {
        public string Result { get; set; }

        public string Description { get; set; }

        public string FailList { get; set; }

        public string Balance { get; set; }

        public List<Item> RecvList { get; set; }
    }

    public class Item
    {
        public string Mobile { get; set; }
        public string Content { get; set; }
        public string RecvTime { get; set; }
    }
}
