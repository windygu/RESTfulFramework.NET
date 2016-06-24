/**
 * 该类主要用于生成URL地址
 */
class HttpHelper extends ConfigInfo {

    apiAlis: string = "api";
    secretKey: string = ConfigInfo.SecretKey;
    baseDataServiceUrl: string = ConfigInfo.BaseDataServiceUrl;
    baseUserServiceUrl: string = ConfigInfo.BaseUserServiceUrl;
    /**
     * 生成指定api的GET方式Url地址
     * @param body json实体
     * @param token 用户token
     * @param api 通用接口的api名称
     */
    public BulidDataServiceGetUrl(body: string, token: string, api: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetDataSign(token, api, timestamp, this.secretKey);
        var resultUrl = this.baseDataServiceUrl + "/get?body=" + encodeURI(JSON.stringify( body)) + "&token=" + token + "&" + this.apiAlis + "=" + api + "&timestamp=" + timestamp + "&sign=" + signString;
        return resultUrl;
    }

    /**
      * 生成指定api的POST方式Url地址
      * @param token 用户token
      * @param api 通用接口的api名称
      */
    public BulidDataServicePostUrl(token: string, api: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetDataSign(token, api, timestamp, this.secretKey);
        var resultUrl = this.baseDataServiceUrl + "/post?token=" + token + "&" + this.apiAlis + "=" + api + "&timestamp=" + timestamp + "&sign=" + signString;
        return resultUrl;
    }
    /**
      * 生成指定资讯api的POST方式Url地址，无须token
      * @param body json实体
      * @param api 通用接口的api名称
      */
    public BulidGetDataInfoUrl(body: string, api: string): string {
        var resultUrl = this.baseDataServiceUrl + "/getinfo?body=" + encodeURI(body) + "&api=" + api;
        return resultUrl;
    }

    /**
     * 如果所使用的一些接口有提供接口文档，则此方法是可用的。该方法生成取接口文档的URL地址。
     * @param body  json实体
     * @param api 通用接口的api名称
     */
    public BulidGetApiListDataInfoUrl(body: string, api: string): string {
        var resultUrl = ConfigInfo.InterfaceDataServiceUrl + "/getinfo?body=" + encodeURI(body) + "&api=" + api;
        return resultUrl;

    }
    /**
     * 生成登陆的地址
     * @param userName
     * @param password
     */
    public BulidUserServiceLoginUrl(userName: string, password: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetUserSign(userName, password, timestamp, ConfigInfo.SecretKey);
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/login?username=" + userName + "&sign=" + signString + "&timestamp=" + timestamp;
        return resultUrl;
    }

    /**
    * 生成登陆2的地址
    * @param userName 用户名
    * @param password 密码
    * @param clientId 客户端的扩展ID，需要时使用。
    */
    public BulidUserServiceLoginUrl2(userName: string, password: string, clientId: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetUserSign(userName, password, timestamp, ConfigInfo.SecretKey);
        var resultUrl = this.baseUserServiceUrl + "/login2?username=" + userName + "&sign=" + signString + "&timestamp=" + timestamp + "&clientid=" + clientId;
        return resultUrl;
    }


    /**
     * 生成退出登陆的地址
     * @param token 用户token
     */
    public BulidUserServiceLoginOutUrl(token: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = this.baseUserServiceUrl + "/loginout?token=" + token + "&timestamp=" + timestamp;
        return resultUrl;
    }


    /**
     * 生成注册的地址
     * @param userName 用户名
     * @param password 密码
     * @param smscode 短信验证码
     * @param realname 真实姓名
     */
    public BulidUserServiceRegisterUrl(userName: string, password: string, smscode: string, realname: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = this.baseUserServiceUrl + "/register?username=" + userName + "&password=" + password + "&smscode=" + smscode + "&realname=" + realname + "&timestamp=" + timestamp;
        return resultUrl;
    }

    /**
     * 生成获取用户信息的地址
     * @param token 用户token
     */
    public BulidUserServiceGetUserinfoUrl(token: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = this.baseUserServiceUrl + "/getuserinfo?token=" + token + "&timestamp=" + timestamp;
        return resultUrl;
    }

    /**
     * 生成获取短信验证码的地址
     * @param phone 手机号码
     */
    public BulidUserServiceSendSmsCodeUrl(phone: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = this.baseUserServiceUrl + "/sendsmscode?phone=" + phone + "&timestamp=" + timestamp;
        return resultUrl;
    }

    /**
    * 生成判断短信验证码是否存在的地址
    * @param code
    */
    public BulidUserServiceSmsCodeExistUrl(code: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/smscodeexist?code=" + code + "&timestamp=" + timestamp;
        return resultUrl;
    }



}