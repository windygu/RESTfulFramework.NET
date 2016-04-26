using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace RESTfulFramework.NET.Units
{
    public class SqliteDBHelper : IDBHelper
    {

        public static IJsonSerialzer JsonSerialzer { get; set; }
        public static ILogManager LogManager { get; set; }

        static SqliteDBHelper()
        {
            JsonSerialzer = new JsonSerialzer();
            LogManager = new LogManager();
        }
        public SqliteDBHelper() { }

        public SqliteDBHelper(string connectionString)

        {
            ConnectionString = connectionString;
        }


        public string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["RESTfulFrameworkConnection"].ToString();

        public int ExcuteSql(string sql) => ExcuteSql(sql, ConnectionString);



        public T QuerySql<T>(string sql) where T : class => QuerySql<T>(sql, ConnectionString);


        public List<Dictionary<string, object>> Query(string sql) => QuerySql<List<Dictionary<string, object>>>(sql);

        public int ExcuteSql(string sql, string connectionString)
        {
            var dbconnection = new SQLiteConnection(connectionString);
            var dbcommand = new SQLiteCommand();

            dbcommand.Connection = dbconnection;
            if (dbconnection.State == ConnectionState.Closed) dbconnection.Open();

            dbcommand.Transaction = dbconnection.BeginTransaction();
            dbcommand.CommandText = sql;

            try
            {
                var i = dbcommand.ExecuteNonQuery();
                dbcommand.Transaction.Commit();

                return i;
            }
            catch (Exception ex)
            {
                dbcommand.Transaction.Rollback();
                throw ex;
            }
            finally
            {

                dbconnection.Close();
            }
        }

        public T QuerySql<T>(string sql, string connectionString) where T : class
        {
            var dbconnection = new SQLiteConnection(connectionString);
            try
            {
                var dbcommand = new SQLiteCommand();
                dbcommand.Connection = dbconnection;
                var dba = new SQLiteDataAdapter(dbcommand);
                if (dbconnection.State == ConnectionState.Closed) dbconnection.Open();
                dbcommand.CommandText = $"{sql};";
                var dt = new DataTable();
                dba.Fill(dt);
                var json = JsonSerialzer.SerializeObject(dt.ToDictionary());
                dba.Dispose();
                return JsonSerialzer.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                LogManager.WriteLog(ex.Message);
                throw ex;
            }
            finally
            {
                dbconnection.Close();
            }
        }

        public List<Dictionary<string, object>> Query(string sql, string connectionString) => QuerySql<List<Dictionary<string, object>>>(sql, connectionString);

    }
}
