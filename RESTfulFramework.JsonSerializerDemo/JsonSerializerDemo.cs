using System.ComponentModel.Composition;

namespace RESTfulFramework.JsonSerializerDemo
{
    [Export(typeof(IJsonSerializerPlugin.IJsonSerialzer))]
    public class JsonSerializerDemo : IJsonSerializerPlugin.IJsonSerialzer
    {
        public T DeserializeObject<T>(string json)
        {
            return Json2KeyValue.JsonConvert.DeserializeObject<T>(json);
        }

        public string SerializeObject<T>(T obj)
        {
            return Json2KeyValue.JsonConvert.SerializeObject(obj);
        }
    }
}
