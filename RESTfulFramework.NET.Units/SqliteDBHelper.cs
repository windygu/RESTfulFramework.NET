using PluginPackage;
using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.Units
{
    public class SqliteDBHelper : IDBHelper
    {
        public SqliteDBHelper() { }

        private static IJsonSerialzer JsonSerialzer { get; set; }
        //private static ILogManager LogManager { get; set; }
        static SqliteDBHelper()
        {
            JsonSerialzer = Factory.GetInstance<IJsonSerialzer>();
            //LogManager = Factory.GetInstance<ILogManager>();
        }


        public string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["RESTfulFrameworkConnection"].ToString();

        public int ExcuteSql(string sql)
        {
            var dbconnection = new SQLiteConnection(ConnectionString);
            var dbcommand = new SQLiteCommand();

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
            var dbconnection = new SQLiteConnection(ConnectionString);
            var dbcommand = new SQLiteCommand();
            dbcommand.Connection = dbconnection;
            var dba = new SQLiteDataAdapter(dbcommand);
            if (dbconnection.State == ConnectionState.Closed) dbconnection.Open();
            dbcommand.CommandText = $"{sql};";
            var dt = new DataTable();
            dba.Fill(dt);
            var json = JsonSerialzer.SerializeObject(dt.ToDictionary());
            dbconnection.Close();
            return JsonSerialzer.DeserializeObject<T>(json);

        }
    }
}
