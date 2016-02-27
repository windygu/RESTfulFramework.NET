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
            //时间有限期
            //SQL防注入
            return true;
        }
    }
}
