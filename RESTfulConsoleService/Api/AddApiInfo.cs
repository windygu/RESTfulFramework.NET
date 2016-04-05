using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RESTfulFramework.NET.ComponentModel;

namespace RESTfulConsoleService.Api
{
    public class AddApiInfo : IInfoApi<RequestModel, ResponseModel>
    {
        public ResponseModel RunApi(RequestModel source)
        {
            var dic = source.Body as Dictionary<string, object>;
            var dbHelper = PluginPackage.Factory.GetInstance<IDBHelper>();
            var sql = $"insert into api_info (`name`,`api`,`body`,`requestMethod`,`url`) values('{dic["name"]}','{dic["api"]}','{dic["body"]}','{dic["requestMethod"]}','{dic["url"]}')";
            if (dbHelper.ExcuteSql(sql) > 0)
            {
                return new ResponseModel()
                {
                    Code = 1,
                    Msg = "添加成功"
                };
            }
            else
            {
                return new ResponseModel()
                {
                    Code = -1,
                    Msg = "添加失败"
                };
            }

        }
    }
}
