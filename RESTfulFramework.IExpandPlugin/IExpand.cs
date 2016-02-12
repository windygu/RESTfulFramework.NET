using RESTfulFramework.IExpandPlugin.Model;

namespace RESTfulFramework.IExpandPlugin
{
    public interface IExpand
    {
        RequestData Expand(RequestData requestData);

        int GetIndex();

    }
}
