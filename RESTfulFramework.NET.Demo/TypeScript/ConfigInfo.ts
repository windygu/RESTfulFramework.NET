class ConfigInfo {
    public static SecretKey: string = "123456";
    public static BaseUserServiceUrl: string = "http://localhost:8737/UserService";
    public static BaseDataServiceUrl: string = "http://localhost:8736/DataService";
    public CurrentTimestamp(): string {
        return Math.round(new Date().getTime() / 1000).toString();
    }
    public static InterfaceDataServiceUrl: string = "";

}