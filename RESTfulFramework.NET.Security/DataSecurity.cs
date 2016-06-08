using RESTfulFramework.NET.Common;
using RESTfulFramework.NET.ComponentModel;
using System;
using System.Text;

namespace RESTfulFramework.NET.Security
{
    public class DataSecurity<TConfigManager> : ISecurity<RequestModel<BaseUserInfo>>
        where TConfigManager : IConfigManager<SysConfigModel>, new()
    {
        private static string AccountSecretKey { get; set; }

        static DataSecurity()
        {
            var configInfo = new TConfigManager().GetConfigInfo();
            AccountSecretKey = configInfo.AccountSecretKey;
        }


        public Tuple<bool, string,int> SecurityCheck(RequestModel<BaseUserInfo> requestModel)
        {
            //仅用于调试
            //if (requestModel.Sign == "ignor") return true;
            //校验签名 (token+protocol+timestamp + 密钥)的MD5
            var sign = Md5.GetMd5(requestModel.Token + requestModel.Api + requestModel.Timestamp + AccountSecretKey, Encoding.UTF8);
            if (sign == requestModel.Sign)
            {
                //签名正确
                return new Tuple<bool, string,int>(true, "签名正确。",Code.Sucess);
            }
            else
            {
                //签名不正确
                return new Tuple<bool, string,int>(false, "签名不正确。",Code.SignErron);
            }

        }
    }
}
