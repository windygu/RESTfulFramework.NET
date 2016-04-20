using RESTfulFramework.NET.ComponentModel;
using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace RESTfulFramework.NET.Units
{
    public class DBHelper : IDBHelper
    {
        public DBHelper() { }
 
        public DBHelper(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; } = ConfigurationManager.ConnectionStrings["RESTfulFrameworkConnection"].ToString();

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
            finally
            {
                dbconnection.Close();
            }
        }

        public T QuerySql<T>(string sql) where T : class
        {
            var dbconnection = new MySqlConnection(ConnectionString);
            try
            {

                var dbcommand = new MySqlCommand();
                dbcommand.Connection = dbconnection;
                var dba = new MySqlDataAdapter(dbcommand);
                if (dbconnection.State == ConnectionState.Closed) dbconnection.Open();
                dbcommand.CommandText = $"{sql};";
                var dt = new DataTable();
                dba.Fill(dt);
                var json = Common.UnitsFactory.JsonSerialzer.SerializeObject(dt.ToDictionary());
                dbconnection.Close();
                return Common.UnitsFactory.JsonSerialzer.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Common.UnitsFactory.LogManager.WriteLog(ex.Message);
                throw ex;
            }
            finally
            {
                dbconnection.Close();
            }

        }
        public List<Dictionary<string, object>> Query(string sql) => QuerySql<List<Dictionary<string, object>>>(sql);
    }
}
