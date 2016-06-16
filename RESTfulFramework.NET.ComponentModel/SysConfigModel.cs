namespace RESTfulFramework.NET.ComponentModel
{
    public class ConfigModel : IConfigModel
    {
        public int id { get; set; }
        public string key { get; set; }
        public string value { get; set; }
        public string remark { get; set; }
    }

    public interface IConfigModel
    {
        int id { get; set; }
        string key { get; set; }
        string value { get; set; }
        string remark { get; set; }
    }
}
