using RESTfulFramework.NET.Common;
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units.Model;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;

namespace RESTfulFramework.NET.Units
{
    public class UserCache : IUserCache<UserInfo>
    {
        public UserCache() { }
        private static RedisClient Client { get; set; }

        static UserCache()
        {
            Client = new RedisClient(ConfigInfo.RedisAddress, int.Parse(ConfigInfo.RedisPort));

            //所有用户基本信息缓存redis
            var dbHelper = new DBHelper();
            var users = dbHelper.QuerySql<List<Dictionary<string, object>>>($"SELECT * FROM `user_view`;");
            foreach (var user in users)
            {
                var redisuser = new UserInfo
                {
                    account_name = user["account_name"].ToString(),
                    account_type_id = Unicode(user["account_type"].ToString()),
                    create_time = Convert.ToDateTime(user["create_time"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                    id = Guid.Parse(user["id"].ToString()),
                    passwrod = user["passwrod"].ToString(),
                    real_name = Unicode(user["realname"].ToString()),
                    company_name = user["company_name"].ToString(),
                    data_library_conntection = user["data_library_conntection"].ToString(),
                    company_name_id = user["company_id"].ToString(),
                    data_library_name = user["data_library_name"].ToString()
                };
                Client.Set(redisuser.id.ToString(), redisuser);
            }
        }

        /// <summary>
        /// 获取Redis缓存的用户信息
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns>返回用户信息</returns>
        public UserInfo GetUserInfo(string token) => Client.Get<UserInfo>(token);

        /// <summary>
        /// 将用户信息保存在Redis缓存
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="token">用户token</param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool SetUserInfo(UserInfo userInfo, string token) => Client.Set(token, userInfo);

        /// <summary>
        /// 删除用户缓存信息
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool RemoveUserInfo(string token) => Client.Remove(token);

        /// <summary>
        /// 是否存在用户信息
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public bool ContainsUserInfo(string token) => Client.ContainsKey(token);

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

    }
}
