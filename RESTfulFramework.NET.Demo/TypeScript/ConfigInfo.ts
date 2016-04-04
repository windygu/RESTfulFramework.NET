class ConfigInfo {
    public SecretKey: string = "123456";
    public BaseUserServiceUrl: string = "http://localhost:8737/UserService";
    public BaseDataServiceUrl: string = "";
    public CurrentTimestamp(): string {
        return Math.round(new Date().getTime() / 1000).toString();
    }
}