using System.ServiceModel;
using System.ServiceModel.Web;

namespace RESTfulFramework.NET.ComponentModel
{
    [ServiceContract]
    public interface IUserService
    {

        [OperationContract]
        [WebGet(UriTemplate = "/login2?username={username}&sign={sign}&timestamp={timestamp}&clientid={clientid}")]
        ResponseModel Login2(string username, string sign, string timestamp, string clientid);


        [OperationContract]
        [WebGet(UriTemplate = "/login?username={username}&sign={sign}&timestamp={timestamp}")]
        ResponseModel Login(string username, string sign, string timestamp);


        [OperationContract]
        [WebGet(UriTemplate = "/loginout?token={token}")]
        ResponseModel LoginOut(string token);


        [OperationContract]
        [WebGet(UriTemplate = "/register?username={username}&password={password}&smscode={smscode}&realname={realname}")]
        ResponseModel Register(string username, string password, string smscode, string realname);


        [OperationContract]
        [WebGet(UriTemplate = "/getuserinfo?token={token}")]
        ResponseModel GetUserInfo(string token);


        [OperationContract]
        [WebGet(UriTemplate = "/sendsmscode?phone={phone}")]
        ResponseModel SendSmsCode(string phone);


        [OperationContract]
        [WebGet(UriTemplate = "/smscodeexist?code={code}")]
        ResponseModel SmsCodeExist(string code);

    }
}
