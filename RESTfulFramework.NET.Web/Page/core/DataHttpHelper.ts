/// <reference path="jquery.d.ts" />
class DataHttpHelper extends HttpHelper {

    /**
     * POST数据到服务器
     * @param body json实体
     * @param api 通用接口的api名称
     * @param token 用户token
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    public PostJson(body: string, api: string, token: string, result: any, error: any): void {
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
    }

    /**
     * GET数据到服务器
     * @param body json实体
     * @param api 通用接口的api名称
     * @param token 用户token
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    public GetJson(body: string, api: string, token: string, result: any, error: any): void {
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
    }

    /**
     * 推送数据到服务器 对PostJson的进一步封装，此时api名称为push，token取自localStorage.token
     * @param body json实体
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    public PushJson(body: string, result: any, error: any): void {
        var token = localStorage.getItem("token");
        this.PostJson(body, "push", token, result, error);
    }

    /**
     * 获取数据 对PostJson的进一步封装，此时api名称为fetch，token取自localStorage.token
     * @param body json实体
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    public FetchJson(body: string, result: any, error: any): void {
        var token = localStorage.getItem("token");
        this.PostJson(body, "fetch", token, result, error);
    }

    /**
     * 移除数据 对PostJson的进一步封装，此时api名称为remove，token取自localStorage.token
     * @param body json实体
     * @param result 用于接收结果的回调
     * @param error 请求过程中产生的错误回调
     */
    public RemoveData(body: string, result: any, error: any): void {
        var token = localStorage.getItem("token");
        this.PostJson(body, "remove", token, result, error);
    }


} 