using RESTfulFramework.NET.ComponentModel;

using System;

namespace RESTfulFramework.NET.Security
{
    public class UserSecurity<TUserCache, TUserInfoModel> : ISecurity<RequestModel<BaseUserInfo>>
        where TUserCache : IUserCache<TUserInfoModel>, new()
        where TUserInfoModel : BaseUserInfo, new()
    {
        private TUserCache UserCache { get; set; }
        public UserSecurity()
        {
            UserCache = new TUserCache();
        }
        public Tuple<bool, string, int> SecurityCheck(RequestModel<BaseUserInfo> requestModel)
        {
            try
            {
                var userInfo = UserCache?.GetUserInfo(requestModel.Token);
                if (userInfo == null)
                {
                    // return false;
                    return new Tuple<bool, string, int>(false, "token无效或已过期。", Code.TokenError);
                }
                requestModel.UserInfo = userInfo;
                return new Tuple<bool, string, int>(true, "token正确。", Code.Sucess);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string, int>(false, $"token异常。{ex.Message}", Code.TokenError);
                //throw new System.Exception($"查询不到指定的Token数据。{ex.Message}");
            }
        }
    }
}
