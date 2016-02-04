using System.IO;
using System.Text;

namespace System
{
    public static class StreamEx
    {
        /// <summary>
        /// 将字符串转成流
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>返回流</returns>
        public static Stream ToStream(this string str)
        {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(str));
            return ms;
        }
    }
}
