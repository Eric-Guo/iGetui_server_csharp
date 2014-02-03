using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.ProtocolBuffers;
using com.gexin.rp.sdk.dto;
using com.igetui.api.openservice;
using com.igetui.api.openservice.igetui;
using com.igetui.api.openservice.igetui.template;



/**
 * Create Date: 2013-04-20
 * Create By: johny.zheng
 * Version: V1.0.0
 * Update Date: 2013-06-08/ 2013-09-27
 * Company: mrtech
 * Url: dev.igetui.com
 * 
 * 说明：
 *      此工程是一个测试工程，所用的相关.dll文件，都已经存在protobuffer文件里，需要加载到References里。
 *      工程中还有用到一个System.Web.Extensions文件，这个文件是用到Framework里V4.0版本的，
 *      一般路径如下：C:\Program Files\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0，
 *      或如下路径：C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.5
 *      没有的也可以在protobuffer文件夹里找到。
 *      如再有问题，请直接联系技术客服，谢谢！
 *      
 * 
 *      更新时间2013.12.02  VERSION:3.0.0.0
 * 
 *      GetuiServerApiSDK：此.dll文件为个推C#版本的SDK文件
 *      Google.ProtocolBuffers：此.dll文件为Google的数据交换格式文件
 *      
 *  注：
 *      新增一个连接超时时间设置，通过在环境变量--用户变量中增加名为：GETUI_TIMEOUT 的变量（修改环境变量，电脑重启后才能生效），值则是超时时间，如不设定，则默认为20秒。
 **/
namespace GetuiServerApiSDKDemo
{
    public class demo
    {
        //参数设置 <-----参数需要重新设置----->
        private static String APPID = "您应用的AppId";                     //您应用的AppId
        private static String APPKEY = "您应用的AppKey";                    //您应用的AppKey
        private static String MASTERSECRET = "您应用的MasterSecret";              //您应用的MasterSecret 
        private static String CLIENTID = "您获取的clientID";        //您获取的clientID
        private static String HOST = "http://sdk.open.api.igexin.com/apiex.htm";    //HOST：OpenService接口地址

        static void Main(string[] args)
        {
            //PushMessageToList接口
            PushMessageToList();

            //PushMessageToSingle接口
            //PushMessageToSingle();

            //pushMessageToApp接口
            //pushMessageToApp();
        }

        //PushMessageToSingle接口测试代码
        private static void PushMessageToSingle()
        {
            // 推送主类
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);

            //通知模版：支持TransmissionTemplate、LinkTemplate、NotificationTemplate，此处以TransmissionTemplate为例
            TransmissionTemplate template = new TransmissionTemplate();
            template.AppId = APPID;                             // 应用APPID
            template.AppKey = APPKEY;                           // 应用APPKEY
            template.TransmissionType = "1";                    // 收到消息是否立即启动应用，1为立即启动，2则广播等待客户端自启动
            template.TransmissionContent = "您需要透传的内容";  // 需要透传的内容

            //iOS推送需要的pushInfo字段
            //template.setPushInfo(actionLocKey, badge, message, sound, payload, locKey, locArgs, launchImage);

            // 单推消息模型
            SingleMessage message = new SingleMessage();
            message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
            message.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
            message.Data = template;

            com.igetui.api.openservice.igetui.Target target = new com.igetui.api.openservice.igetui.Target();
            target.appId = APPID;
            target.clientId = CLIENTID;

            String pushResult = push.pushMessageToSingle(message, target);
            System.Console.WriteLine("-----------PushMessageToSingle--------------" + pushResult);
        }

        //PushMessageToList接口测试代码
        private static void PushMessageToList()
        {
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);

            ListMessage message = new ListMessage();

            NotificationTemplate template = new NotificationTemplate();
            template.AppId = APPID;                             // 应用APPID
            template.AppKey = APPKEY;                           // 应用APPKEY
            template.Title = "此处填写通知标题";                // 通知标题
            template.Text = "此处填写通知内容";                 // 通知内容
            template.Logo = "icon.png";                         // 应用Logo
            template.LogoURL = "http://www.photophoto.cn/m23/086/010/0860100017.jpg"; //通知栏网络图片展示
            template.TransmissionType = "2";                    // 收到消息是否立即启动应用，1为立即启动，2则广播等待客户端自启动
            template.TransmissionContent = "你需要透传的内容";  // 需要透传的内容
            //template.IsRing = true;					// 收到通知是否响铃，可选，默认响铃
            //template.IsVibrate = true;					// 收到通知是否震动，可选，默认振动
            //template.IsClearable = true;				// 通知是否可清除，可选，默认可清除

            message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
            message.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
            message.Data = template;

            //设置接收者
            List<com.igetui.api.openservice.igetui.Target> targetList = new List<com.igetui.api.openservice.igetui.Target>();
            com.igetui.api.openservice.igetui.Target target1 = new com.igetui.api.openservice.igetui.Target();
            target1.appId = APPID;
            target1.clientId = CLIENTID;

            // 如需要，可以设置多个接收者
            //com.igetui.api.openservice.igetui.Target target2 = new com.igetui.api.openservice.igetui.Target();
            //target2.AppId = APPID;
            //target2.ClientId = "ddf730f6cabfa02ebabf06e0c7fc8da0";

            targetList.Add(target1);
            //targetList.Add(target2);

            String contentId = push.getContentId(message);
            String pushResult = push.pushMessageToList(contentId, targetList);
            System.Console.WriteLine("-----------PushMessageToList--------------" + pushResult);
        }

        //pushMessageToApp接口测试代码
        private static void pushMessageToApp()
        {
            IGtPush push = new IGtPush(HOST, APPKEY, MASTERSECRET);

            AppMessage message = new AppMessage();

            //通知模版：支持TransmissionTemplate、LinkTemplate、NotificationTemplate，此处以LinkTemplate为例
            LinkTemplate template = new LinkTemplate();
            template.AppId = APPID;                     //应用APPID
            template.AppKey = APPKEY;                   // 应用APPKEY
            template.Title = "toAPP消息";        // 通知标题
            template.Text = "toAPP消息";         // 通知内容
            template.Logo = "icon.png";                 // 通知Logo
            template.LogoURL = "http://www.photophoto.cn/m23/086/010/0860100017.jpg"; //通知栏网络图片展示
            template.Url = "http://www.baidu.com";            // 跳转地址
            //template.IsRing = true;					// 收到通知是否响铃，可选，默认响铃
            //template.IsVibrate = true;					// 收到通知是否震动，可选，默认振动
            //template.IsClearable = true;				// 通知是否可清除，可选，默认可清除

            message.IsOffline = true;                         // 用户当前不在线时，是否离线存储,可选
            message.OfflineExpireTime = 1000 * 3600 * 12;            // 离线有效时间，单位为毫秒，可选
            message.Data = template;

            List<String> appIdList = new List<string>();
            appIdList.Add(APPID);

            List<String> phoneTypeList = new List<string>();    //通知接收者的手机操作系统类型
            phoneTypeList.Add("ANDROID");
			//phoneTypeList.Add("IOS");

            List<String> provinceList = new List<string>();     //通知接收者所在省份
            //provinceList.Add("浙江");
            //provinceList.Add("上海");
            //provinceList.Add("北京");

            message.AppIdList = appIdList;
            message.PhoneTypeList = phoneTypeList;
            message.ProvinceList = provinceList;

            String pushResult = push.pushMessageToApp(message);
            System.Console.WriteLine("-----------pushMessageToApp--------------" + pushResult);
        }
    }
}
