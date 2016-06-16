
using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.Units
{
    public class JsonSerialzer : IJsonSerialzer
    {
        public JsonSerialzer() { }
        public T DeserializeObject<T>(string json) => Json2KeyValue.JsonConvert.DeserializeObject<T>(json);
        public string SerializeObject<T>(T obj) => Json2KeyValue.JsonConvert.SerializeObject(obj);
    }
}
