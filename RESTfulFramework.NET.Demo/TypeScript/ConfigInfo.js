var ConfigInfo = (function () {
    function ConfigInfo() {
        this.SecretKey = "123456";
        this.BaseUserServiceUrl = "http://localhost:8737/UserService";
        this.BaseDataServiceUrl = "";
    }
    ConfigInfo.prototype.CurrentTimestamp = function () {
        return Math.round(new Date().getTime() / 1000).toString();
    };
    return ConfigInfo;
}());
//# sourceMappingURL=ConfigInfo.js.map