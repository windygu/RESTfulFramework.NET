using RESTfulFramework.NET.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace RESTfulConsoleService.Api
{
    public class GetApiInfo : IInfoApi<RequestModel, ResponseModel>
    {
        public ResponseModel RunApi(RequestModel source)
        {
            //返回API接口信息列表
            var dic = source.Body as Dictionary<string, object>;
            var dbHelper = PluginPackage.Factory.GetInstance<IDBHelper>();
            var sql = $"select * from api_info";
            var result = dbHelper.QuerySql<List<Dictionary<string, object>>>(sql);
            if (result != null && result.Any() && result.Count > 0)
            {
                return new ResponseModel()
                {
                    Code = 1,
                    Msg = result
                };
            }
            else
            {
                return new ResponseModel()
                {
                    Code = -1,
                    Msg = null
                };
            }
        }
    }
}
