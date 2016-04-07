var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
var DataHttpHelper = (function (_super) {
    __extends(DataHttpHelper, _super);
    function DataHttpHelper() {
        _super.apply(this, arguments);
    }
    DataHttpHelper.prototype.PostJson = function (body, api, token, callBackFunction) {
        var url = this.BulidDataServicePostUrl(token, api);
        $.post(url, body, function (data, textStatus, jqXHR) {
            callBackFunction(data, textStatus, jqXHR);
        }, "json");
    };
    DataHttpHelper.prototype.GetJson = function (body, api, token, callBackFunction) {
        var url = this.BulidDataServiceGetUrl(body, token, api);
        $.getJSON(url, function (data, textStatus, jqXHR) {
            callBackFunction(data, textStatus, jqXHR);
        });
    };
    return DataHttpHelper;
}(HttpHelper));
//# sourceMappingURL=DataHttpHelper.js.map