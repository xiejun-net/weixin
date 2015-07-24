using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using sohovan.com.common;

namespace sohovan.com.wxapi
{
    /// <summary>
    /// 基础接口
    /// </summary>
    public class BasicApi
    {
        public static string SessionAccessToken = "";//access_token缓存 其他接口的通行证

        public BasicApi() { }

        #region 获取access_token缓存
        public static string GetTokenSession(string AppID, string AppSecret)
        {
            string TokenSession = "";

            if (System.Web.HttpContext.Current.Session[SessionAccessToken] == null)
            {
                TokenSession = AddTokenSession(AppID, AppSecret);
            }
            else
            {
                TokenSession = System.Web.HttpContext.Current.Session[SessionAccessToken].ToString();
            }

            return TokenSession;
        }
        /// <summary>
        /// 添加AccessToken缓存
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSecret"></param>
        /// <returns></returns>
        public static string AddTokenSession(string AppID, string AppSecret)
        {
            //获取AccessToken
            string AccessToken = GetAccessToken(AppID, AppSecret);
            HttpContext.Current.Session[SessionAccessToken] = AccessToken;
            HttpContext.Current.Session.Timeout = 7200;
            return AccessToken;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSecret"></param>
        /// <returns></returns>
        public static string GetAccessToken(string AppID, string AppSecret)
        {
            JavaScriptSerializer Jss = new JavaScriptSerializer();
            string respText = CommonMethod.WebRequestPostOrGet(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppID, AppSecret), "");
            Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
            string accessToken = respDic["access_token"].ToString();
            return accessToken;
        }
        #endregion


    }
}
