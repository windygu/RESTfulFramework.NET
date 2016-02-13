namespace RESTfulFramework.IDataCheckPlugin
{
    public interface IDataCheck
    {
        bool CheckSign(object body, string token, string api, string type, string sign, string timestamp);
    }
}
