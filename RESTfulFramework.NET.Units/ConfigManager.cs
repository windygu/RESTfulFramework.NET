using RESTfulFramework.NET.ComponentModel;
using PluginPackage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace RESTfulFramework.NET.Units
{
    public class ConfigManager : IConfigManager<SysConfigModel>
    {
        private string ConnectionString { get; set; }
        private List<SysConfigModel> ConfigModels { get; set; }
        public ConfigManager()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["restfullframework"].ToString();
        }
        public SysConfigModel GetValue(string key)
        {
            if (ConfigModels == null)
            {
                var dbHelper = Factory.GetInstance<IDBHelper>();
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
                            remark = item["remark"].ToString()
                        });
                    }
                    return ConfigModels.FirstOrDefault(s => s.key == key);
                }
                else {
                    throw new Exception("无该配置信息");
                }
            }
            else {

                return ConfigModels.FirstOrDefault(s => s.key == key);
            }
        }

        public bool SetValue(string key, SysConfigModel value)
        {
            throw new NotImplementedException();
        }
    }
}
