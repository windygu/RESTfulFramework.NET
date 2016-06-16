using RESTfulFramework.NET.ComponentModel;
using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace RESTfulFramework.NET.Units
{
    public class UserCache<TConfigManager, TConfigModel> : IUserCache<UserInfo>
        where TConfigManager : IConfigManager<TConfigModel>, new()
        where TConfigModel : IConfigModel
    {
        private static IDatabase RedisDataBase { get; set; }
        private static IServer RedisServer { get; set; }
        private static List<string> Keys { get; set; }
        static UserCache()
        {
            //所有用户基本信息缓存redis
            Refresh();
            var configManager = new TConfigManager();
            var accountSecretKey = configManager.GetValue("account_secret_key")?.value;
            var smsAccount = configManager.GetValue("sms_account")?.value;
            var smsPassword = configManager.GetValue("sms_password")?.value;
            var smsCodeContent = configManager.GetValue("sms_code_content")?.value;
            var redisAddress = configManager.GetValue("redis_address")?.value;
            var redisPort = configManager.GetValue("redis_port")?.value;

            var client = ConnectionMultiplexer.Connect($"{redisAddress}:{redisPort},allowAdmin=true");
            RedisDataBase = client.GetDatabase();
            RedisServer = client.GetServer($"{redisAddress}:{redisPort}");
            Keys = new List<string>();
        }
        public UserCache()
        {

            Refresh();
        }

        /// <summary>
        /// 获取Redis缓存的用户信息
        /// </summary>
        /// <param name="key">用户token</param>
        /// <returns>返回用户信息</returns>
        public UserInfo GetUserInfo(string key)
        {
            lock (RedisDataBase)
            {
                var usrinfo = Json2KeyValue.JsonConvert.DeserializeObject<UserInfo>(RedisDataBase.StringGet(key).ToString());
                return usrinfo;
            }

        }

        /// <summary>
        /// 将用户信息保存在Redis缓存
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="key">用户token</param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool SetUserInfo(UserInfo userInfo, string key)
        {
            lock (RedisDataBase)
            {
                return RedisDataBase.StringSet(key, Json2KeyValue.JsonConvert.SerializeObject(userInfo), new TimeSpan(365, 0, 0, 0));

            }
        }

        /// <summary>
        /// 删除用户缓存信息
        /// </summary>
        /// <param name="key">用户token</param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool RemoveUserInfo(string key)
        {
            lock (RedisDataBase)
            {
                return RedisDataBase.KeyDelete(key);
            }
        }

        /// <summary>
        /// 是否存在用户信息
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public bool ContainsUserInfo(string key)
        {
            lock (RedisDataBase)
            {
                return RedisDataBase.KeyExists(key);
            }

        }
        private static string Unicode(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    //将中文字符转为10进制整数，然后转为16进制unicode字符  
                    outStr += "\\u" + ((int)str[i]).ToString("x");
                }
            }
            return outStr;
        }

        public bool Contains(string key)
        {
            lock (RedisDataBase)
            {
                return RedisDataBase.KeyExists(key);
            }
        }


        public string GetValue(string key)
        {
            lock (RedisDataBase)
            {
                return RedisDataBase.StringGet(key);
            }
        }


        public bool SetValue(string value, string key)
        {
            lock (RedisDataBase)
            {
                return RedisDataBase.StringSet(key, value, new TimeSpan(365, 0, 0, 0));
            }
        }

        public Dictionary<string, object> GetAll()
        {
            lock (RedisDataBase)
            {
                lock (RedisServer)
                {
                    var dictionary = new Dictionary<string, object>();
                    foreach (var item in RedisServer.Keys())
                    {
                        dictionary.Add(item, RedisDataBase.StringGet(item));
                    }
                    return dictionary;
                }
            }
        }

        public bool RefreshCache()
        {
            try
            {
                Refresh();
                return true;
            }
            catch (Exception ex)
            {
                LogManager.GetCurrLogManager().Error(ex.ToString());
                return false;
            }
        }

        private static void Refresh()
        {
            //所有用户基本信息缓存redis
            var dbHelper = new DBHelper();
            var users = dbHelper.QuerySql<List<Dictionary<string, object>>>($"SELECT * FROM `user_view`;");
            foreach (var user in users)
            {
                try
                {
                    lock (RedisDataBase)
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
                        RedisDataBase.StringSet(redisuser.id.ToString(), Json2KeyValue.JsonConvert.SerializeObject(redisuser), new TimeSpan(365, 0, 0, 0));
                    }
                }
                catch (Exception ex)
                {
                    LogManager.GetCurrLogManager().Error(ex.ToString());
                }
            }
        }
    }
}
