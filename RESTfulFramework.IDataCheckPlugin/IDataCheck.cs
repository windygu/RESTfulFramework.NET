namespace RESTfulFramework.IDataCheckPlugin
{
    public interface IDataCheck
    {
        bool CheckSign(object body,string token, string protocol, string sign, string timestamp);
    }
}
