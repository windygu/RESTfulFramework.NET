using RESTfulFramework.IUserPlugin;
using System.ComponentModel.Composition;

namespace RESTfulFramework.Core.CorePlugin
{
    public  class UserDefine
    {
        [Import(typeof(IUser))]

        public   IUser UserState;
    }
}
