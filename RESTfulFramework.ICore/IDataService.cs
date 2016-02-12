using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace RESTfulFramework.ICore
{
    [ServiceContract]
    public interface IDataService
    {
        #region 通用接口

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = "/post?token={token}&api={api}&type={type}&timestamp={timestamp}&sign={sign}")]
        Stream Post(Stream stream, string token,string api, string type, string timestamp, string sign);

        [OperationContract]
        [WebGet(UriTemplate = "/get?body={body}&token={token}&api={api}&type={type}&timestamp={timestamp}&sign={sign}")]
        Stream Get(string body, string token, string api, string type, string timestamp, string sign);

        #endregion
    }
}
