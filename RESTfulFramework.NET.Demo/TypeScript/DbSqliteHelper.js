/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
var DbSqliteHelper = (function () {
    function DbSqliteHelper() {
    }
    DbSqliteHelper.prototype.CreateApiTestInfo = function (api, name, body, requestMethod, apiUrl, callbackFunction) {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetApiListDataInfoUrl(encodeURIComponent(JSON.stringify({ name: name, api: api, body: body, requestMethod: requestMethod, url: apiUrl })), "AddApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });
    };
    DbSqliteHelper.prototype.UpdateApiTestInfo = function (id, api, name, body, requestMethod, apiUrl, callbackFunction) {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetApiListDataInfoUrl(encodeURIComponent(JSON.stringify({ id: id, name: name, api: api, body: body, requestMethod: requestMethod, url: apiUrl })), "UpdateSingleApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });
    };
    DbSqliteHelper.prototype.GetApiTestInfo = function (id, callbackFunction) {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetApiListDataInfoUrl(encodeURIComponent(JSON.stringify({ id: id })), "GetSingleApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });
    };
    DbSqliteHelper.prototype.DeleteApiTestInfo = function (id, callbackFunction) {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetApiListDataInfoUrl(encodeURIComponent(JSON.stringify({ id: id })), "DeleteApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });
    };
    DbSqliteHelper.prototype.GetAllTestInfo = function (callbackFunction) {
        var httpHelper = new HttpHelper();
        var url = httpHelper.BulidGetApiListDataInfoUrl(JSON.stringify({}), "GetApiInfo");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callbackFunction(data, textStatus, jqXHR);
        });
    };
    return DbSqliteHelper;
}());
//# sourceMappingURL=DbSqliteHelper.js.map