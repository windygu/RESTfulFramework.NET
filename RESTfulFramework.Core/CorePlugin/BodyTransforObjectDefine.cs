using RESTfulFramework.IBodyTransforObjectPlugin;
using System.ComponentModel.Composition;

namespace RESTfulFramework.Core.CorePlugin
{
    public  class BodyTransforObjectDefine
    {
        [Import(typeof(IBodyTransforObject))]
        public   IBodyTransforObject BodyTransforObject { get; set; }
    }
}
