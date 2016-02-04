namespace RESTfulFramework.IProtocolPlugin.Model
{
    public class CodeEnum
    {
        /*Code大于0为成功，小于等于0为失败。
         *错误代码如下
         *1:成功
         *-1:密码无效
         *-2:用户名无效
         *-3:帐户异常
         *-4:签名错误
         *-5:服务器未响应
         *-6:验证码错误
         *-7:帐户被限制
         *-8:帐户已存在
         *-9:帐户正在使用中
         *-10:Token无效
         *-11:发送短信验证码失败
         *-12:帐号异常    
        */


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

        public static int NoAllow { get; } = -13;

        /// <summary>
        /// 签到或签退失败
        /// </summary>
        public static int SignInOutError { get; } = -14;
    }

  
}
