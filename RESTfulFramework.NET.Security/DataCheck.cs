using RESTfulFramework.NET.ComponentModel;
using System;

namespace RESTfulFramework.NET.Security
{
    public class DataCheck : ISecurity<RequestModel>
    {
        public bool SecurityCheck(RequestModel requestModel)
        {
            long timestamp;
            try
            {

                if (requestModel.Timestamp.Length != 13 && requestModel.Timestamp.Length != 10) throw new Exception($"时间戳格式不正确。正确的时间戳应该是13位或者10位。{requestModel.Timestamp.Length}");
                timestamp = requestModel.Timestamp.Length == 13 ? long.Parse(requestModel.Timestamp) / 1000 : long.Parse(requestModel.Timestamp);
            }
            catch (Exception ex)
            {
                throw new Exception($"时间戳格式不正确。{ex.Message}");
            }

            var epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            if (Math.Abs((epoch - timestamp)) > 600)
            {
                return false;
            }

            return true;
        }
    }
}
