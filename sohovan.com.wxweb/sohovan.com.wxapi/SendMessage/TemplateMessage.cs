using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace sohovan.com.wxapi.SendMessage
{
    public class TemplateMessage
    {
        JavaScriptSerializer Jss = new JavaScriptSerializer();
        /// <summary>
        /// 给指定的用户发送模板消息
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSecret"></param>
        /// <param name="openId">用户标识openid</param>
        /// <param name="templateId">对应的模板id</param>
        /// <param name="data">对应模板的参数</param>
        /// <param name="url">点击对应消息弹出的地址</param>
        /// <param name="topcolor">颜色</param>
        /// <returns>返回json数据包</returns>
        public string SendTemplate(string AppID, string AppSecret, string openId, string templateId, object data, string url, string topcolor = "#173177")
        {
            var msgData = new 
            {
                touser = openId,
                template_id = templateId,
                topcolor = topcolor,
                url = url,
                data = data
            };
            string postData = Jss.Serialize(msgData);
            return CommonMethod.WebRequestPostOrGet("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + BasicApi.GetTokenSession(AppID, AppSecret), postData);
        }
    }
}
