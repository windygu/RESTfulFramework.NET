var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var HttpHelper = (function (_super) {
    __extends(HttpHelper, _super);
    function HttpHelper() {
        _super.apply(this, arguments);
        this.apiAlis = "protocol";
    }
    /**
     * 生成指定api的Url地址
     * @param token
     * @param api
     * @param timestamp
     * @param scretKey
     */
    HttpHelper.prototype.BulidDataServiceGetUrl = function (body, token, api) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetDataSign(token, api, timestamp, ConfigInfo.SecretKey);
        var resultUrl = ConfigInfo.BaseDataServiceUrl + "/get?body=" + body + "&token=" + token + "&" + this.apiAlis + "=" + api + "&timestamp=" + timestamp + "&sign=" + signString;
        return resultUrl;
    };
    /**
 * 生成指定api的Url地址
 * @param token
 * @param api
 */
    HttpHelper.prototype.BulidDataServicePostUrl = function (token, api) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetDataSign(token, api, timestamp, ConfigInfo.SecretKey);
        var resultUrl = ConfigInfo.BaseDataServiceUrl + "/post?token=" + token + "&" + this.apiAlis + "=" + api + "&timestamp=" + timestamp + "&sign=" + signString;
        return resultUrl;
    };
    /**
     * 获取资讯api的Url地址
     * @param body
     * @param api
     */
    HttpHelper.prototype.BulidGetDataInfoUrl = function (body, api) {
        var resultUrl = ConfigInfo.BaseDataServiceUrl + "/getinfo?body=" + body + "&api=" + api;
        return resultUrl;
    };
    HttpHelper.prototype.BulidGetApiListDataInfoUrl = function (body, api) {
        var resultUrl = ConfigInfo.InterfaceDataServiceUrl + "/getinfo?body=" + body + "&api=" + api;
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
    * @param userName
    * @param password
    * @param clientId
    */
    HttpHelper.prototype.BulidUserServiceLoginUrl2 = function (userName, password, clientId) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetUserSign(userName, password, timestamp, ConfigInfo.SecretKey);
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/login2?username=" + userName + "&sign=" + signString + "&timestamp=" + timestamp + "&clientid=" + clientId;
        return resultUrl;
    };
    /**
    * 生成退出登陆的地址
    */
    HttpHelper.prototype.BulidUserServiceLoginOutUrl = function (token) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/loginout?token=" + token + "&timestamp=" + timestamp;
        return resultUrl;
    };
    /**
    * 生成注册的地址
    */
    HttpHelper.prototype.BulidUserServiceRegisterUrl = function (userName, password, smscode, realname) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/register?username=" + userName + "&password=" + password + "&smscode=" + smscode + "&realname=" + realname + "&timestamp=" + timestamp;
        return resultUrl;
    };
    /**
     * 生成获取用户信息的地址
     * @param token
     */
    HttpHelper.prototype.BulidUserServiceGetUserinfoUrl = function (token) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/getuserinfo?token=" + token + "&timestamp=" + timestamp;
        return resultUrl;
    };
    /**
     * 生成获取短信验证码的地址
     * @param phone
     */
    HttpHelper.prototype.BulidUserServiceSendSmsCodeUrl = function (phone) {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/sendsmscode?phone=" + phone + "&timestamp=" + timestamp;
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