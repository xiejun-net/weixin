using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

namespace sohovan.com.common
{
    /// <summary>
    /// 通用方法类
    /// </summary>
    public class CommonMethod
    {
        #region Post/Get提交调用抓取
        /// <summary>
        /// Post/get 提交调用抓取
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="param">参数</param>
        /// <returns>string</returns>
        public static string WebRequestPostOrGet(string sUrl, string sParam)
        {
            byte[] bt = System.Text.Encoding.UTF8.GetBytes(sParam);

            Uri uriurl = new Uri(sUrl);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(uriurl);//HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url + (url.IndexOf("?") > -1 ? "" : "?") + param);
            req.Method = "Post";
            req.Timeout = 120 * 1000;
            req.ContentType = "application/x-www-form-urlencoded;";
            req.ContentLength = bt.Length;

            using (Stream reqStream = req.GetRequestStream())//using 使用可以释放using段内的内存
            {
                reqStream.Write(bt, 0, bt.Length);
                reqStream.Flush();
            }
            try
            {
                using (WebResponse res = req.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理 

                    Stream resStream = res.GetResponseStream();

                    StreamReader resStreamReader = new StreamReader(resStream, System.Text.Encoding.UTF8);

                    string resLine;

                    System.Text.StringBuilder resStringBuilder = new System.Text.StringBuilder();

                    while ((resLine = resStreamReader.ReadLine()) != null)
                    {
                        resStringBuilder.Append(resLine + System.Environment.NewLine);
                    }

                    resStream.Close();
                    resStreamReader.Close();

                    return resStringBuilder.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;//url错误时候回报错
            }
        }
        #endregion Post/Get提交调用抓取

        #region unix/datatime 时间转换
        /// <summary>
        /// unix时间转换为datetime
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// datetime转换为unixtime
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        #endregion

        #region 记录bug，以便调试
        /// <summary>
        /// 记录bug，以便调试
        /// </summary>
        public static bool WriteTxt(string str)
        {
            try
            {
                string LogPath = HttpContext.Current.Server.MapPath("/err_log/");
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
                FileStream FileStream = new FileStream(System.Web.HttpContext.Current.Server.MapPath("/err_log//xiejun_" + DateTime.Now.ToLongDateString() + "_.txt"), FileMode.Append);
                StreamWriter StreamWriter = new StreamWriter(FileStream);
                //开始写入
                StreamWriter.WriteLine(str);
                //清空缓冲区
                StreamWriter.Flush();
                //关闭流
                StreamWriter.Close();
                FileStream.Close();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 生成随机字符
        /// <summary>
        /// 生成随机字符
        /// </summary>
        /// <param name="iLength">生成字符串的长度</param>
        /// <returns>返回随机字符串</returns>
        public static string GetRandCode(int iLength)
        {
            string sCode = "";
            if (iLength == 0)
            {
                iLength = 4;
            }
            string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] arr = codeSerial.Split(',');
            int randValue = -1;
            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (int i = 0; i < iLength; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);
                sCode += arr[randValue];
            }
            return sCode;
        }
        #endregion

        #region 根据ip获取地点
        /// 获取Ip归属地
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns>归属地</returns>
        public static string GetIpAddress(string ip)
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            //http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js&ip=218.192.3.42 调用新浪的接口
            string address = string.Empty;
            try
            {
                string reText = WebRequestPostOrGet("http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js&ip=" + ip, "");
                Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(reText);
                address = DicText["city"].ToString();
                WriteTxt("city:" + address);
            }
            catch { }
            return address;
        }
        #endregion
    }
}
