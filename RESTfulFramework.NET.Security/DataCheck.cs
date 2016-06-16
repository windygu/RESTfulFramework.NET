using RESTfulFramework.NET.ComponentModel;
using System;

namespace RESTfulFramework.NET.Security
{
    public class DataCheck<TUserInfoModel> : ISecurity<RequestModel<TUserInfoModel>>
        where TUserInfoModel : IBaseUserInfo
    {
        /// <summary>
        /// 对时间戳进行校验
        /// </summary>
        /// <param name="requestModel">请求的模型</param>
        public Tuple<bool, string, int> SecurityCheck(RequestModel<TUserInfoModel> requestModel)
        {
            long timestamp;
            try
            {
                if (requestModel.Timestamp.Length != 13 && requestModel.Timestamp.Length != 10)
                {
                    return new Tuple<bool, string, int>(false, $"时间戳格式不正确。正确的时间戳应该是13位或者10位。{requestModel.Timestamp.Length}", Code.TimestampError);
                }
                timestamp = requestModel.Timestamp.Length == 13 ? long.Parse(requestModel.Timestamp) / 1000 : long.Parse(requestModel.Timestamp);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, int>(false, $"时间戳格式不正确，无法进行转换。{ex.Message}", Code.TimestampError);
            }

            var epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            if (Math.Abs((epoch - timestamp)) > 600)
            {
                return new Tuple<bool, string, int>(false, "时间戳超过有效时间。", Code.TimestampError);
            }

            return new Tuple<bool, string, int>(true, "时间戳正常。", Code.TimestampError);
        }
    }
}
