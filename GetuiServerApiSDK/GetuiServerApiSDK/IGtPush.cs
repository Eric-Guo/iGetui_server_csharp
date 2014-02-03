using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Net;
using System.Collections;
using Google.ProtocolBuffers;
using System.Security.Cryptography;
using com.gexin.rp.sdk.dto;
using com.igetui.api.openservice.igetui;

/*
 * Create Date: 2013-04-20
 * Create By: johny.zheng
 * Version: V1.0.0
 * Update Date: 2013-06-08
 * Company: mrtech
 * dev.igetui.com
 */

namespace com.igetui.api.openservice
{
    public class IGtPush : IIGtPush
    {
        private String appKey;
        private String host;
        private String masterSecret;
        private String appId;

        public IGtPush(String host, String appKey, String masterSecret)
        {
            this.appKey = appKey;
            this.host = host;
            this.masterSecret = masterSecret;
        }

        public Boolean connect()
        {
            DateTime dt = DateTime.Now;
            long timeStamp = ConvertDateTimeInt(dt);

            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("appkey", appKey);
            param.Add("timeStamp", timeStamp);
            param.Add("masterSecret", masterSecret);
            String sign = getMD5SignValue(param);

            Dictionary<string, object> postData = new Dictionary<string, object>();
            postData.Add("action", "connect");
            postData.Add("appkey", appKey);
            postData.Add("timeStamp", timeStamp);
            postData.Add("sign", sign);

            String Result = httpPostJSON(postData);

            // 判断鉴权是否成功
            if (Result.IndexOf("success") > -1)
            {
                return true;
            }
            else
            {
                // 输出错误原因
                System.Console.WriteLine("----------failed----------" + Result);
            }

            return false;
        }

        //pushMessageToSingle接口测试代码
        public String pushMessageToSingle(SingleMessage message, com.igetui.api.openservice.igetui.Target target)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            /*---以下代码用于设定接口相应参数---*/
            param.Add("action", "pushMessageToSingleAction");
            param.Add("appkey", appKey);
            param.Add("appId", target.appId);
            this.appId = target.appId;
            param.Add("clientId", target.clientId);
            //param.Add("time", "2012-12-31 12:24:23");		            //当前请求时间，可选
            //param.Add("expire", 3600);					            //消息超时时间，单位为秒，可选
            param.Add("isOffline", message.IsOffline);                              //是否离线
            param.Add("offlineExpireTime", message.OfflineExpireTime);           //离线有效时间，单位为毫秒
            param.Add("transmissionContent", message.Data.getTransmissionContent());       //您需要透传的内容
            param.Add("type", 2);                                       //默认都为消息
            param.Add("pushType", message.Data.getPushType());                   //TransmissionMsg, LinkMsg, NotifyMsg    

            //Transparent.Builder builder = getTransparentBuilder();
            param.Add("clientData", Convert.ToBase64String(message.Data.getTransparent().ToByteArray()));

            // post数据
            String Result = httpPostJSON(param);
            System.Console.WriteLine("--------pushMessageToSingle result------------" + Result);
            return Result;
        }

        //pushMessageToApp接口测试代码
        public String pushMessageToApp(AppMessage message)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("action", "pushMessageToAppAction");
            param.Add("appkey", appKey);
            param.Add("transmissionContent", message.Data.getTransmissionContent());
            param.Add("isOffline", message.IsOffline);
            param.Add("offlineExpireTime", message.OfflineExpireTime);
            param.Add("appIdList", message.AppIdList);
            param.Add("phoneTypeList", message.PhoneTypeList);
            param.Add("provinceList", message.ProvinceList);
            param.Add("type", 2);
            param.Add("pushType", message.Data.getPushType());

            //Transparent.Builder builder = getTransparentBuilder();
            param.Add("clientData", Convert.ToBase64String(message.Data.getTransparent().ToByteArray()));

            // post数据
            String Result = httpPostJSON(param);
            System.Console.WriteLine("--------pushMessageToSingle result------------" + Result);
            return Result;
        }

        //PushMessageToList接口测试代码
        public String pushMessageToList(String contentId, List<com.igetui.api.openservice.igetui.Target> targetList)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("action", "pushMessageToListAction");
            param.Add("appkey", appKey);
            param.Add("contentId", contentId);
            param.Add("targetList", targetList);
            param.Add("type", 2);

            // post数据
            String Result = httpPostJSON(param);
            System.Console.WriteLine("---------pushMessageToList result-----------" + Result);
            return Result;
        }

        public void pushMessageToList(ListMessage message, IListProvider listProvider, IPushEventListener listener)
        {
            
        }

        public void close()
        { 
        }

        public String getContentId(ListMessage message)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            String contentId = String.Empty;

            param.Add("action", "getContentIdAction");
            param.Add("appkey", appKey);
            param.Add("clientData", Convert.ToBase64String(message.Data.getTransparent().ToByteArray()));
            param.Add("transmissionContent", message.Data.getTransmissionContent());
            param.Add("isOffline", message.IsOffline);
            param.Add("offlineExpireTime", message.OfflineExpireTime);
            param.Add("pushType", message.Data.getPushType());

            // post数据
            String Result = httpPostJSON(param);
            System.Console.WriteLine("---------pushMessageToList result-----------" + Result);
            ContentInfo contentInfo = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<ContentInfo>(Result);
            String result = contentInfo.result;
            if (result.IndexOf("ok") > -1)
            {
                contentId = contentInfo.contentId;
            }
            return contentId;
        }

        public Boolean cancleContentId(String contentId)
        {
            return false;
        }

        //动作链用pb方式封装
        /***
        private Transparent.Builder getTransparentBuilder()
        {
            Transparent.Builder builder = new Transparent.Builder();
            builder.SetAppId(appId);
            builder.SetAppKey(appKey);
            builder.SetId("");
            builder.SetMessageId("");
            builder.SetTaskId("");
            builder.SetAction("pushmessage");
            builder.SetPushInfo(PushInfo.CreateBuilder().SetActionKey("").SetMessage("").SetBadge("").SetSound("").Build());

            ActionChain actionChain1 = ActionChain.CreateBuilder().SetActionId(1).SetType(ActionChain.Types.Type.Goto).SetNext(10030).Build();
            AppStartUp appStartUp = AppStartUp.CreateBuilder().SetAndroid("").SetSymbia("").SetIos("").Build();
            ActionChain actionChain2 = ActionChain.CreateBuilder().SetActionId(10030).SetType(ActionChain.Types.Type.startapp).SetAppid("")
                .SetAutostart(("1".Equals(transmissionType)) ? true : false).SetAppstartupid(appStartUp).SetFailedAction(100).SetNext(100).Build();
            ActionChain actionChain3 = ActionChain.CreateBuilder().SetActionId(100).SetType(ActionChain.Types.Type.eoa).Build();

            builder.AddActionChain(actionChain1);
            builder.AddActionChain(actionChain2);
            builder.AddActionChain(actionChain3);
            //builder.AddRangeActionChain(actionChains.GetEnumerator());

            return builder;
        }***/

        // post数据
        private String httpPostJSON(Dictionary<string, object> postData)
        {
            postData.Add("version", "3.0.0.0");
            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(postData);
            System.Console.WriteLine(json);
            string Result = httpPost(host, json);//CURL：OpenService接口地址
            System.Console.WriteLine("----------post Result----------" + Result);
            if(Result.IndexOf("sign_error") > -1)
            {
                if (connect())
                {
                    return httpPostJSON(postData);
                }
            }
            return Result;
        }

        //http POST 请求 
        private string httpPost(string postUrl, string paramData)
        {
            String ret = String.Empty;
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "GeTui/1.0";
                webReq.Timeout = GetGetuiServerTimeOut();      //获取系统变量中设置的连接超时时间
                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return ret;
        }

        // 从环境变量中读取连接超时时间，如果没有设置，则调用默认值60秒，环境变量名称为 GETUI_TIMEOUT
        private int GetGetuiServerTimeOut()
        {
            int timeOut = 60000;  //设置超时时间 单位毫秒 默认为60秒
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry de in environmentVariables)
            {
                //Console.WriteLine("  {0} = {1}", de.Key, de.Value);
                if (de.Key.Equals("GETUI_TIMEOUT"))
                {
                    //Console.WriteLine("{0} = {1}", de.Key, de.Value);
                    timeOut = Convert.ToInt32(de.Value);
                }
            }

            return timeOut;
        }

        // DateTime时间格式转换为Unix时间戳格式 
        private long ConvertDateTimeInt(System.DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            DateTime nowTime = DateTime.Now;
            long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);

            return unixTime;
        }

        // 获取MD5加密的sign值
        private string getMD5SignValue(Dictionary<string, object> param)
        {
            // 生成Sign值，用于鉴权
            string sign = "";
            ArrayList list = new ArrayList();
            list.AddRange(param.Keys);

            foreach (string item in list)
            {
                Console.WriteLine(item);
                System.Console.WriteLine(param[item].GetType());

                // 针对非 array object 对象进行sign
                if (param[item].GetType() == typeof(String) || param[item].GetType() == typeof(Int32) || param[item].GetType() == typeof(Int64))
                {
                    sign += param[item];
                }
            }
            System.Console.WriteLine("----------sign: " + sign);
            //MD5加密运算
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(sign));

            //转换字符串
            sign = bytearray2string(result);

            return sign;
        }

        //转换字符串
        private string bytearray2string(byte[] bytes)
        {
            string output = "";
            foreach (byte b in bytes)
            {
                output += b.ToString("x2");
            }
            return output;
        }
    }
}
