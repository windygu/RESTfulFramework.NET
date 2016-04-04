var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="../Scripts/typings/jquery/jquery.d.ts" />
var UserHttpHelper = (function (_super) {
    __extends(UserHttpHelper, _super);
    function UserHttpHelper() {
        _super.apply(this, arguments);
    }
    UserHttpHelper.prototype.Login = function (userName, password, ResultFunction) {
        var loginUrl = this.BulidUserServiceLoginUrl(userName, password);
        $.getJSON(loginUrl, function (data, textStatus, jqXHR) {
            ResultFunction(data, textStatus);
        });
    };
    return UserHttpHelper;
}(HttpHelper));
//# sourceMappingURL=UserHttpHelper.js.map