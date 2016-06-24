class ConfigInfo {
    public static SecretKey: string = "123456";
    public static BaseUserServiceUrl: string = "http://localhost:57050/UserService.svc";
    public static BaseDataServiceUrl: string = "http://localhost:57050/DataService.svc";
    public CurrentTimestamp(): string {
        return Math.round(new Date().getTime() / 1000).toString();
    } 
    public static InterfaceDataServiceUrl: string = "";
     
}