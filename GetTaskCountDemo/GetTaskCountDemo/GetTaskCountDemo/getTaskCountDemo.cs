using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;

namespace getTaskCountDemo
{
    public class GetTaskCountDemo
    {

        static void Main(string[] args)
        {
            String appKey = "tpDVam96sY8pxhwBupJ462";
            String masterSecret = "TBokfpttQJ6aHIhBE9y867";
            String taskId = "GT_1017_gJs4GvJxZV77gdgBKsuvO9";
            String url = "http://sdk.open.api.igexin.com/api.htm";

            Dictionary<string, object> reseult = getPushResult(url, appKey, masterSecret, taskId);

        }
        private static Dictionary<string, object> getPushResult(String url, String appKey, String masterSecret, String taskId)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("action", "getPushMsgResult");
            param.Add("appkey", appKey);
            param.Add("taskId", taskId);
            string sign = createSign(param, masterSecret);
            param.Add("sign", sign);
            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(param);
            System.Console.WriteLine(json);
            string resp = httpPost(url, json);
            System.Console.WriteLine(resp);
            Dictionary<string, object> result = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Dictionary<string, object>>(resp);
            return result;
        }
        private static string createSign(Dictionary<string, object> param, string masterSecret)
        {
            string sign = masterSecret;
            ArrayList list = new ArrayList();
            list.AddRange(param.Keys);
            foreach (string item in list)
            {
                Console.WriteLine(item);
                System.Console.WriteLine(param[item].GetType());

                // 针对非 array object 对象进行sign
                if (param[item].GetType() == typeof(String) || param[item].GetType() == typeof(Int32) || param[item].GetType() == typeof(Int64))
                {
                    sign += item + param[item];
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
        private static string bytearray2string(byte[] bytes)
        {
            string output = "";
            foreach (byte b in bytes)
            {
                output += b.ToString("x2");
            }
            return output;
        }
        //http POST 请求 
        private static string httpPost(string postUrl, string paramData)
        {
            String ret = String.Empty;
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "GeTui/1.0";
                webReq.Timeout = 300;      //获取系统变量中设置的连接超时时间
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

    }
}
