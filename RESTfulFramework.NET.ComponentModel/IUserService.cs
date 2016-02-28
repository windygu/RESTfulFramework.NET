using System.ServiceModel;
using System.ServiceModel.Web;

namespace RESTfulFramework.NET.ComponentModel
{
    [ServiceContract]
    public interface IUserService
    {

        [OperationContract]
        [WebGet(UriTemplate = "/login2?username={username}&sign={sign}&timestamp={timestamp}&clientid={clientid}",ResponseFormat = WebMessageFormat.Json)]
        ResponseModel Login2(string username, string sign, string timestamp, string clientid);


        [OperationContract]
        [WebGet(UriTemplate = "/login?username={username}&sign={sign}&timestamp={timestamp}", ResponseFormat = WebMessageFormat.Json)]
        ResponseModel Login(string username, string sign, string timestamp);


        [OperationContract]
        [WebGet(UriTemplate = "/loginout?token={token}", ResponseFormat = WebMessageFormat.Json)]
        ResponseModel LoginOut(string token);


        [OperationContract]
        [WebGet(UriTemplate = "/register?username={username}&password={password}&smscode={smscode}&realname={realname}", ResponseFormat = WebMessageFormat.Json)]
        ResponseModel Register(string username, string password, string smscode, string realname);


        [OperationContract]
        [WebGet(UriTemplate = "/getuserinfo?token={token}", ResponseFormat = WebMessageFormat.Json)]
        ResponseModel GetUserInfo(string token);


        [OperationContract]
        [WebGet(UriTemplate = "/sendsmscode?phone={phone}", ResponseFormat = WebMessageFormat.Json)]
        ResponseModel SendSmsCode(string phone);


        [OperationContract]
        [WebGet(UriTemplate = "/smscodeexist?code={code}", ResponseFormat = WebMessageFormat.Json)]
        ResponseModel SmsCodeExist(string code);

    }
}
