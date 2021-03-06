﻿
using RESTfulFramework.NET.ComponentModel;

using System;
using System.Collections.Generic;

namespace RESTfulFramework.NET.Units
{
    public class LocalUserCache : IUserCache<UserInfo>

    {

        public LocalUserCache() { }

        //有一定的性能损失，后期优化
        private static Dictionary<string, object> Client { get; set; } = new Dictionary<string, object>();

        static LocalUserCache()
        {

            Refresh();
        }
        public bool ContainsUserInfo(string key) => Client.ContainsKey(key);


        public UserInfo GetUserInfo(string key) => (UserInfo)Client[key];


        public bool RemoveUserInfo(string key) => Client.Remove(key);


        public bool SetUserInfo(UserInfo userInfo, string key)
        {
            if (!ContainsUserInfo(key))
            {
                Client.Add(key, userInfo);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Contains(string key) => ContainsUserInfo(key);


        public string GetValue(string key) => Client[key]?.ToString();



        public bool SetValue(string value, string key)
        {
            if (!Contains(key))
            {
                Client.Add(key, value);
                return true;
            }
            else
            {
                Client[key] = value;
                return true;
            }
        }

        public Dictionary<string, object> GetAll()
        {
            return Client;
        }

        public bool RefreshCache()
        {
            try
            {
                Refresh();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void Refresh()
        {
            //所有用户基本信息缓存redis

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

                if (!Client.ContainsKey(redisuser.id.ToString()))
                {
                    Client.Add(redisuser.id.ToString(), redisuser);
                }
                else
                {
                    Client[redisuser.id.ToString()] = redisuser;
                }
            }
        }
    }
}
