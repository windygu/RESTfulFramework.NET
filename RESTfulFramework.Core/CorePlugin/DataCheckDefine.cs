using RESTfulFramework.IDataCheckPlugin;
using System.ComponentModel.Composition;

namespace RESTfulFramework.Core.CorePlugin
{
    public  class DataCheckDefine
    {
        [Import(typeof(IDataCheck))]
        public    IDataCheck DataCheck { get; set; }

    }
}
