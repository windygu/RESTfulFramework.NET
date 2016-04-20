using System.Collections.Generic;

namespace RESTfulFramework.NET.Common
{
    public class ConfigInfo
    {
        static ConfigInfo()
        {
            try
            {
                var configManager = UnitsFactory.ConfigManager;
                AccountSecretKey = configManager.GetValue("account_secret_key").value;
                SmsAccount = configManager.GetValue("sms_account").value;
                SmsPassword = configManager.GetValue("sms_password").value;
                SmsCodeContent = configManager.GetValue("sms_code_content").value;
                RedisAddress = configManager.GetValue("redis_address").value;
                RedisPort = configManager.GetValue("redis_port").value;
            }
            catch { }
        }
        public static Dictionary<string, string> SmsCodeDictionary { get; set; }
        /// <summary>
        /// 短信帐号
        /// </summary>
        public static string SmsAccount { get; set; }
        /// <summary>
        /// 短信密码
        /// </summary>
        public static string SmsPassword { get; set; }
        /// <summary>
        /// 短信验证码内容
        /// </summary>

        public static string SmsCodeContent { get; set; }
        /// <summary>
        /// 系统密钥
        /// </summary>

        public static string AccountSecretKey { get; set; }
        /// <summary>
        /// redis地址
        /// </summary>

        public static string RedisAddress { get; set; }

        /// <summary>
        /// redis端口
        /// </summary>
        public static string RedisPort { get; set; }

    }
}
