var ConfigInfo = (function () {
    function ConfigInfo() {
    }
    ConfigInfo.prototype.CurrentTimestamp = function () {
        return Math.round(new Date().getTime() / 1000).toString();
    };
    ConfigInfo.SecretKey = "123456";
    ConfigInfo.BaseUserServiceUrl = "http://localhost:8737/UserService";
    ConfigInfo.BaseDataServiceUrl = "http://localhost:8736/DataService";
    return ConfigInfo;
}());
