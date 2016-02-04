using RESTfulFramework.IJsonSerializerPlugin;
using System.ComponentModel.Composition;

namespace RESTfulFramework.Core.CorePlugin
{
    public class JsonSerialzerDefine
    {
        [Import(typeof(IJsonSerialzer))]
        public IJsonSerialzer JsonSerialzer { get; set; }
    }
}
