using RESTfulFramework.NET.ComponentModel;
using MySql.Data.MySqlClient;
using PluginPackage;
using System;
using System.Configuration;
using System.Data;
using System.Linq;

namespace RESTfulFramework.NET.Units
{
    public class DBHelper : IDBHelper
    {
        public DBHelper() { }

        private static IJsonSerialzer JsonSerialzer { get; set; }
        static DBHelper()
        {
            JsonSerialzer = Factory.GetInstance<IJsonSerialzer>();
        }

        public string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["restfullframework"].ToString();

        public int ExcuteSql(string sql)
        {
            var dbconnection = new MySqlConnection(ConnectionString);
            var dbcommand = new MySqlCommand();

            dbcommand.Connection = dbconnection;
            if (dbconnection.State == ConnectionState.Closed) dbconnection.Open();

            dbcommand.Transaction = dbconnection.BeginTransaction();
            dbcommand.CommandText = sql;

            try
            {
                var i = dbcommand.ExecuteNonQuery();
                dbcommand.Transaction.Commit();
                dbconnection.Close();
                return i;
            }
            catch (Exception ex)
            {
                dbcommand.Transaction.Rollback();
                throw ex;
            }
        }

        public T QuerySql<T>(string sql) where T : class
        {
            try
            {


                var dbconnection = new MySqlConnection(ConnectionString);
                var dbcommand = new MySqlCommand();
                dbcommand.Connection = dbconnection;
                var dba = new MySqlDataAdapter(dbcommand);
                if (dbconnection.State == ConnectionState.Closed) dbconnection.Open();
                dbcommand.CommandText = $"{sql};";
                var dt = new DataTable();
                dba.Fill(dt);
                var json = JsonSerialzer.SerializeObject(dt.ToDictionary());
                dbconnection.Close();
                return JsonSerialzer.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
