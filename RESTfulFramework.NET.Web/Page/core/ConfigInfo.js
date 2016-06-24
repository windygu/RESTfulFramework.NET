var ConfigInfo = (function () {
    function ConfigInfo() {
    }
    ConfigInfo.prototype.CurrentTimestamp = function () {
        return Math.round(new Date().getTime() / 1000).toString();
    };
    ConfigInfo.SecretKey = "123456";
    ConfigInfo.BaseUserServiceUrl = "http://localhost:57050/UserService.svc";
    ConfigInfo.BaseDataServiceUrl = "http://localhost:57050/DataService.svc";
    ConfigInfo.InterfaceDataServiceUrl = "";
    return ConfigInfo;
}());
//# sourceMappingURL=ConfigInfo.js.map