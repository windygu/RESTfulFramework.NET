using RESTfulFramework.NET.ComponentModel;
using System.Collections.Generic;

namespace RESTfulConsoleService.Api
{
    public class UpdateSingleApiInfo : IInfoApi<RequestModel, ResponseModel>
    {
        public ResponseModel RunApi(RequestModel source)
        {
            var dic = source.Body as Dictionary<string, object>;
            var dbHelper = PluginPackage.Factory.GetInstance<IDBHelper>();
            var sql = $"update api_info set `name`='{dic["name"]}',`body`='{dic["body"]}',`requestMethod`='{dic["requestMethod"]}',`url`='{dic["url"]}' where `id`={dic["id"]};";
            if (dbHelper.ExcuteSql(sql) > 0)
                return new ResponseModel() { Code = 1, Msg = "修改成功" };

            return new ResponseModel() { Code = -1, Msg = "修改失败" };

        }
    }
}
