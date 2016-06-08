using System;

namespace RESTfulFramework.NET.ComponentModel
{
    /// <summary>
    /// 用于安全必须的组件
    /// </summary>
    public interface ISecurity<TRequestModel>
    {
        Tuple<bool, string,int> SecurityCheck(TRequestModel requestModel);
    }

}
