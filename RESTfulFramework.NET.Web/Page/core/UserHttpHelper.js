var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="jquery.d.ts" />
/// <reference path="sqlite.d.ts" />
var UserHttpHelper = (function (_super) {
    __extends(UserHttpHelper, _super);
    function UserHttpHelper() {
        _super.apply(this, arguments);
    }
    /**
     * 登陆
     * @param userName 用户名
     * @param password 密码
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    UserHttpHelper.prototype.Login = function (userName, password, result, error) {
        var err = error;
        var loginUrl = this.BulidUserServiceLoginUrl(userName, password);
        $.getJSON(loginUrl, function (data, textStatus, jqXHR) {
            if (textStatus == "success") {
                result(data);
            }
            else {
                error("请求出现异常，异常信息:" + textStatus);
            }
        })
            .error(function (xhr, textStatus, errorThrown) {
            err("请求出现异常，异常信息:" + textStatus);
        });
    };
    /**
     * 注册
     * @param userName 用户名
     * @param password 密码
     * @param smscode
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    UserHttpHelper.prototype.Register = function (userName, password, smscode, realname, result, error) {
        var err = error;
        if (realname == null || realname == undefined || realname == "") {
            realname = "日记本用户";
        }
        var registerUrl = this.BulidUserServiceRegisterUrl(userName, password, smscode, realname);
        $.getJSON(registerUrl, function (data, textStatus, jqXHR) {
            if (textStatus == "success") {
                result(data);
            }
            else {
                error("请求出现异常，异常信息:" + textStatus);
            }
        })
            .error(function (xhr, textStatus, errorThrown) {
            err("请求出现异常，异常信息:" + textStatus);
        });
    };
    /**
     * 发送短信验证码
     * @param phone 手机号码
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    UserHttpHelper.prototype.SendSmsCode = function (phone, result, error) {
        var err = error;
        var sendsmscodeUrl = this.BulidUserServiceSendSmsCodeUrl(phone);
        $.getJSON(sendsmscodeUrl, function (data, textStatus, jqXHR) {
            if (textStatus == "success") {
                result(data);
            }
            else {
                error("请求出现异常，异常信息:" + textStatus);
            }
        })
            .error(function (xhr, textStatus, errorThrown) {
            err("请求出现异常，异常信息:" + textStatus);
        });
    };
    /**
     * 取用户基础信息，该调用会取localStroage.token的字符串
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    UserHttpHelper.prototype.GetUserInfo = function (result, error) {
        var err = error;
        var token = localStorage.getItem("token");
        var getuserUrl = this.BulidUserServiceGetUserinfoUrl(token);
        $.getJSON(getuserUrl, function (data, textStatus, jqXHR) {
            if (textStatus == "success") {
                result(data);
            }
            else {
                error("请求出现异常，异常信息:" + textStatus);
            }
        })
            .error(function (xhr, textStatus, errorThrown) {
            err("请求出现异常，异常信息:" + textStatus);
        });
    };
    return UserHttpHelper;
}(HttpHelper));
//# sourceMappingURL=UserHttpHelper.js.map