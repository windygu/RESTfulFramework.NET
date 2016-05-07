using RESTfulFramework.NET.ComponentModel;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;

namespace RESTfulFramework.NET.Units
{
    public class UserCache<TUserInfoModel> : IUserCache<TUserInfoModel>
      where TUserInfoModel : BaseUserInfo, new()
    {
        public UserCache() { }
        private static RedisClient Client { get; set; }

        static UserCache()
        {
            var configManager = new ConfigManager();
            var configInfo = configManager.GetConfigInfo();
            Client = new RedisClient(configInfo.RedisAddress, int.Parse(configInfo.RedisPort));

            //所有用户基本信息缓存redis
            var dbHelper = new DBHelper();
            var users = dbHelper.QuerySql<List<Dictionary<string, object>>>($"SELECT * FROM `user`;");
            foreach (var user in users)
            {
                var redisuser = new TUserInfoModel
                {
                    account_name = user["account_name"]?.ToString(),
                    account_type_id = Unicode(user["account_type"]?.ToString()),
                    create_time = Convert.ToDateTime(user["create_time"]?.ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                    id = Guid.Parse(user["id"]?.ToString()),
                    passwrod = user["passwrod"]?.ToString(),
                    real_name = Unicode(user["realname"]?.ToString()),
                    //company_name = user["company_name"].ToString(),
                    //data_library_conntection = user["data_library_conntection"].ToString(),
                    //company_name_id = user["company_id"].ToString(),
                    //data_library_name = user["data_library_name"].ToString()
                };
                Client.Set(redisuser.id.ToString(), redisuser);
            }
        }

        /// <summary>
        /// 获取Redis缓存的用户信息
        /// </summary>
        /// <param name="key">用户token</param>
        /// <returns>返回用户信息</returns>
        public TUserInfoModel GetUserInfo(string key) => Client.Get<TUserInfoModel>(key);

        /// <summary>
        /// 将用户信息保存在Redis缓存
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="key">用户token</param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool SetUserInfo(TUserInfoModel userInfo, string key) => Client.Set(key, userInfo);

        /// <summary>
        /// 删除用户缓存信息
        /// </summary>
        /// <param name="key">用户token</param>
        /// <returns>成功返回true,失败返回false</returns>
        public bool RemoveUserInfo(string key) => Client.Remove(key);

        /// <summary>
        /// 是否存在用户信息
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns>存在返回true,不存在返回false</returns>
        public bool ContainsUserInfo(string key) => Client.ContainsKey(key);

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

        public bool Contains(string key) => Client.ContainsKey(key);


        public string GetValue(string key) => Client.Get<string>(key);


        public bool SetValue(string value, string key) => Client.Set(key, value);

        public Dictionary<string, object> GetAll()
        {
            return Client.GetAll<object>(Client.GetAllKeys()) as Dictionary<string, object>;
        }

        public bool RefreshCache()
        {
            throw new NotImplementedException();
        }
    }
}
