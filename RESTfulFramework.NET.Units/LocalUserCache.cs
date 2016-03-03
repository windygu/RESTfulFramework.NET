using PluginPackage;
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units.Model;
using System;
using System.Collections.Generic;

namespace RESTfulFramework.NET.Units
{
    public class LocalUserCache : IUserCache<UserInfo>
    {

        public LocalUserCache() { }
        private static Dictionary<string, UserInfo> Client { get; set; }

        static LocalUserCache()
        {
            //所有用户基本信息缓存redis
            var users = Factory.GetInstance<IDBHelper>().QuerySql<List<Dictionary<string, object>>>($"SELECT * FROM `user`;");
            foreach (var user in users)
            {
                var redisuser = new UserInfo
                {
                    account_name = user["account_name"].ToString(),
                    account_type_id = user["account_type"].ToString(),
                    create_time = Convert.ToDateTime(user["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                    id = Guid.Parse(user["id"].ToString()),
                    passwrod = user["passwrod"].ToString(),
                    real_name = user["realname"].ToString()
                };
                Client.Add(redisuser.id.ToString(), redisuser);
            }
        }
        public bool ContainsUserInfo(string token) => Client.ContainsKey(token);


        public UserInfo GetUserInfo(string token) => Client.GetValueOrDefault(token);


        public bool RemoveUserInfo(string token) => Client.Remove(token);


        public bool SetUserInfo(UserInfo userInfo, string token)
        {
            if (!ContainsUserInfo(token))
            {
                Client.Add(token, userInfo);
                return true;
            }
            else {
                return false;
            }
        }
    }
}
