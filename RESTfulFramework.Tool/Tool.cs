using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace RESTfulFramework.Common
{
    public static class Tool
    {
        public static string GetMd5(string str, Encoding encode)
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
