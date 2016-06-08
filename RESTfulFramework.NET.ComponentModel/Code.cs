namespace RESTfulFramework.NET.ComponentModel
{
    /// <summary>
    /// 用于返回结果的Code
    /// </summary>
    public struct Code
    {
        /// <summary>
        /// 成功
        /// </summary>
        public static int Sucess { get; } = 1;

        /// <summary>
        /// 密码无效
        /// </summary>
        public static int PasswordError { get; } = -1;

        /// <summary>
        /// 用户名无效
        /// </summary>
        public static int UsernameError { get; } = -2;

        /// <summary>
        /// 帐户异常
        /// </summary>
        public static int AccountException { get; } = -3;

        /// <summary>
        /// 签名错误
        /// </summary>
        public static int SignErron { get; } = -4;

        /// <summary>
        /// 服务器未响应
        /// </summary>
        public static int ServerNotResponse { get; } = -5;

        /// <summary>
        /// 验证码错误
        /// </summary>
        public static int ValCodeError { get; } = -6;

        /// <summary>
        /// 帐户被限制
        /// </summary>
        public static int AccountLimit { get; } = -7;

        /// <summary>
        /// 帐户已存在
        /// </summary>
        public static int AccountExsit { get; } = -8;

        /// <summary>
        /// 帐户正在使用中
        /// </summary>
        public static int AccountUsing { get; } = -9;

        /// <summary>
        /// Token无效
        /// </summary>
        public static int TokenError { get; } = -10;

        /// <summary>
        /// 发送短信验证码失败
        /// </summary>
        public static int SmsCodeFail { get; } = -11;

        /// <summary>
        /// 系统异常
        /// </summary>
        public static int SystemException { get; } = -12;

        /// <summary>
        /// 权限不足
        /// </summary>
        public static int NoAllow { get; } = -13;

        /// <summary>
        /// 签到或签退失败
        /// </summary>
        public static int SignInOutError { get; } = -14;

        /// <summary>
        /// 没有数据
        /// </summary>
        public static int IsNotData { get; } = -15;

        /// <summary>
        /// 操作失败
        /// </summary>
        public static int Fail { get; } = -16;

        /// <summary>
        /// 没有加入组织
        /// </summary>
        public static int IsNotJoinEnterpriseLibrary { get; } = -17;

        /// <summary>
        /// 时间戳不正确
        /// </summary>
        public static int TimestampError { get; } = -18;

        /// <summary>
        /// JSON格式无效
        /// </summary>
        public static int JsonInvalid { get; } = -19;


    }


}
