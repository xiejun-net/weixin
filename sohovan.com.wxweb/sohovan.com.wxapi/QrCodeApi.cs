using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Script.Serialization;

namespace sohovan.com.wxapi
{
    //微信推广支持接口开发
    public class QrCodeApi
    {
        JavaScriptSerializer Jss = new JavaScriptSerializer();
        #region 申请带参数的临时/永久二维码
        /// <summary>
        /// 调用微信接口获取带参数临时二维码的ticket
        /// 使用方法：https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=TICKET
        /// </summary>
        /// <param name="scene_id">二维码带的参数</param>
        /// <returns>json:ticket:换取二维码的凭证，expire_seconds:凭证有效时间，url:二维码解析后的地址。此处返回ticket 否则返回错误码</returns>
        public string GetQrcode(string appid, string appsecret, Int32 scene_id)
        {
            string QrcodeUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";//WxQrcodeAPI接口
            string AccessToken = BasicApi.GetAccessToken(appid, appsecret);//拉取AccessToken
            QrcodeUrl = string.Format(QrcodeUrl, AccessToken);
            string PostJson = "{\"expire_seconds\": 1800, \"action_name\": \"QR_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": " + scene_id + "}}}";
            string ReText = sohovan.com.common.CommonMethod.WebRequestPostOrGet(QrcodeUrl, PostJson);//post提交
            Dictionary<string, object> reDic = (Dictionary<string, object>)Jss.DeserializeObject(ReText);
            if (reDic.ContainsKey("ticket"))
            {
                return reDic["ticket"].ToString();//成功
            }
            else
            {
                return reDic["errcode"].ToString();//返回错误码
            }
        }

        /// <summary>
        /// 调用微信接口获取带参数永久二维码的ticket
        /// 使用方法：https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=TICKET
        /// </summary>
        /// <param name="scene_id">二维码带的参数</param>
        /// <returns>json:ticket:换取二维码的凭证，expire_seconds:凭证有效时间，url:二维码解析后的地址。此处返回ticket 否则返回错误码</returns>
        public string GetQrcode(string appid, string appsecret, string scene_str)
        {
            string QrcodeUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";//WxQrcodeAPI接口
            string AccessToken = BasicApi.GetAccessToken(appid, appsecret);//拉取AccessToken
            QrcodeUrl = string.Format(QrcodeUrl, AccessToken);
            string PostJson = "{\"expire_seconds\": 1800, \"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\": " + scene_str + "}}}";
            string ReText = sohovan.com.common.CommonMethod.WebRequestPostOrGet(QrcodeUrl, PostJson);//此处省略了 WebRequestPostOrGet即为WebHttpRequest发送Post请求
            Dictionary<string, object> reDic = (Dictionary<string, object>)Jss.DeserializeObject(ReText);
            if (reDic.ContainsKey("ticket"))
            {
                return reDic["ticket"].ToString();//成功
            }
            else
            {
                return reDic["errcode"].ToString();//返回错误码
            }
        }
        #endregion
    }
}
