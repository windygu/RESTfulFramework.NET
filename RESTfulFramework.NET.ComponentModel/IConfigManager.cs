using System.Collections.Generic;

namespace RESTfulFramework.NET.ComponentModel
{
    public interface IConfigManager<TConfigModel>
        where TConfigModel: IConfigModel
    {
        string GetConnectionString();
        bool SetValue(string key, TConfigModel value);
        TConfigModel GetValue(string key);
    }
}
