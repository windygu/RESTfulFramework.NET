using RESTfulFramework.NET.Common;
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units.Model;
using PluginPackage;
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

    }
}
