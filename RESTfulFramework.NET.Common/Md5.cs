using System.Security.Cryptography;
using System.Text;

namespace RESTfulFramework.NET.Common
{
    public class Md5
    {
        /// <summary>
        /// MD5　32位加密
        /// </summary>
        public static string GetMd5(string str, Encoding encode)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(encode.GetBytes(str));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            return sb.ToString().ToUpper();
        }
    }
}
