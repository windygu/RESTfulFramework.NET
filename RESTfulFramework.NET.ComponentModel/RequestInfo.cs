using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace RESTfulFramework.NET.ComponentModel
{
    [MessageContract]
    public class RequestInfo : IDisposable
    {
        //string token, string api, string timestamp, string sign, Stream stream
        [MessageHeader(MustUnderstand = true)]
        public string token { get; set; }
        [MessageHeader(MustUnderstand = true)]
        public string api { get; set; }
        [MessageHeader(MustUnderstand = true)]
        public string timestamp { get; set; }
        [MessageHeader(MustUnderstand = true)]
        public string sign { get; set; }
        [MessageHeader(MustUnderstand = true)]
        public string body { get; set; }
        [MessageBodyMember(Order = 1)]
        public Stream stream { get; set; }

        public void Dispose()
        {
            if (stream != null)
            {
                stream.Close();
                stream = null;
            }
        }
    }

}
