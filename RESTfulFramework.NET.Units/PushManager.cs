//using RESTfulFramework.NET.ComponentModel;
//using RESTfulFramework.NET.Common.Model;
//using com.igetui.api.openservice.igetui.template;
//using com.igetui.api.openservice;
//using com.igetui.api.openservice.igetui;
//using com.igetui.api.openservice.payload;

//namespace RESTfulFramework.NET.Units
//{
//    public class PushManager : IPushManager<PushInfo>
//    {
//        private static string Host { get; set; }
//        private static string AppKey { get; set; }
//        private static string MasterSecret { get; set; }
//        private static string AppID { get; set; }
//        static PushManager()
//        {
//            var configManager = Common.UnitsFactory.ConfigManager;
//            try
//            {
//                Host = configManager?.GetValue("getui_host").value;
//                AppKey = configManager?.GetValue("getui_appkey").value;
//                MasterSecret = configManager?.GetValue("getui_mastersecret").value;
//                AppID = configManager?.GetValue("getui_appid").value;
//            }
//            catch (System.Exception ex)
//            {
//                Common.UnitsFactory.LogManager.WriteLog(ex.Message);
//            }

//        }

//        public bool PushInfo(PushInfo info)
//        {
//            // 推送主类
//            //根据HOST判断是http还是https
//            IGtPush push = new IGtPush(Host, AppKey, MasterSecret);
//            //http接口的访问模式
//            //IGtPush push = new IGtPush(APPKEY, MASTERSECRET, false);
//            //https接口的访问模式
//            //IGtPush push = new IGtPush(APPKEY, MASTERSECRET, true);

//            /*消息模版：
//                1.TransmissionTemplate:透传模板
//                2.LinkTemplate:通知链接模板
//                3.NotificationTemplate：通知透传模板
//                4.NotyPopLoadTemplate：通知弹框下载模板
//             */

//            //TransmissionTemplate template = TransmissionTemplateDemo();
//            NotificationTemplate template = NotificationTemplateDemo(AppID, AppKey, info);
//            //LinkTemplate template = LinkTemplateDemo();
//            //NotyPopLoadTemplate template = NotyPopLoadTemplateDemo();
//            template.TransmissionContent = info.Content;

//            // 单推消息模型
//            SingleMessage message = new SingleMessage();
//            message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
//            message.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
//            message.Data = template;
//            message.PushNetWorkType = 0;        //判断是否客户端是否wifi环境下推送，1为在WIFI环境下，0为非WIFI环境

//            com.igetui.api.openservice.igetui.Target target = new com.igetui.api.openservice.igetui.Target();
//            target.appId = AppID;
//            target.clientId = info.CliendId;
//            target.alias = info.Alias;

//            try
//            {
//                var pushResult = push.pushMessageToSingle(message, target);
//            }
//            catch (RequestException e)
//            {
//                var requestId = e.RequestId;
//                var pushResult = push.pushMessageToSingle(message, target, requestId);

//            }
//            return true;

//        }


//        /// <summary>
//        /// 透传模板动作内容
//        /// 所有推送接口均支持四个消息模板，依次为透传模板，通知透传模板，通知链接模板，通知弹框下载模板
//        /// 注：IOS离线推送需通过APN进行转发，需填写pushInfo字段，目前仅不支持通知弹框下载功能</summary>
//        public static TransmissionTemplate TransmissionTemplateDemo(string appid, string appkey, PushInfo info)
//        {
//            var template = new TransmissionTemplate();
//            template.AppId = appid;
//            template.AppKey = appkey;
//            template.TransmissionType = "1";            //应用启动类型，1：强制应用启动 2：等待应用启动
//            template.TransmissionContent = "";  //透传内容

//            //APN高级推送
//            var apnpayload = new APNPayload();
//            var alertMsg = new DictionaryAlertMsg();
//            alertMsg.Body = "Body";
//            alertMsg.ActionLocKey = "ActionLocKey";
//            alertMsg.LocKey = "LocKey";
//            alertMsg.addLocArg("LocArg");
//            alertMsg.LaunchImage = "LaunchImage";

//            //IOS8.2支持字段
//            alertMsg.Title = "Title";
//            alertMsg.TitleLocKey = "TitleLocKey";
//            alertMsg.addTitleLocArg("TitleLocArg");
//            apnpayload.AlertMsg = alertMsg;
//            apnpayload.Badge = 10;
//            apnpayload.ContentAvailable = 1;
//            template.setAPNInfo(apnpayload);

//            //设置客户端展示时间
//            return template;
//        }

//        //通知透传模板动作内容
//        public static NotificationTemplate NotificationTemplateDemo(string appid, string appkey, PushInfo info)
//        {
//            NotificationTemplate template = new NotificationTemplate();
//            template.AppId = appid;
//            template.AppKey = appkey;
//            template.Title = info.Title;         //通知栏标题
//            template.Text = info.Descript;          //通知栏内容
//            template.Logo = "";               //通知栏显示本地图片
//            template.LogoURL = "";                    //通知栏显示网络图标
//            template.TransmissionType = "1";          //应用启动类型，1：强制应用启动  2：等待应用启动
//            template.TransmissionContent = info.Content;   //透传内容

//            //设置客户端展示时间
//            //String begin = "2015-03-06 14:36:10";
//            //String end = "2015-03-06 14:46:20";
//            //template.setDuration(begin, end);

//            template.IsRing = true;                //接收到消息是否响铃，true：响铃 false：不响铃
//            template.IsVibrate = true;               //接收到消息是否震动，true：震动 false：不震动
//            template.IsClearable = true;             //接收到消息是否可清除，true：可清除 false：不可清除
//            return template;
//        }


//        /// <summary>
//        /// 通知链接动作内容
//        /// </summary>
//        public static LinkTemplate LinkTemplateDemo(string appid, string appkey, PushInfo info)
//        {
//            LinkTemplate template = new LinkTemplate();
//            template.AppId = appid;
//            template.AppKey = appkey;
//            template.Title = info.Title;         //通知栏标题
//            template.Text = info.Descript;          //通知栏内容
//            template.Logo = "";               //通知栏显示本地图片
//            template.LogoURL = info.LogoUrl;  //通知栏显示网络图标，如无法读取，则显示本地默认图标，可为空
//            template.Url = info.Url;      //打开的链接地址

//            template.IsRing = true;                 //接收到消息是否响铃，true：响铃 false：不响铃
//            template.IsVibrate = true;               //接收到消息是否震动，true：震动 false：不震动
//            template.IsClearable = true;             //接收到消息是否可清除，true：可清除 false：不可清除
//            return template;
//        }



//    }
//}
