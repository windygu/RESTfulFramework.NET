/// <reference path="jquery.d.ts" />

class Sign {

    /**
     * 生成登陆接口的签名
     * @param userName 用户名
     * @param password 密码
     * @param timestamp 时间戳
     * @param scretKey 密钥
     */
    public GetUserSign(userName: string, password: string, timestamp: string, scretKey: string): string {
        var str: string = userName + password + timestamp + scretKey;
        var sign = $.md5(str).toUpperCase();
        return sign;
    }

    /**
     * 生成数据接口的签名
     * @param token 用户token
     * @param api 通用接口的api名称
     * @param timestamp 时间戳
     * @param scretKey 密钥
     */
    public GetDataSign(token: string, api: string, timestamp: string, scretKey: string): string {
        var str: string = token + api + timestamp + scretKey;
        var sign = $.md5(str).toUpperCase();
        return sign;
    }

}