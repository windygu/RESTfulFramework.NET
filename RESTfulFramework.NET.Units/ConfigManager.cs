using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace RESTfulFramework.NET.Units
{
    public class ConfigManager : IConfigManager<ConfigModel>
    {
        public string ConnectionString { get; set; }
        private static List<ConfigModel> ConfigModels { get; set; }
        public ConfigManager() { }
        public ConfigManager(string connectionString)
            : this()
        {
            ConnectionString = connectionString;
        }
        public ConfigModel GetValue(string key)
        {
            if (ConfigModels == null)
            {
                var dbHelper = new DBHelper(ConnectionString);
                var result = dbHelper.QuerySql<List<Dictionary<string, object>>>($"SELECT * FROM sys_config");
                if (result != null && result.Any())
                {
                    ConfigModels = new List<ConfigModel>();
                    foreach (var item in result)
                    {
                        ConfigModels.Add(new ConfigModel
                        {
                            key = item["key"].ToString(),
                            value = item["value"].ToString(),
                            remark = item["remark"]?.ToString()
                        });
                    }
                    return ConfigModels.FirstOrDefault(s => s.key == key);
                }
                else
                {
                    throw new Exception("数据库表sys_config为空，加载基础配置信息失败。");
                }
            }
            else
            {

                return ConfigModels.FirstOrDefault(s => s.key == key);
            }
        }

        public bool SetValue(string key, ConfigModel value)
        {
            throw new NotImplementedException();
        }

        public ConfigInfo GetConfigInfo()
        {
            return new ConfigInfo
            {
                AccountSecretKey = this.GetValue("account_secret_key")?.value,
                SmsAccount = this.GetValue("sms_account")?.value,
                SmsPassword = this.GetValue("sms_password")?.value,
                SmsCodeContent = this.GetValue("sms_code_content")?.value,
                RedisAddress = this.GetValue("redis_address")?.value,
                RedisPort = this.GetValue("redis_port")?.value,
            };

        }

        public string GetConnectionString()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["RESTfulFrameworkConnection"]?.ToString();
            return ConnectionString;
        }
    }
}
