/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
var Sign = (function () {
    function Sign() {
    }
    /**
     * 获取登陆接口的签名
     * @param userName
     * @param password
     * @param timestamp
     * @param scretKey
     */
    Sign.prototype.GetUserSign = function (userName, password, timestamp, scretKey) {
        var str = userName + password + timestamp + scretKey;
        var sign = $.md5(str).toUpperCase();
        return sign;
    };
    /**
     * 获取数据接口的签名
     * @param token
     * @param api
     * @param timestamp
     * @param scretKey
     */
    Sign.prototype.GetDataSign = function (token, api, timestamp, scretKey) {
        var str = token + api + timestamp + scretKey;
        var sign = $.md5(str).toUpperCase();
        return sign;
    };
    return Sign;
}());
//# sourceMappingURL=Sign.js.map