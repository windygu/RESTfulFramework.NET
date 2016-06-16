//using System;

//namespace RESTfulFramework.NET.ComponentModel
//{
//    /// <summary>
//    /// 用于分配 业务逻辑实现 分配器
//    /// </summary>
//    public interface ITokenApi<TRequestModel, TResponseModel, TUserInfoModel>
//        where TUserInfoModel : BaseUserInfo, new()
//    {
//        TResponseModel RunApi(TRequestModel source);
//        ApiContext<TUserInfoModel> Context { get; set; }
//    }

//    public abstract class BaseTokenApi<TRequestModel, TResponseModel, TUserInfoModel>
//        : ITokenApi<TRequestModel, TResponseModel, TUserInfoModel>
//        where TUserInfoModel : BaseUserInfo, new()
//    {
//        public ApiContext<TUserInfoModel> Context { get; set; }

//        public abstract TResponseModel RunApi(TRequestModel source);
//    }
//}
