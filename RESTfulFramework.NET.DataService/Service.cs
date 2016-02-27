using RESTfulFramework.NET.ComponentModel;
using PluginPackage.Core;
using System.IO;

namespace RESTfulFramework.NET.DataService
{
    /// <summary>
    /// WCF服务通用接口
    /// </summary>
    /// <typeparam name="TConfigurator">组件配置信息</typeparam>
    /// <typeparam name="TRequestModel">请求模型</typeparam>
    /// <typeparam name="TResponseModel">输出模型</typeparam>

    public abstract class Service< TRequestModel, TResponseModel>
       : IService
       where TRequestModel : RequestModel, new()
       where TResponseModel : ResponseModel, new()
    {

        
        /// <summary>
        /// GET通用接口
        /// </summary>
        /// <param name="body">主要信息</param>
        /// <param name="token">token</param>
        /// <param name="api">api</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="sign">签名</param>
        /// <returns>返回流</returns>
        public virtual Stream Get(string body, string token, string api, string timestamp, string sign)
        {

            var requestModel = new TRequestModel
            {
                Body = StringToObject(body),
                Token = token,
                Api = api,
                Timestamp = timestamp,
                Sign = sign,
                Tag = body
            };
            var securityResult = SecurityCheck(ref requestModel);
            if (!securityResult) return ResponseModelToStream(new TResponseModel { Code = Code.NoAllow, Msg = "权限不足" });

            TResponseModel result = GetContainer().GetPluginInstance<IApiPlugin<TRequestModel, TResponseModel>>(api).RunApi(requestModel);
            return ResponseModelToStream(result);
        }

        /// <summary>
        /// POST通用接口
        /// </summary>
        /// <param name="stream">流</param>
        /// <param name="token">token</param>
        /// <param name="api">api</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="sign">签名</param>
        /// <returns>返回流</returns>
        public virtual Stream Post(Stream stream, string token, string api, string timestamp, string sign)
        {
            var requestModel = StreamToRequestModel(stream);
            return Get(ObjectToString(requestModel.Body), requestModel.Token, requestModel.Api, requestModel.Timestamp, requestModel.Sign);
        }

        /// <summary>
        /// 安全检查
        /// </summary>
        /// <param name="requestModel">请求的模型</param>
        /// <returns>验证成功返回true,失败返回false</returns>
        public abstract bool SecurityCheck(ref TRequestModel requestModel);

        /// <summary>
        /// 获取组件容器
        /// </summary>
        /// <returns>组件容器</returns>
        public abstract IPluginContainer GetContainer();


        /// <summary>
        /// 将要输出的对像转为流
        /// </summary>
        /// <param name="responseModel">输出的对像</param>
        /// <returns>流</returns>
        public abstract Stream ResponseModelToStream(TResponseModel responseModel);

        /// <summary>
        /// 将接收的二进制转为请求的对像
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>对像模型</returns>
        public abstract TRequestModel StreamToRequestModel(Stream stream);

        /// <summary>
        /// 对像转为字符串，重写这个方法时，需要注意的是与StringToObject方法能互转
        /// </summary>
        public abstract string ObjectToString(object obj);

        /// <summary>
        /// 字符串转为对像，重写这个方法时，需要注意的是与ObjectToString方法能互转
        /// </summary>
        public abstract object StringToObject(string str);



    }
}
