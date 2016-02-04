using RESTfulFramework.IProtocolPlugin.Model;

namespace RESTfulFramework.IProtocolPlugin
{
    public interface IProtocol
    {
        Result<object> SetupProtocol(object body, string protocol, object user = null);
    }
}
