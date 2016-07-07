var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/**
 * 该类主要用于生成URL地址
 */
var HttpHelper = (function (_super) {
    __extends(HttpHelper, _super);
    function HttpHelper() {
        _super.apply(this, arguments);
        this.apiAlis = "api";
        this.secretKey = ConfigInfo.SecretKey;
        this.baseDataServiceUrl = ConfigInfo.BaseDataServiceUrl;
        this.baseUserServiceUrl = ConfigInfo.BaseUserServiceUrl;
    }
    /**
     * 生成指定api的GET方式Url地址
     * @param body json实体
     * @param token 用户token
     * @param api 通用接口的api名称
     */
    HttpHelper.prototype.BulidDataServiceGetUrl = function (body, token, api) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetDataSign(token, api, timestamp, this.secretKey);
        var resultUrl = this.baseDataServiceUrl + "/get?body=" + encodeURI(JSON.stringify(body)) + "&token=" + token + "&" + this.apiAlis + "=" + api + "&timestamp=" + timestamp + "&sign=" + signString;
        return resultUrl;
    };
    /**
      * 生成指定api的POST方式Url地址
      * @param token 用户token
      * @param api 通用接口的api名称
      */
    HttpHelper.prototype.BulidDataServicePostUrl = function (token, api) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetDataSign(token, api, timestamp, this.secretKey);
        var resultUrl = this.baseDataServiceUrl + "/post?token=" + token + "&" + this.apiAlis + "=" + api + "&timestamp=" + timestamp + "&sign=" + signString;
        return resultUrl;
    };
    /**
      * 生成指定资讯api的POST方式Url地址，无须token
      * @param body json实体
      * @param api 通用接口的api名称
      */
    HttpHelper.prototype.BulidGetDataInfoUrl = function (body, api) {
        var resultUrl = this.baseDataServiceUrl + "/getinfo?body=" + encodeURI(body) + "&api=" + api;
        return resultUrl;
    };
    /**
     * 如果所使用的一些接口有提供接口文档，则此方法是可用的。该方法生成取接口文档的URL地址。
     * @param body  json实体
     * @param api 通用接口的api名称
     */
    HttpHelper.prototype.BulidGetApiListDataInfoUrl = function (body, api) {
        var resultUrl = ConfigInfo.InterfaceDataServiceUrl + "/getinfo?body=" + encodeURI(body) + "&api=" + api;
        return resultUrl;
    };
    /**
     * 生成登陆的地址
     * @param userName
     * @param password
     */
    HttpHelper.prototype.BulidUserServiceLoginUrl = function (userName, password) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetUserSign(userName, password, timestamp, ConfigInfo.SecretKey);
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/login?username=" + userName + "&sign=" + signString + "&timestamp=" + timestamp;
        return resultUrl;
    };
    /**
    * 生成登陆2的地址
    * @param userName 用户名
    * @param password 密码
    * @param clientId 客户端的扩展ID，需要时使用。
    */
    HttpHelper.prototype.BulidUserServiceLoginUrl2 = function (userName, password, clientId) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetUserSign(userName, password, timestamp, ConfigInfo.SecretKey);
        var resultUrl = this.baseUserServiceUrl + "/login2?username=" + userName + "&sign=" + signString + "&timestamp=" + timestamp + "&clientid=" + clientId;
        return resultUrl;
    };
    /**
     * 生成退出登陆的地址
     * @param token 用户token
     */
    HttpHelper.prototype.BulidUserServiceLoginOutUrl = function (token) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = this.baseUserServiceUrl + "/loginout?token=" + token + "&timestamp=" + timestamp;
        return resultUrl;
    };
    /**
     * 生成注册的地址
     * @param userName 用户名
     * @param password 密码
     * @param smscode 短信验证码
     * @param realname 真实姓名
     */
    HttpHelper.prototype.BulidUserServiceRegisterUrl = function (userName, password, smscode, realname) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = this.baseUserServiceUrl + "/register?username=" + userName + "&password=" + password + "&smscode=" + smscode + "&realname=" + realname + "&timestamp=" + timestamp;
        return resultUrl;
    };
    /**
     * 生成获取用户信息的地址
     * @param token 用户token
     */
    HttpHelper.prototype.BulidUserServiceGetUserinfoUrl = function (token) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = this.baseUserServiceUrl + "/getuserinfo?token=" + token + "&timestamp=" + timestamp;
        return resultUrl;
    };
    /**
     * 生成获取短信验证码的地址
     * @param phone 手机号码
     */
    HttpHelper.prototype.BulidUserServiceSendSmsCodeUrl = function (phone) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = this.baseUserServiceUrl + "/sendsmscode?phone=" + phone + "&timestamp=" + timestamp;
        return resultUrl;
    };
    /**
    * 生成判断短信验证码是否存在的地址
    * @param code
    */
    HttpHelper.prototype.BulidUserServiceSmsCodeExistUrl = function (code) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/smscodeexist?code=" + code + "&timestamp=" + timestamp;
        return resultUrl;
    };
    return HttpHelper;
}(ConfigInfo));
//# sourceMappingURL=HttpHelper.js.map