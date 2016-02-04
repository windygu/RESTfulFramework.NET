using System.ComponentModel.Composition;
using System.Security.Cryptography;
using System.Text;

namespace RESTfulFramework.DataCheckDemo
{
    [Export(typeof(IDataCheckPlugin.IDataCheck))]
    public class DataCheckDemo : IDataCheckPlugin.IDataCheck
    {
        public bool CheckSign(object body, string token, string protocol, string sign, string timestamp)
        {
            /*您需要自定义签名的计算规则，此处仅供参考*/
            string systemSecretKey = "88888888";
            //仅用于调试
            if (sign == "ignor") return true;
            //校验签名 (token+ protocol + timestamp + 密钥)的MD5
            return sign == GetMd5(token + protocol + timestamp + systemSecretKey, Encoding.UTF8);
        }
        public static string GetMd5(string str, System.Text.Encoding encode)
        {

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(encode.GetBytes(str));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            return sb.ToString().ToUpper();
        }
    }
}
