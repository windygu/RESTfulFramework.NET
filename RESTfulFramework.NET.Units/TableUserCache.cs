using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.Units
{
    public class TableUserCache<TConfigManager, TConfigModel> : IUserCache<UserInfo>
        where TConfigManager : IConfigManager<TConfigModel>, new()
        where TConfigModel : IConfigModel
    {
        private string ConnectionString { get; set; }
        public TableUserCache()
        {
            //检查缓存表是否存在
            

            //如果不存在，则自动创建



        }

        public bool Contains(string key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsUserInfo(string key)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetAll()
        {
            throw new NotImplementedException();
        }

        public UserInfo GetUserInfo(string key)
        {
            throw new NotImplementedException();
        }

        public string GetValue(string key)
        {
            throw new NotImplementedException();
        }

        public bool RefreshCache()
        {
            throw new NotImplementedException();
        }

        public bool RemoveUserInfo(string key)
        {
            throw new NotImplementedException();
        }

        public bool SetUserInfo(UserInfo userInfo, string key)
        {
            throw new NotImplementedException();
        }

        public bool SetValue(string value, string key)
        {
            throw new NotImplementedException();
        }
    }
}
