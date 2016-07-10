using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESTfulFramework.NET.ComponentModel;
using RESTfulFramework.NET.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.Units.Tests
{
    [TestClass()]
    public class TableUserCacheTests
    {
        [TestMethod()]
        public void StaticSetValueTest()
        {

            var tableUserCache = new TableUserCache<ConfigManager, ConfigModel>();
            var result = tableUserCache.SetValue("234568", "13088855548");
            //TableUserCache<ConfigManager, ConfigModel>.StaticSetValue("值6", "键3");
            //TableUserCache<ConfigManager, ConfigModel>.StaticSetValue("值5234", "键5");
            //TableUserCache<ConfigManager, ConfigModel>.StaticSetValue("值623", "键2");

            if (!result) Assert.Fail();
        }

        [TestMethod()]
        public void RemoveUserInfoTest()
        {
            var tableUserCache = new TableUserCache<ConfigManager, ConfigModel>();
            var result = tableUserCache.RemoveUserInfo("键3");
            if (!result) Assert.Fail();
        }

        [TestMethod()]
        public void ContainsTest()
        {
            var tableUserCache = new TableUserCache<ConfigManager, ConfigModel>();
            var result = tableUserCache.Contains("键3");
            if (!result) Assert.Fail();
        }
    }
}