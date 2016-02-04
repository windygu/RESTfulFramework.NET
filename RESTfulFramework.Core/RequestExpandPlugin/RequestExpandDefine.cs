using System.ComponentModel.Composition;

namespace RESTfulFramework.Core.RequestExpandPlugin
{
    public class RequestExpandDefine
    {
        [Import(typeof(IExpandPlugin.IExpand))]
        public IExpandPlugin.IExpand RequestExpand { get; set; }
    }
}
