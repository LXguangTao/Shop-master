using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class SendTelCode
    {
        
        /// <summary>
        /// 发送语音验证码
        /// </summary>
        /// <param name="tel">电话号码</param>
        /// <param name="count">播放次数</param>
        /// <param name="content">验证码内容</param>
        /// <returns></returns>
        public static string SendMsg(string tel,string count,string content)
        {
            string appkey = "ab1af8f388a9f0a505a14167a76f457c";
            string url = "http://op.juhe.cn/yuntongxun/voice";
            var parameters = new SortedDictionary<string, string>();
            parameters.Add("valicode", content); //验证码内容，字母、数字 4-8位
            parameters.Add("to", tel); //接收手机号码
            parameters.Add("playtimes", count); //验证码播放次数，默认3
            parameters.Add("key", appkey);//你申请的key
            parameters.Add("dtype", ""); //返回数据的格式,xml或json，默认json
            string jsonMsg = WebHelper.GetRequestData(parameters, url);//发送
            return jsonMsg;
        }
    }
}
