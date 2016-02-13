using System.ComponentModel.Composition;
using System.Security.Cryptography;
using System.Text;

namespace RESTfulFramework.DataCheckDemo
{
    [Export(typeof(IDataCheckPlugin.IDataCheck))]
    public class DataCheckDemo : IDataCheckPlugin.IDataCheck
    {
        public bool CheckSign(object body, string token, string api, string type, string sign, string timestamp)
        {
            /*您需要自定义签名的计算规则，此处仅供参考*/
            string systemSecretKey = "88888888";
            //仅用于调试
            if (sign == "ignor") return true;
            //校验签名 (token+ api + remark + timestamp + 密钥)的MD5
            return sign == Common.Tool.GetMd5(token + api + type + timestamp + systemSecretKey, Encoding.UTF8);
        }

    }
}
