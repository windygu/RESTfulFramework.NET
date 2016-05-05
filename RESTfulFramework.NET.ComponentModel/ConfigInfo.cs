using System.Collections.Generic;

namespace RESTfulFramework.NET.ComponentModel
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public class ConfigInfo
    {
        /// <summary>
        /// 用于存储短信验证码集合
        /// </summary>
        //public Dictionary<string, string> SmsCodeDictionary { get; set; }

        /// <summary>
        /// 短信帐号
        /// </summary>
        public string SmsAccount { get; set; }

        /// <summary>
        /// 短信密码
        /// </summary>
        public string SmsPassword { get; set; }

        /// <summary>
        /// 短信验证码内容
        /// </summary>
        public string SmsCodeContent { get; set; }

        /// <summary>
        /// 系统密钥
        /// </summary>
        public string AccountSecretKey { get; set; }

        /// <summary>
        /// redis地址
        /// </summary>
        public string RedisAddress { get; set; }

        /// <summary>
        /// redis端口
        /// </summary>
        public string RedisPort { get; set; }

    }
}
