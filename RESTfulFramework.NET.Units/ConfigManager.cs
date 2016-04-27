using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace RESTfulFramework.NET.Units
{
    public class ConfigManager : IConfigManager<SysConfigModel>
    {
        public string ConnectionString { get; set; }
        private List<SysConfigModel> ConfigModels { get; set; }
        public ConfigManager()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["RESTfulFrameworkConnection"].ToString();
        }
        public SysConfigModel GetValue(string key)
        {
            if (ConfigModels == null)
            {
                var dbHelper = new DBHelper();
                dbHelper.ConnectionString = ConnectionString;
                var result = dbHelper.QuerySql<List<Dictionary<string, object>>>($"SELECT * FROM sys_config");
                if (result != null && result.Any())
                {
                    ConfigModels = new List<SysConfigModel>();
                    foreach (var item in result)
                    {
                        ConfigModels.Add(new SysConfigModel
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

        public bool SetValue(string key, SysConfigModel value)
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
    }
}
