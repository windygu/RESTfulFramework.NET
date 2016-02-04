
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ServiceModel.Web;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;
using RESTfulFramework.ICore;
using RESTfulFramework.IProtocolPlugin.Model;
using RESTfulFramework.IExpandPlugin;
using RESTfulFramework.IProtocolPlugin;
using RESTfulFramework.Core.CorePlugin;
using RESTfulFramework.Core.RequestExpandPlugin;
using RESTfulFramework.Core.ProtocolPlugin;

namespace RESTfulFramework.Core
{
    public class DataService : IDataService
    {


        #region 实例初始
        public DataService() { }

        #endregion


        #region 组件定义

        private static LogDefine LogPlugin { get; set; }
        private static JsonSerialzerDefine JsonSerialzerPlugin { get; set; }
        private static DataCheckDefine DataCheckPlugin { get; set; }
        private static BodyTransforObjectDefine BodyTransforObjectPlugin { get; set; }

        private static UserDefine UserStatePlugin;

        static Dictionary<string, IExpand> RequestExpandPluginList { get; set; } = new Dictionary<string, IExpand>();

        static Dictionary<string, IProtocol> ProtocolList { get; set; } = new Dictionary<string, IProtocol>();

        #endregion


        #region 静态初始
        static DataService()
        {

            #region 加载基础核心组件
            try
            {
                LogPlugin = new LogDefine();
                JsonSerialzerPlugin = new JsonSerialzerDefine();
                DataCheckPlugin = new DataCheckDefine();
                BodyTransforObjectPlugin = new BodyTransforObjectDefine();
                UserStatePlugin = new UserDefine();

                var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CorePlugin");
                var catalog = new DirectoryCatalog(directory);
                var container = new CompositionContainer(catalog);
                try { container.ComposeParts(LogPlugin); } catch (Exception ex) { LogPlugin.Log?.WriteLog($"导入LogPlugin插件出错，异常原因{ex.Message}"); }
                try { container.ComposeParts(JsonSerialzerPlugin); } catch (Exception ex) { LogPlugin.Log?.WriteLog($"导入JsonSerialzerPlugin插件出错，异常原因{ex.Message}"); }
                try { container.ComposeParts(DataCheckPlugin); } catch (Exception ex) { LogPlugin.Log?.WriteLog($"导入DataCheckPlugin插件出错，异常原因{ex.Message}"); }
                try { container.ComposeParts(BodyTransforObjectPlugin); } catch (Exception ex) { LogPlugin.Log?.WriteLog($"导入BodyTransforObjectPlugin插件出错，异常原因{ex.Message}"); }
                try { container.ComposeParts(UserStatePlugin); } catch (Exception ex) { LogPlugin.Log?.WriteLog($"导入UserStatePlugin插件出错，异常原因{ex.Message}"); }
            }
            catch (Exception) { }

            #endregion

            #region 加载所有协义组件
            try
            {
                var protocolDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ProtocolPlugin");
                foreach (var protocolItem in Directory.GetFiles(protocolDirectory, "*.dll"))
                {
                    try
                    {
                        var myProtocol = new ProtocolEx();
                        var fileName = (new FileInfo(protocolItem)).Name;
                        var catalog = new DirectoryCatalog(protocolDirectory, fileName);
                        var container = new CompositionContainer(catalog);
                        container.ComposeParts(myProtocol);
                        ProtocolList.Add(fileName.Replace(".dll", "").Replace(".DLL", ""), myProtocol.Protocol);
                    }
                    catch (Exception ex)
                    {
                        LogPlugin.Log?.WriteLog(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LogPlugin.Log?.WriteLog(ex.Message);
            }
            #endregion

            #region 加载扩展组件
            try
            {
                var requestExpandDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RequestExpandPlugin");
                foreach (var requestExpandItem in Directory.GetFiles(requestExpandDirectory, "*.dll"))
                {
                    try
                    {
                        var requestExpand = new RequestExpandDefine();
                        var fileName = (new FileInfo(requestExpandItem)).Name;
                        var catalog = new DirectoryCatalog(requestExpandDirectory, fileName);
                        var container = new CompositionContainer(catalog);
                        container.ComposeParts(requestExpand);
                        RequestExpandPluginList.Add(fileName.Replace(".dll", "").Replace(".DLL", ""), requestExpand.RequestExpand);
                    }
                    catch (Exception ex)
                    {
                        LogPlugin.Log?.WriteLog(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LogPlugin.Log?.WriteLog(ex.Message);
            }
            #endregion

        }
        #endregion



        public Stream Get(string body, string token, string protocol, string timestamp, string sign)
        {

            try
            {
                LogPlugin.Log?.WriteLog($"{{{body}}}{{{token}}}{{{protocol}}}{{{timestamp}}}{{{sign}}}");

                #region 设置头部信息
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.ContentType = "application/json;charset=utf-8";
                #endregion

                #region 扩展组件的干涉
                if (RequestExpandPluginList != null && RequestExpandPluginList.Any())
                {
                    var requestExpandList = RequestExpandPluginList.OrderBy(m => m.Value.GetIndex());
                    foreach (var item in requestExpandList)
                    {
                        var requestData = item.Value.Expand(new IExpandPlugin.Model.RequestData
                        {
                            body = body,
                            protocol = protocol,
                            timestamp = timestamp,
                            sign = sign,
                            token = token
                        });
                        body = requestData.body;
                        protocol = requestData.protocol;
                        timestamp = requestData.timestamp;
                        sign = requestData.sign;
                        token = requestData.token;
                    }
                }
                #endregion

                #region  检查数据完整性(签名)
                var signResult = DataCheckPlugin.DataCheck?.CheckSign(body, token, protocol, sign, timestamp);
                if (signResult == false)
                {
                    return JsonSerialzerPlugin.JsonSerialzer?.SerializeObject(
                        new Result<string>
                        {
                            Code = CodeEnum.SignErron,
                            Msg = "sign error"
                        })
                      .ToStream();
                }
                #endregion

                #region body转为对像
                var bodyObject = BodyTransforObjectPlugin.BodyTransforObject?.TransforObject(body);
                #endregion

                #region  获取当前用户
                var user = UserStatePlugin.UserState?.GetUser(token);
                #endregion

                #region  调用协议
                if (ProtocolList.ContainsKey(protocol))
                {
                    var result = ProtocolList[protocol].SetupProtocol(bodyObject, protocol, user);
                    LogPlugin.Log?.WriteLog($"{{{result}}}");
                    return JsonSerialzerPlugin.JsonSerialzer.SerializeObject(result).ToStream();
                }
                else
                {
                    var result = new Result<object>
                    {
                        Code = CodeEnum.SystemException,
                        Msg = "未找到该调用协议"
                    };
                    LogPlugin.Log?.WriteLog($"{{{result}}}");
                    return JsonSerialzerPlugin.JsonSerialzer.SerializeObject(result)
                        .ToStream();
                }
                #endregion

            }
            catch (Exception ex)
            {
                LogPlugin.Log?.WriteLog(ex.Message);
                return JsonSerialzerPlugin.JsonSerialzer?.SerializeObject(
                    new Result<string>
                    {
                        Code = CodeEnum.SystemException,
                        Msg = ex.Message
                    })
                .ToStream();
            }
        }

        public Stream Post(Stream stream, string token, string protocol, string timestamp, string sign)
        {
            //获取流数据
            string body;
            using (var sr = new StreamReader(stream)) { body = sr.ReadToEnd(); }
            return Get(body, token, protocol, timestamp, sign);
        }
    }
}
