using RESTfulFramework.NET.Common;
using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.Security
{
    public class DataSecurity : ISecurity<RequestModel>
    {
        private static string AccountSecretKey { get; set; }

        static DataSecurity()
        {
            var configInfo = new Factory.UnitsFactory<RequestModel, ResponseModel>().GetConfigManager().GetConfigInfo();
            AccountSecretKey = configInfo.AccountSecretKey;
        }


        public bool SecurityCheck(RequestModel requestModel)
        {
            //仅用于调试
            if (requestModel.Sign == "ignor") return true;
            //校验签名 (token+protocol+timestamp + 密钥)的MD5
            var sign = Md5.GetMd5(requestModel.Token + requestModel.Api + requestModel.Timestamp + AccountSecretKey, Encoding.UTF8);
            return sign == requestModel.Sign ? true : false;
        }
    }
}
