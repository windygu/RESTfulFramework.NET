using DynamicDapper;
using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;

namespace RESTfulFramework.NET.Units
{
    public class TableUserCache<TConfigManager, TConfigModel> : IUserCache<UserInfo>
        where TConfigManager : IConfigManager<TConfigModel>, new()
        where TConfigModel : IConfigModel
    {
        private static string ConnectionString { get; set; }
        private static string tmpUserCacheTableName = "tmp_user_cache";
        static TableUserCache()
        {
            try
            {
                ConnectionString = new TConfigManager().GetConnectionString();
                var entityModelFactory = new EntityModelFactory();
                entityModelFactory.ConnectionString = ConnectionString;

                tmpUserCacheTableName = "tmp_user_cache";

                if (entityModelFactory.ExistModelMeta(tmpUserCacheTableName)) return;

                var entityModelMeta = new EntityModelMeta();
                entityModelMeta.EntityName = tmpUserCacheTableName;
                entityModelMeta.ConnectionString = ConnectionString;

                entityModelMeta.Add("id", new EntityKeyMetaType
                {
                    EntityKeyType = typeof(Guid),
                    FieldType = new KeyValuePair<string, string>[2]
                    {
                    new KeyValuePair<string, string>(SqlFieldType.NotNull,""),
                    new KeyValuePair<string, string>(SqlFieldType.PrimaryKey,"")
                     }
                });

                entityModelMeta.Add("key", new EntityKeyMetaType
                {
                    EntityKeyType = typeof(string),
                    FieldType = new KeyValuePair<string, string>[1]
                     {
                     new KeyValuePair<string, string>(SqlFieldType.NotNull,"")
                     }
                });

                entityModelMeta.Add("value", new EntityKeyMetaType
                {
                    EntityKeyType = typeof(StringBuilder)
                });

                entityModelMeta.Add("create_time", new EntityKeyMetaType
                {
                    EntityKeyType = typeof(DateTime),
                    FieldType = new KeyValuePair<string, string>[1]
                    {
                        new KeyValuePair<string, string>(SqlFieldType.DefaultValueCurrentDateTime,"")
                    }
                });

                entityModelMeta.Add("update_time", new EntityKeyMetaType
                {
                    EntityKeyType = typeof(DateTime),
                    FieldType = new KeyValuePair<string, string>[1]
                    {
                        new KeyValuePair<string, string>(SqlFieldType.DefaultValueCurrentDateTimeAndUpdateTime,"")
                    }
                });

                entityModelFactory.CreateEntityModelMeta(entityModelMeta);

            }
            catch (Exception)
            {
            }

        }
        public TableUserCache()
        {
        }

        public bool Contains(string key)
        {
            try
            {
                var entityModelFactory = new EntityModelFactory();
                entityModelFactory.ConnectionString = ConnectionString;
                var entityModels = entityModelFactory.GetEntityModels<EntityModel>(tmpUserCacheTableName, $"select * from {tmpUserCacheTableName} where key='{key}'", "id");
                if (entityModels != null && entityModels.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ContainsUserInfo(string key)
        {
            return Contains(key);
        }

        public Dictionary<string, object> GetAll()
        {
            try
            {
                var entityModelFactory = new EntityModelFactory();
                entityModelFactory.ConnectionString = ConnectionString;
                var entityModels = entityModelFactory.GetEntityModels<EntityModel>(tmpUserCacheTableName, $"select * from {tmpUserCacheTableName}", "id");
                if (entityModels != null && entityModels.Any())
                {
                    var dictionary = new Dictionary<string, object>();
                    foreach (var item in entityModels)
                    {
                        dictionary.Add(item["key"].ToString(), Json2KeyValue.JsonConvert.DeserializeObject<UserInfo>(item["value"].ToString()));
                    }
                    return dictionary;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public UserInfo GetUserInfo(string key)
        {
            return Json2KeyValue.JsonConvert.DeserializeObject<UserInfo>(GetValue(key));
        }

        public string GetValue(string key)
        {
            try
            {
                var entityModelFactory = new EntityModelFactory();
                entityModelFactory.ConnectionString = ConnectionString;
                var entityModels = entityModelFactory.GetEntityModels<EntityModel>(tmpUserCacheTableName, $"select * from {tmpUserCacheTableName} where key='{key}'", "id");
                if (entityModels != null && entityModels.Any())
                {
                    foreach (var item in entityModels)
                    {
                        return item["value"].ToString();
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool RefreshCache()
        {
            try
            {
                var dbHelper = new DBHelper();
                dbHelper.ConnectionString = new ConfigManager().GetConnectionString();
                var users = dbHelper.QuerySql<List<Dictionary<string, object>>>($"SELECT * FROM `user`;");
                foreach (var user in users)
                {
                    var redisuser = new UserInfo
                    {
                        account_name = user["account_name"]?.ToString(),
                        account_type_id = user["account_type"]?.ToString(),
                        create_time = Convert.ToDateTime(user["create_time"]?.ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                        id = Guid.Parse(user["id"]?.ToString()),
                        passwrod = user["passwrod"]?.ToString(),
                        real_name = user["realname"]?.ToString()
                    };
                    SetUserInfo(redisuser, redisuser.id.ToString());
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveUserInfo(string key)
        {
            var connection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
            return connection.Execute($"delete from {tmpUserCacheTableName} where key='{key}'") > 0;
        }

        public bool SetUserInfo(UserInfo userInfo, string key)
        {
            return SetValue(Json2KeyValue.JsonConvert.SerializeObject(userInfo), key);
        }

        public bool SetValue(string value, string key)
        {
            try
            {
                var entityModelFactory = new EntityModelFactory();
                entityModelFactory.ConnectionString = ConnectionString;
                var entityModels = entityModelFactory.GetEntityModels<EntityModel>(tmpUserCacheTableName, $"select * from {tmpUserCacheTableName} where key='{key}';", "id");
                if (entityModels != null && entityModels.Any() && entityModels.Count > 0)
                {
                    foreach (var item in entityModels)
                    {
                        item[key] = value;
                        item.SaveChange();
                    }
                    return true;
                }
                else
                {
                    var entityModel = new EntityModel();
                    entityModel.ConnectionString = ConnectionString;
                    entityModel.EntityName = tmpUserCacheTableName;
                    entityModel.PrimaryKey = new KeyValuePair<string, object>("id", Guid.NewGuid());
                    entityModel.Add("key", key);
                    entityModel.Add("value", value);
                    entityModelFactory.AddEntityModel(entityModel);
                    return true;

                }
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
