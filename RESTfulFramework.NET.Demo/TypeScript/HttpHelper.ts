class HttpHelper extends ConfigInfo {

      apiAlis: string = "protocol";

    /**
     * 生成指定api的Url地址
     * @param token
     * @param api
     * @param timestamp
     * @param scretKey
     */
    public BulidDataServiceGetUrl(body: string, token: string, api: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetDataSign(token, api, timestamp, ConfigInfo.SecretKey);
        var resultUrl = ConfigInfo.BaseDataServiceUrl + "/get?body=" + body + "&token=" + token + "&" + this.apiAlis + "=" + api + "&timestamp=" + timestamp + "&sign=" + signString;
        return resultUrl;
    }
    /**
 * 生成指定api的Url地址
 * @param token
 * @param api
 */
    public BulidDataServicePostUrl(token: string, api: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetDataSign(token, api, timestamp, ConfigInfo.SecretKey);
        var resultUrl = ConfigInfo.BaseDataServiceUrl + "/post?token=" + token + "&" + this.apiAlis +"=" + api + "&timestamp=" + timestamp + "&sign=" + signString;
        return resultUrl;
    }
    /**
     * 获取资讯api的Url地址
     * @param body
     * @param api
     */
    public BulidGetDataInfoUrl(body: string, api: string): string {
        var resultUrl = ConfigInfo.BaseDataServiceUrl + "/getinfo?body=" + body + "&api=" + api;
        return resultUrl;
    }

    public BulidGetApiListDataInfoUrl(body: string, api: string): string {

        var resultUrl = ConfigInfo.InterfaceDataServiceUrl + "/getinfo?body=" + body + "&api=" + api;
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
    * @param userName
    * @param password
    * @param clientId
    */
    public BulidUserServiceLoginUrl2(userName: string, password: string, clientId: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var signString = sign.GetUserSign(userName, password, timestamp, ConfigInfo.SecretKey);
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/login2?username=" + userName + "&sign=" + signString + "&timestamp=" + timestamp + "&clientid=" + clientId;
        return resultUrl;
    }

    /**
    * 生成退出登陆的地址
    */
    public BulidUserServiceLoginOutUrl(token: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/loginout?token=" + token + "&timestamp=" + timestamp;
        return resultUrl;
    }

    /**
    * 生成注册的地址
    */
    public BulidUserServiceRegisterUrl(userName: string, password: string, smscode: string, realname: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/register?username=" + userName + "&password=" + password + "&smscode=" + smscode + "&realname=" + realname + "&timestamp=" + timestamp;
        return resultUrl;
    }

    /**
     * 生成获取用户信息的地址
     * @param token
     */
    public BulidUserServiceGetUserinfoUrl(token: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/getuserinfo?token=" + token + "&timestamp=" + timestamp;
        return resultUrl;
    }

    /**
     * 生成获取短信验证码的地址
     * @param phone
     */
    public BulidUserServiceSendSmsCodeUrl(phone: string): string {
        var sign = new Sign();
        var timestamp = this.CurrentTimestamp();
        var resultUrl = ConfigInfo.BaseUserServiceUrl + "/sendsmscode?phone=" + phone + "&timestamp=" + timestamp;
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