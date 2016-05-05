namespace RESTfulFramework.NET.ComponentModel
{
    public interface IUserCache<T>
        where T : BaseUserInfo, new()
    {
        /// <summary>
        /// 获取Redis缓存的用户信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>返回用户信息</returns>
        T GetUserInfo(string key);

        /// <summary>
        /// 将用户信息保存在Redis缓存
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="key">key</param>
        /// <returns>成功返回true,失败返回false</returns>
        bool SetUserInfo(T userInfo, string key);

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
    }
}
