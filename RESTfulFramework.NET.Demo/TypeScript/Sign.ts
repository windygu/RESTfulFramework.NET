/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />

class Sign {

    /**
     * 获取登陆接口的签名
     * @param userName
     * @param password
     * @param timestamp
     * @param scretKey
     */
    public GetUserSign(userName: string, password: string, timestamp: string, scretKey: string): string {
        var str: string = userName + password + timestamp + scretKey;
        var sign = $.md5(str).toUpperCase();
        return sign;
    }

    /**
     * 获取数据接口的签名
     * @param token
     * @param api
     * @param timestamp
     * @param scretKey
     */
    public GetDataSign(token: string, api: string, timestamp: string, scretKey: string): string {
        var str: string = token + api + timestamp + scretKey;
        var sign = $.md5(str).toUpperCase();
        return sign;
    }

}