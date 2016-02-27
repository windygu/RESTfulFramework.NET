namespace RESTfulFramework.NET.ComponentModel
{
    public interface IConfigManager<T>
       where T : class
    {
        bool SetValue(string key, T value);
        T GetValue(string key);
    }
}
