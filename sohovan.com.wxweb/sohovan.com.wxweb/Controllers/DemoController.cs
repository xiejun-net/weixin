using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sohovan.com.wxapi;
using sohovan.com.wxapi.UserApi;
using System.Web.Script.Serialization;

namespace sohovan.com.wxweb.Controllers
{
    public class DemoController : Controller
    {
        Oauth2 Oauth2 = new Oauth2();
        sohovan.com.wxapi.SendMessage.TemplateMessage TemplateMessage = new sohovan.com.wxapi.SendMessage.TemplateMessage();
        JavaScriptSerializer Jss = new JavaScriptSerializer();
        //
        // GET: /Demo/

        public ActionResult Index()
        {
            return View();
        }


        #region 网页授权demo
        public string openid = "openid";
        public string Appid = "Appid";
        public string Appsecret = "Appsecret";
        public string nickname = "测试昵称";
        public string sex = "";
        public string headimgurl = "";
        public string city = "";
        
        public ActionResult Oauth() {
            Session["openid"] = "openid";
            if (Session["openid"] != null)
            {
                openid = Session["openid"].ToString();
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.QueryString["code"]))
                {
                    GetUserInfo();
                }
                else
                {
                    Response.Redirect(Oauth2.GetCodeUrl("Appid", Request.Url.AbsolutePath.ToString(), "snsapi_userinfo"));
                }
            }
            return View();
        }
        //获取用户个人信息
        private void GetUserInfo()
        {
            string UserInfoJson = Oauth2.GetUserInfo("Appid", "Appsecret", Request.QueryString["code"].Trim());//获取到用户信息 Json格式
            Dictionary<string, object> DicJson = (Dictionary<string, object>)Jss.DeserializeObject(UserInfoJson);
            nickname = DicJson["nickname"].ToString();
            openid = DicJson["openid"].ToString();
            Session["openid"] = openid;
            Session.Timeout = 7200;
            sex = DicJson["sex"].ToString();
            headimgurl = DicJson["headimgurl"].ToString();
            city = DicJson["city"].ToString();
        }
        #endregion

        #region 模板消息demo
        //对应参数正确即可发送消息
        public ActionResult TemMessage() {
            var data = new
            {
                keyword1 = new
                {
                    value = "1",
                    color = "#173177"
                },
                keyword2 = new
                {
                    value = "1",
                    color = "#173177"
                },
                keyword3 = new
                {
                    value = "1",
                    color = "#173177"
                }
            };
            TemplateMessage.SendTemplate("", "", "", "", data, "http://mp.weixin.qq.com/", "#173177");
            return View();
        }
        #endregion
    }
}
