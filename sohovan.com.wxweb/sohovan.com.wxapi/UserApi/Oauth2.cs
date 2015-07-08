using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using sohovan.com.common;

namespace sohovan.com.wxapi.UserApi
{
    //微信网页授权2.0
    public class Oauth2
    {
        JavaScriptSerializer Jss = new JavaScriptSerializer();
        public Oauth2() { }

        /// <summary>
        /// 对页面是否要用授权 
        /// </summary>
        /// <param name="Appid">微信应用id</param>
        /// <param name="redirect_uri">回调页面</param>
        /// <param name="scope">应用授权作用域snsapi_base（不弹出授权页面，直接跳转，只能获取用户openid），snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）</param>
        /// <returns>授权地址</returns>
        public string GetCodeUrl(string Appid, string redirect_uri, string scope)
        {
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state=STATE#wechat_redirect", Appid, redirect_uri, scope);
        }

        /// <summary>
        /// 用code换取openid 此方法一般是不获取用户昵称时候使用
        /// </summary>
        /// <param name="Appid"></param>
        /// <param name="Appsecret"></param>
        /// <param name="Code">回调页面带的code参数</param>
        /// <returns>微信用户唯一标识openid</returns>
        public string CodeGetOpenid(string Appid, string Appsecret, string Code)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", Appid, Appsecret, Code);
            string ReText = CommonMethod.WebRequestPostOrGet(url, "");//post/get方法获取信息 
            Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(ReText);
            if (!DicText.ContainsKey("openid"))
                return "";
            return DicText["openid"].ToString();
        }

        /// <summary>
        ///用code换取获取用户信息（包括非关注用户的）
        /// </summary>
        /// <param name="Appid"></param>
        /// <param name="Appsecret"></param>
        /// <param name="Code">回调页面带的code参数</param>
        /// <returns>获取用户信息（json格式）</returns>
        public string GetUserInfo(string Appid, string Appsecret, string Code)
        {
            string url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", Appid, Appsecret, Code);
            string ReText = CommonMethod.WebRequestPostOrGet(url, "");//post/get方法获取信息
            Dictionary<string, object> DicText = (Dictionary<string, object>)Jss.DeserializeObject(ReText);
            if (!DicText.ContainsKey("openid"))
            {
                CommonMethod.WriteTxt("获取openid失败，错误码：" + DicText["errcode"].ToString());
                return "";
            }
            else
            {
                return CommonMethod.WebRequestPostOrGet("https://api.weixin.qq.com/sns/userinfo?access_token=" + DicText["access_token"] + "&openid=" + DicText["openid"] + "&lang=zh_CN", "");
            }
        }
    }
}
