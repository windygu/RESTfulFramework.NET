using RESTfulFramework.NET.ComponentModel;

using System;

namespace RESTfulFramework.NET.Security
{
    public class UserSecurity<TUserCache, TUserInfoModel> : ISecurity<RequestModel<TUserInfoModel>>
        where TUserCache : IUserCache<TUserInfoModel>, new()
        where TUserInfoModel : IBaseUserInfo, new()
    {
        private TUserCache UserCache { get; set; }
        public UserSecurity()
        {
            UserCache = new TUserCache();
        }
        /// <summary>
        /// token校验
        /// </summary>
        /// <param name="requestModel">请求的模型</param>
        /// <returns></returns>
        public Tuple<bool, string, int> SecurityCheck(RequestModel<TUserInfoModel> requestModel)
        {
            try
            {
                var userInfo = UserCache.GetUserInfo(requestModel.Token);
                if (userInfo == null)
                {
                    return new Tuple<bool, string, int>(false, "token无效或已过期。", Code.TokenError);
                }
                requestModel.UserInfo = userInfo;
                return new Tuple<bool, string, int>(true, "token正确。", Code.Sucess);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, int>(false, $"token异常。{ex.Message}", Code.TokenError);
            }
        }
    }
}
