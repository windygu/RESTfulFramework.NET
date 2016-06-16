using System.Collections.Generic;

namespace RESTfulFramework.NET.ComponentModel
{
    /// <summary>
    /// 用户缓存
    /// </summary>
    public interface IUserCache<TUserInfoModel>
        where TUserInfoModel : IBaseUserInfo
    {
        /// <summary>
        /// 获取Redis缓存的用户信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>返回用户信息</returns>
        TUserInfoModel GetUserInfo(string key);

        /// <summary>
        /// 将用户信息保存在Redis缓存
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="key">key</param>
        /// <returns>成功返回true,失败返回false</returns>
        bool SetUserInfo(TUserInfoModel userInfo, string key);

        /// <summary>
        /// 删除用户缓存信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>成功返回true,失败返回false</returns>
        bool RemoveUserInfo(string key);

        /// <summary>
        /// 是否存在用户信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>存在返回true,不存在返回false</returns>
        bool ContainsUserInfo(string key);

        /// <summary>
        /// 是否存在某键信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        bool Contains(string key);


        /// <summary>
        /// 取缓存值
        /// </summary>
        /// <param name="key">key</param>
        string GetValue(string key);

        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool SetValue(string value, string key);

        /// <summary>
        /// 获取所有键值对
        /// </summary>
        Dictionary<string, object> GetAll();

        /// <summary>
        /// 刷新缓存
        /// </summary>
        bool RefreshCache();
    }
}
