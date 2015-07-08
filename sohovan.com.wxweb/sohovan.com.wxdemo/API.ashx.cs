using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;

namespace sohovan.com.wxdemo
{
    /// <summary>
    /// API 的摘要说明
    /// </summary>
    public class API : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Request.HttpMethod.ToLower() == "post")
            {
                //回复消息的时候也需要验证消息，这个很多开发者没有注意这个，存在安全隐患  
                //微信中 谁都可以获取信息 所以 关系不大 对于普通用户 但是对于某些涉及到验证信息的开发非常有必要
                if (CheckSignature())
                {
                    //接收消息
                }
                else
                {
                    HttpContext.Current.Response.Write("消息并非来自微信");
                    HttpContext.Current.Response.End();
                }
            }
            else
            {
                CheckWechat();
            }
        }

        #region 验证微信签名
        /// <summary>
        /// 返回随机数表示验证成功
        /// </summary>
        private void CheckWechat()
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["echoStr"]))
            {
                HttpContext.Current.Response.Write("消息并非来自微信");
                HttpContext.Current.Response.End();
            }
            string echoStr = HttpContext.Current.Request.QueryString["echoStr"];
            if (CheckSignature())
            {
                HttpContext.Current.Response.Write(echoStr);
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// <returns></returns>
        /// * 将token、timestamp、nonce三个参数进行字典序排序
        /// * 将三个参数字符串拼接成一个字符串进行sha1加密
        /// * 开发者获得加密后的字符串可与signature对比，标识该请求来源于微信。
        private bool CheckSignature()
        {
            string access_token = "sohovan";

            string signature = HttpContext.Current.Request.QueryString["signature"].ToString();
            string timestamp = HttpContext.Current.Request.QueryString["timestamp"].ToString();
            string nonce = HttpContext.Current.Request.QueryString["nonce"].ToString();
            string[] ArrTmp = { access_token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");

            if (tmpStr.ToLower() == signature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}