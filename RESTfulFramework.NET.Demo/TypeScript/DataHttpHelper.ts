/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
class DataHttpHelper extends HttpHelper {

    PostJson(body: string, api: string, token: string, callBackFunction: any): void {
        var url = this.BulidDataServicePostUrl(token, api);

        $.post(url, body, function (data, textStatus, jqXHR) {
            callBackFunction(data, textStatus, jqXHR);
        }, "json");
    }

    GetJson(body: string, api: string, token: string, callBackFunction: any): void {
        var url = this.BulidDataServiceGetUrl(body, token, api);
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callBackFunction(data, textStatus, jqXHR);    
        });
    }

} 