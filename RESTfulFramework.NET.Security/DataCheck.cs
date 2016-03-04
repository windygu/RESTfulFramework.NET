using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.Security
{
    public class DataCheck : ISecurity<RequestModel>
    {
        public bool SecurityCheck(RequestModel requestModel)
        {
            long timestamp;
            try
            {
                
                timestamp = requestModel.Timestamp.Length==12? long.Parse(requestModel.Timestamp)/100: long.Parse(requestModel.Timestamp);
            }
            catch (Exception ex)
            {
                throw new Exception($"时间戳格式不正确。{ex.Message}");
            }

            var epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            if ((epoch - timestamp) > 600)
            {
                return false;
            }

            return true;
        }
    }
}
