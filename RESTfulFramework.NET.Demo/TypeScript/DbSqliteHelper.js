/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
var DbSqliteHelper = (function () {
    function DbSqliteHelper() {
    }
    DbSqliteHelper.prototype.CreateApiTestInfo = function (api, name, body, requestMethod, apiUrl, callbackFunction) {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetDataInfoUrl(encodeURIComponent(JSON.stringify({ name: name, api: api, body: body, requestMethod: requestMethod, url: apiUrl })), "AddApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });
    };
    DbSqliteHelper.prototype.GetApiTestInfo = function (api) {
        var str = localStorage.getItem(api);
        return str;
    };
    DbSqliteHelper.prototype.GetAllTestInfo = function (callbackFunction) {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetDataInfoUrl(JSON.stringify({}), "GetApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });
    };
    return DbSqliteHelper;
}());
