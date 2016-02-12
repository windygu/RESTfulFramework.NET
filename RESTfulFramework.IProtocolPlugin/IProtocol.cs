using RESTfulFramework.IProtocolPlugin.Model;

namespace RESTfulFramework.IProtocolPlugin
{
    public interface IProtocol
    {
        Result<object> SetupProtocol(object body, string api, string type, object user = null);
    }
}
