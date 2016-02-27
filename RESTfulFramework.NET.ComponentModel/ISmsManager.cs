namespace RESTfulFramework.NET.ComponentModel
{
    public interface ISmsManager
    {
        bool SendSms(string phone, string content);
        string ReadSms(string phone);
    }
}
