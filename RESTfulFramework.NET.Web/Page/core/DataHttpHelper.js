var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="jquery.d.ts" />
var DataHttpHelper = (function (_super) {
    __extends(DataHttpHelper, _super);
    function DataHttpHelper() {
        _super.apply(this, arguments);
    }
    /**
     * POST数据到服务器
     * @param body json实体
     * @param api 通用接口的api名称
     * @param token 用户token
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    DataHttpHelper.prototype.PostJson = function (body, api, token, result, error) {
        var err = error;
        var url = this.BulidDataServicePostUrl(token, api);
        $.post(url, body, function (data, textStatus, jqXHR) {
            if (textStatus == "success") {
                result(data);
            }
            else {
                error("请求出现异常，异常信息:" + textStatus);
            }
        }, "json")
            .error(function (xhr, textStatus, errorThrown) {
            err("请求出现异常，异常信息:" + textStatus);
        });
    };
    /**
     * GET数据到服务器
     * @param body json实体
     * @param api 通用接口的api名称
     * @param token 用户token
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    DataHttpHelper.prototype.GetJson = function (body, api, token, result, error) {
        var err = error;
        var url = this.BulidDataServiceGetUrl(body, token, api);
        $.getJSON(url, function (data, textStatus, jqXHR) {
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
     * 推送数据到服务器 对PostJson的进一步封装，此时api名称为push，token取自localStorage.token
     * @param body json实体
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    DataHttpHelper.prototype.PushJson = function (body, result, error) {
        var token = localStorage.getItem("token");
        this.PostJson(body, "push", token, result, error);
    };
    /**
     * 获取数据 对PostJson的进一步封装，此时api名称为fetch，token取自localStorage.token
     * @param body json实体
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    DataHttpHelper.prototype.FetchJson = function (body, result, error) {
        var token = localStorage.getItem("token");
        this.PostJson(body, "fetch", token, result, error);
    };
    /**
     * 移除数据 对PostJson的进一步封装，此时api名称为remove，token取自localStorage.token
     * @param body json实体
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    DataHttpHelper.prototype.RemoveData = function (body, result, error) {
        var token = localStorage.getItem("token");
        this.PostJson(body, "remove", token, result, error);
    };
    return DataHttpHelper;
}(HttpHelper));
//# sourceMappingURL=DataHttpHelper.js.map