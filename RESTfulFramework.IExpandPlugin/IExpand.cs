using RESTfulFramework.IExpandPlugin.Model;

namespace RESTfulFramework.IExpandPlugin
{
    public interface IExpand
    {
        RequestData Expand(RequestData requestData);

        void SetIndex(int index);

        int GetIndex();

    }
}
