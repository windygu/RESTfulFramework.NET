/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
class DbSqliteHelper {

    CreateApiTestInfo(api: string, name: string, body: string, requestMethod: string, apiUrl: string, callbackFunction: any): void {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetApiListDataInfoUrl(encodeURIComponent(JSON.stringify({ name: name, api: api, body: body, requestMethod: requestMethod, url: apiUrl })), "AddApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });

    }

    UpdateApiTestInfo(id:string,api: string, name: string, body: string, requestMethod: string, apiUrl: string, callbackFunction: any): void {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetApiListDataInfoUrl(encodeURIComponent(JSON.stringify({id:id, name: name, api: api, body: body, requestMethod: requestMethod, url: apiUrl })), "UpdateSingleApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });

    }

    GetApiTestInfo(id: string, callbackFunction: any): void {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetApiListDataInfoUrl(encodeURIComponent(JSON.stringify({ id: id })), "GetSingleApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });
    }
    DeleteApiTestInfo(id: string, callbackFunction: any): void {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetApiListDataInfoUrl(encodeURIComponent(JSON.stringify({ id: id })), "DeleteApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });
    }
    GetAllTestInfo(callbackFunction: any): void {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetApiListDataInfoUrl(JSON.stringify({}), "GetApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });
    }
}


