using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RESTfulConsoleService.Api
{
    public class DeleteApiInfo : IInfoApi<RequestModel, ResponseModel>
    {
        public ResponseModel RunApi(RequestModel source)
        {
            return new ResponseModel() { Code = -1, Msg = null };

            var dic = source.Body as Dictionary<string, object>;
            var api = dic["id"];
            var sql = $"delete from api_info where id={api};";
            var dbHelper = PluginPackage.Factory.GetInstance<IDBHelper>();
            var result = dbHelper.ExcuteSql(sql);
            if (result > 0)
                return new ResponseModel() { Code = 1, Msg = result };
            return new ResponseModel() { Code = -1, Msg = null };
        }
    }
}
