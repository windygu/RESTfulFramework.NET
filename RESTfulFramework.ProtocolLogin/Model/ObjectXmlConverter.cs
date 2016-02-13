using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace RESTfulFramework.ProtocolAccount.Model
{
    public static class ObjectXmlConverter
    {
        public static T GetObjectByXmlString<T>(this string xmlStr)
            where T : class
        {
            try
            {
                var xmlPath = Path.GetTempFileName();
                var streamWrite = new StreamWriter(xmlPath, false, Encoding.GetEncoding("gb2312"));
                streamWrite.Write(xmlStr);
                streamWrite.Close();
                using (var stream = new FileStream(xmlPath, FileMode.Open, FileAccess.Read))
                {
                    //得到被序列化的类型
                    var sz = new XmlSerializer(typeof(T));

                    //开始序列化
                    var t = (T)sz.Deserialize(stream);
                    return t;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static T GetObject<T>(this string xmlPath)
         where T : class
        {
            try
            {
                using (var stream = new FileStream(xmlPath, FileMode.Open, FileAccess.Read))
                {
                    //得到被序列化的类型
                    var sz = new XmlSerializer(typeof(T));

                    //开始序列化
                    var t = (T)sz.Deserialize(stream);
                    return t;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }


        public static string SetObjectToString<T>(this T t)
        {
            try
            {
                var path = Path.GetTempFileName();
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    //得到被序列化的类型
                    var type = t.GetType();
                    var sz = new XmlSerializer(type);
                    //开始序列化
                    sz.Serialize(stream, t);
                }

                using (var stream = new StreamReader(path))
                {
                    var str = stream.ReadToEnd();
                    return str;
                }
            }
            catch (Exception)
            {
                return null;

            }
        }

    }
}
