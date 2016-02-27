using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace RESTfulFramework.NET.ComponentModel
{
    /// <summary>
    /// 基础核心接口
    /// </summary>
    [ServiceContract]
    public interface IService

    {
        #region 通用接口

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = "/post?token={token}&api={api}&timestamp={timestamp}&sign={sign}")]
        Stream Post(Stream stream, string token, string api, string timestamp, string sign);

        [OperationContract]
        [WebGet(UriTemplate = "/get?body={body}&token={token}&api={api}&timestamp={timestamp}&sign={sign}")]
        Stream Get(string body, string token, string api, string timestamp, string sign);

        #endregion
    }
}
