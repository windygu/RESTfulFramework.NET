
using RESTfulFramework.NET.ComponentModel;
 
using System;
using System.Collections.Generic;

namespace RESTfulFramework.NET.Units
{
    public class LocalUserCache : IUserCache<BaseUserInfo>
    {

        public LocalUserCache() { }
    
        private static Dictionary<string, BaseUserInfo> Client { get; set; } = new Dictionary<string, BaseUserInfo>();
 
        static LocalUserCache()
        {
            //所有用户基本信息缓存redis
            var dbHelper =  new  DBHelper();
            var users = dbHelper.QuerySql<List<Dictionary<string, object>>>($"SELECT * FROM `user`;");
            foreach (var user in users)
            {
                var redisuser = new BaseUserInfo
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


        public BaseUserInfo GetUserInfo(string token) => Client.GetValueOrDefault(token);


        public bool RemoveUserInfo(string token) => Client.Remove(token);


        public bool SetUserInfo(BaseUserInfo userInfo, string token)
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
