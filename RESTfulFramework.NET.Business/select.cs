using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Business
{
    public class select : IApiPlugin<RequestModel, ResponseModel>
    {
        public ResponseModel RunApi(RequestModel source)
        {
            return null;
            //try
            //{
            //    var jsonEntity = new JsonEntityToSql();
            //    var sql = jsonEntity.Json2SelectSql((string)source.Tag);
            //    var dbHelper = Factory.GetInstance<IDBHelper>();
            //    var dbResult = dbHelper.QuerySql<object>(sql);
            //    if (dbResult != null)
            //        return new ResponseModel { Code = Code.Sucess, Msg = dbResult };
            //    else
            //        return new ResponseModel { Code = Code.Fail, Msg = "没有数据" };
            //}
            //catch (Exception ex)
            //{
            //    return new ResponseModel { Code = Code.SystemException, Msg = ex.Message };
            //}
        }
    }
}
