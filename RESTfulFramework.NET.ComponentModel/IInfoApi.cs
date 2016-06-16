//namespace RESTfulFramework.NET.ComponentModel
//{
//    /// <summary>
//    /// 用于分配 业务逻辑实现 分配器
//    /// </summary>
//    public interface IInfoApi<TRequestModel, TResponseModel, TUserInfoModel>
//         where TUserInfoModel : IBaseUserInfo, new()
//    {
//        TResponseModel RunApi(TRequestModel request);

//        ApiContext<TUserInfoModel> Context { get; set; }
//    }

//    public abstract class BaseInfoApi<TRequestModel, TResponseModel, TUserInfoModel>
//       : IInfoApi<TRequestModel, TResponseModel, TUserInfoModel>
//       where TUserInfoModel : IBaseUserInfo, new()
//    {
//        public abstract TResponseModel RunApi(TRequestModel request);

//        public ApiContext<TUserInfoModel> Context { get; set; }
//    }
//}
