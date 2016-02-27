namespace RESTfulFramework.NET.ComponentModel
{
    public interface IUserCache<T>
    {
        /// <summary>
        /// 获取Redis缓存的用户信息
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns>返回用户信息</returns>
        T GetUserInfo(string token);

        /// <summary>
        /// 将用户信息保存在Redis缓存
        /// </summary>
        /// <param name="userInfo">用户信息</param>
        /// <param name="token">用户token</param>
        /// <returns>成功返回true,失败返回false</returns>
        bool SetUserInfo(T userInfo, string token);

        /// <summary>
        /// 删除用户缓存信息
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns>成功返回true,失败返回false</returns>
        bool RemoveUserInfo(string token);

        /// <summary>
        /// 是否存在用户信息
        /// </summary>
        /// <param name="token">用户token</param>
        /// <returns>存在返回true,不存在返回false</returns>
        bool ContainsUserInfo(string token);
    }
}
