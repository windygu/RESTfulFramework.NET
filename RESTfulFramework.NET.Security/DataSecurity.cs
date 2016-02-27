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
        public bool SecurityCheck(RequestModel requestModel)
        {
            //仅用于调试
            if (requestModel.Sign == "ignor") return true;
            //校验签名 (token+protocol+timestamp + 密钥)的MD5
            var sign = Md5.GetMd5(requestModel.Token + requestModel.Api + requestModel.Timestamp + ConfigInfo.AccountSecretKey, Encoding.UTF8);
            return sign == requestModel.Sign ? true : false;
        }
    }
}
