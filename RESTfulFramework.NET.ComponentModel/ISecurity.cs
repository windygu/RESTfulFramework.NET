namespace RESTfulFramework.NET.ComponentModel
{
    /// <summary>
    /// 用于安全必须的组件
    /// </summary>
    public interface ISecurity<TRequestModel>
    {
        bool SecurityCheck( TRequestModel requestModel);
    }

}
