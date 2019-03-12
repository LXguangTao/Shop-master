using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Utilities
{
    /// <summary>
    /// 邮件发送的帮助类
    /// </summary>
    public class NetHelper
    {      

        #region 下载公开网页资源
        /// <summary>
        /// 下载指定路径的网页内容
        /// </summary>
        /// <param name="url">网页路径，可以是html、css、js等</param>
        /// <returns></returns>
        public static string DownContent(string url)
        {
            try
            {
                WebClient client = new WebClient();
                Stream strm = client.OpenRead(url);
                using (StreamReader sr = new StreamReader(strm))
                {
                    return sr.ReadToEnd();
                }
            }
            catch (Exception exp)
            {
                LogHelper.Default.Error("{ time:'" + DateTime.Now + "',msg:'" + exp.Message + exp.StackTrace + "'}");
            }
            return "";
        }
        #endregion

        #region 发送电子邮件

        /// <summary> 
        /// 发送邮件程序 
        /// </summary> 
        /// <param name="from">发送人邮件地址</param> 
        /// <param name="fromName">发送人显示名称</param> 
        /// <param name="to">发送给谁（邮件地址）</param> 
        /// <param name="subject">标题</param> 
        /// <param name="body">内容</param> 
        /// <param name="username">邮件登录名</param> 
        /// <param name="password">邮件密码</param> 
        /// <param name="server">邮件服务器,例如：smtp.126.com </param> 
        /// <param name="fujian">附件,例如： C:\\a.txt;D:\\b.rar</param> 
        /// <param name="port">端口,例如：25</param> 
        /// <returns>是否发送成功</returns> 
        public static bool SendEMail(string from, string fromName, string to, string subject, string body,
            string username, string password, string server, string fujian, int port = 25)
        {
            try
            {
                MailMessage mail = new MailMessage(); //创建邮件信息对象                
                mail.From = new MailAddress(from, fromName); //是谁发送的邮件 
                mail.To.Add(to);//发送给谁
                mail.Subject = subject;//标题               
                mail.BodyEncoding = Encoding.UTF8;//内容编码
                mail.Priority = MailPriority.High;//发送优先级
                mail.Body = body;//邮件内容 
                mail.IsBodyHtml = false;//是否HTML形式发送 
                //附件 
                string[] fjs = fujian.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if(fjs !=null && fjs.Length > 0)
                {
                    for (int i = 0; i < fjs.Length; i++)
                        mail.Attachments.Add(new Attachment(fjs[i]));
                }
                SmtpClient smtp = new SmtpClient(server, port); //smtp邮件服务器和端口                
                smtp.UseDefaultCredentials = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;//指定发送方式 
                smtp.Credentials = new System.Net.NetworkCredential(username, password);//指定登录名和密码 
                smtp.Timeout = 10000;//超时时间 
                smtp.Send(mail);
                return true;
            }
            catch (SmtpException exp)
            {
                LogHelper.Default.Error("{ time:'" + DateTime.Now + "',msg:'" + exp.Message + exp.StackTrace + "'}");
                return false;
            }
            catch (Exception exp)
            {
                LogHelper.Default.Error("{ time:'"+ DateTime.Now+"',msg:'" + exp.Message+exp.StackTrace+"'}");
                return false;
            }
        }

        #endregion
    }
}
