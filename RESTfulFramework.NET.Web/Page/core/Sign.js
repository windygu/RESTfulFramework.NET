/// <reference path="jquery.d.ts" />
var Sign = (function () {
    function Sign() {
    }
    /**
     * 生成登陆接口的签名
     * @param userName 用户名
     * @param password 密码
     * @param timestamp 时间戳
     * @param scretKey 密钥
     */
    Sign.prototype.GetUserSign = function (userName, password, timestamp, scretKey) {
        var str = userName + password + timestamp + scretKey;
        var sign = $.md5(str).toUpperCase();
        return sign;
    };
    /**
     * 生成数据接口的签名
     * @param token 用户token
     * @param api 通用接口的api名称
     * @param timestamp 时间戳
     * @param scretKey 密钥
     */
    Sign.prototype.GetDataSign = function (token, api, timestamp, scretKey) {
        var str = token + api + timestamp + scretKey;
        var sign = $.md5(str).toUpperCase();
        return sign;
    };
    return Sign;
}());
//# sourceMappingURL=Sign.js.map