using RESTfulFramework.NET.ComponentModel;
using System.IO;
using System;
using System.ServiceModel.Activation;

namespace RESTfulFramework.NET.DataService
{
    /// <summary>
    /// WCF服务通用接口
    /// </summary>
    /// <typeparam name="TConfigurator">组件配置信息</typeparam>
    /// <typeparam name="TRequestModel">请求模型</typeparam>
    /// <typeparam name="TResponseModel">输出模型</typeparam>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public abstract class Service 
       : IService
    {

        private Factory.UnitsFactory Factory { get; set; }

        public Service()
        {
            Factory = new Factory.UnitsFactory();
        }

        protected static ILogManager LogManager { get; set; }

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

            try
            {
                var requestModel = new RequestModel
                {
                    Body = StringToObject(body),
                    Token = token,
                    Api = api,
                    Timestamp = timestamp,
                    Sign = sign,
                    Tag = body
                };
                var securityResult = SecurityCheck(requestModel);
                if (!securityResult) return ResponseModelToStream(new ResponseModel { Code = Code.NoAllow, Msg = "权限不足" });


                ResponseModel result = ApiHandler(requestModel);

                return ResponseModelToStream(result);
            }
            catch (Exception ex)
            {

                return ResponseModelToStream(new ResponseModel { Code = Code.SystemException, Msg = ex.Message });

            }
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
        public virtual Stream Post(Stream stream,string token, string api, string timestamp, string sign)
        {
            var body = StreamToString(stream);
            return Get(body, token, api, timestamp, sign);
        }




        /// <summary>
        /// 获取信息通用接口(不用token)
        /// </summary>
        public Stream PostInfo(Stream stream,string api)
        {
            var body = StreamToString(stream);
            return GetInfo(body, api);
        }
        /// <summary>
        /// 获取信息通用接口(不用token)
        /// </summary>
        public Stream GetInfo(string body, string api)
        {
            var requestModel = new RequestModel
            {
                Body = StringToObject(body),
                Api = api,
                Tag = body
            };

            ResponseModel result = InfoApiHandler(requestModel);
            return ResponseModelToStream(result);
        }

        public Stream GetStream(string body, string api)
        {
            var requestModel = new RequestModel
            {
                Body = StringToObject(body),
                Api = api,
                Tag = body
            };

            Stream result = StreamApiHandler(requestModel);
            return result;
        }
        /// <summary>
        /// 安全检查
        /// </summary>
        /// <param name="requestModel">请求的模型</param>
        /// <returns>验证成功返回true,失败返回false</returns>
        public abstract bool SecurityCheck(RequestModel requestModel);




        /// <summary>
        /// 将要输出的对像转为流
        /// </summary>
        /// <param name="responseModel">输出的对像</param>
        /// <returns>流</returns>
        public abstract Stream ResponseModelToStream(ResponseModel responseModel);

        /// <summary>
        /// 将接收的二进制转为请求的对像
        /// </summary>
        /// <param name="stream">流</param>
        /// <returns>对像模型</returns>
        public abstract string StreamToString(Stream stream);

        /// <summary>
        /// 对像转为字符串，重写这个方法时，需要注意的是与StringToObject方法能互转
        /// </summary>
        public abstract string ObjectToString(object obj);

        /// <summary>
        /// 字符串转为对像，重写这个方法时，需要注意的是与ObjectToString方法能互转
        /// </summary>
        public abstract object StringToObject(string str);


        /// <summary>
        /// 处理TOKEN请求
        /// </summary>
        protected virtual ResponseModel ApiHandler(RequestModel requestModel) => Factory.GetTokenApi(requestModel.Api).RunApi(requestModel);

        /// <summary>
        /// 取信息请求(不用验证)
        /// </summary>
        protected virtual ResponseModel InfoApiHandler(RequestModel requestModel) => Factory.GetInfoApi(requestModel.Api).RunApi(requestModel);


        protected virtual Stream StreamApiHandler(RequestModel requestModel) => Factory.GetStreamApi(requestModel.Api).RunApi(requestModel);

    }
}
