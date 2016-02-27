namespace RESTfulFramework.NET.ComponentModel
{
    public interface IDBHelper
    {
        string ConnectionString { get; set; }
        int ExcuteSql(string sql);
        T QuerySql<T>(string sql) where T : class;
    }
}
