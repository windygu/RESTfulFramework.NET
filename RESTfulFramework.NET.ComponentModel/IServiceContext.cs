using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RESTfulFramework.NET.ComponentModel
{
    public interface IServiceContext<TUserInfoModel>
       where TUserInfoModel : BaseUserInfo, new()
    {
        ApiContext<TUserInfoModel> Context { get; }
    }
}
