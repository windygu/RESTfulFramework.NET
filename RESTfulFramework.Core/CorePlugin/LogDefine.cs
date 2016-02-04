using RESTfulFramework.ILogPlugin;
using System.ComponentModel.Composition;

namespace RESTfulFramework.Core.CorePlugin
{
    public class LogDefine
    {
        [Import(typeof(ILog))]
        public   ILog Log { get; set; }
    }
}
