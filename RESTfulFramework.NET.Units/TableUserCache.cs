using RESTfulFramework.NET.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.Units
{
    public class TableUserCache : IUserCache<UserInfo>
    {
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
