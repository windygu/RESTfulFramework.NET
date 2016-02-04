using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.IUserPlugin
{
    public interface IUser
    {
        object GetUser(string token);
    }
}
