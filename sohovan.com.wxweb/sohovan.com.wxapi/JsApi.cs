using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Web.Script.Serialization;
using sohovan.com.common;

namespace sohovan.com.wxapi
{
    public class JsApi
    {
        JavaScriptSerializer Jss = new JavaScriptSerializer();

        public JsApi() { }

        #region 验证JsApi权限配置 
        /// <summary>
        /// 获取JsApi权限配置的数组/四个参数
        /// </summary>
        /// <param name="Appid">应用id</param>
        /// <param name="Appsecret">密钥</param>
        /// <returns>json格式的四个参数</returns>
        public string GetJsApiInfo(string Appid, string Appsecret)
        {
            string timestamp = CommonMethod.ConvertDateTimeInt(DateTime.Now).ToString();//生成签名的时间戳
            string nonceStr = CommonMethod.GetRandCode(16);//生成签名的随机串
            string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri.ToString();//当前的地址
            string jsapi_ticket = "";
            //ticket 缓存7200秒
            if (System.Web.HttpContext.Current.Session["jsapi_ticket"] == null)
            {
                jsapi_ticket = CommonMethod.WebRequestPostOrGet("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + BasicApi.GetTokenSession(Appid, Appsecret) + "&type=jsapi", "");
                System.Web.HttpContext.Current.Session["jsapi_ticket"] = jsapi_ticket;
                System.Web.HttpContext.Current.Session.Timeout = 7200;
            }
            else
            {
                jsapi_ticket = System.Web.HttpContext.Current.Session["jsapi_ticket"].ToString();
            }
            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(jsapi_ticket);
            jsapi_ticket = respDic["ticket"].ToString();//获取ticket
            string[] ArrayList = { "jsapi_ticket=" + jsapi_ticket, "timestamp=" + timestamp, "noncestr=" + nonceStr, "url=" + url };
            Array.Sort(ArrayList);
            string signature = string.Join("&", ArrayList);
            signature = FormsAuthentication.HashPasswordForStoringInConfigFile(signature, "SHA1").ToLower();
            return "{\"appId\":\"" + Appid + "\", \"timestamp\":" + timestamp + ",\"nonceStr\":\"" + nonceStr + "\",\"signature\":\"" + signature + "\"}";
        }
        #endregion
    }
}
