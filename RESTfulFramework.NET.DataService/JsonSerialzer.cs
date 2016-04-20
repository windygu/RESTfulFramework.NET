using System;
using RESTfulFramework.NET.ComponentModel;

namespace RESTfulFramework.NET.DataService
{
    public class JsonSerialzer : IJsonSerialzer
    {
        public T DeserializeObject<T>(string json) => Json2KeyValue.JsonConvert.DeserializeObject<T>(json);
        public string SerializeObject<T>(T obj) => Json2KeyValue.JsonConvert.SerializeObject(obj);

    }
}
