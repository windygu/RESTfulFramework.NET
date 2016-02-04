namespace RESTfulFramework.IJsonSerializerPlugin
{
    public interface IJsonSerialzer
    {
        T DeserializeObject<T>(string json);
        string SerializeObject<T>(T obj);
    }
}
