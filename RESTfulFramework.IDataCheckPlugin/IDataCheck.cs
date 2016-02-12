namespace RESTfulFramework.IDataCheckPlugin
{
    public interface IDataCheck
    {
        bool CheckSign(object body, string token, string api, string remark, string sign, string timestamp);
    }
}
