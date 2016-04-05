/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
class DbSqliteHelper {

    CreateApiTestInfo(api: string, name: string, body: string, requestMethod: string, apiUrl: string, callbackFunction: any): void {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetDataInfoUrl(encodeURIComponent(JSON.stringify({ name: name, api: api, body: body, requestMethod: requestMethod, url: apiUrl })), "AddApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });

    }

    GetApiTestInfo(api: string): any {
        var str = localStorage.getItem(api);
        return str;
    }

    GetAllTestInfo(callbackFunction: any): void {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetDataInfoUrl(JSON.stringify({}), "GetApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });
    }
}


