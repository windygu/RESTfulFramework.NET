using RESTfulFramework.NET.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace RESTfulConsoleService.Api
{
    public class GetSingleApiInfo : IInfoApi<RequestModel, ResponseModel>
    {
        public ResponseModel RunApi(RequestModel source)
        {
            var dic = source.Body as Dictionary<string, object>;
            var api = dic["id"];
            var sql = $"select * from api_info where id={api}";
            var dbHelper = PluginPackage.Factory.GetInstance<IDBHelper>();
            var result = dbHelper.QuerySql<List<Dictionary<string, object>>>(sql);
            if (result != null && result.Any() && result.Count > 0)
                return new ResponseModel() { Code = 1, Msg = result[0] };

            return new ResponseModel() { Code = -1, Msg = null };
        }
    }
}
