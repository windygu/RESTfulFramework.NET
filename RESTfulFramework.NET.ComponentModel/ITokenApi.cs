namespace RESTfulFramework.NET.ComponentModel
{
    /// <summary>
    /// 用于分配 业务逻辑实现 分配器
    /// </summary>
    public interface ITokenApi<TRequestModel, TResponseModel>
    {
        TResponseModel RunApi(TRequestModel source);
    } 
}
