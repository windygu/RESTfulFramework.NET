using RESTfulFramework.IProtocolPlugin;
using System.ComponentModel.Composition;

namespace RESTfulFramework.Core.ProtocolPlugin
{
    public class ProtocolEx
    {
        [Import(typeof(IProtocol))]
        public IProtocol Protocol { get; set; }
    }
}
