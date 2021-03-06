﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace web.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            var fileName = System.IO.Path.GetFileName(upload.FileName);
            var filePhysicalPath = Server.MapPath("~/Content/" + fileName);//我把它保存在网站根目录的 content 文件夹

            upload.SaveAs(filePhysicalPath);

            var url = "/Content/" + fileName;
            var CKEditorFuncNum = System.Web.HttpContext.Current.Request["CKEditorFuncNum"];

            //上传成功后，我们还需要通过以下的一个脚本把图片返回到第一个tab选项
            return Content("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\");</script>");
        }
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetRandCode(int? id)
        {
            string content = StringHelper.GetRandomNumAndLetters(5);
            byte[] img = ImageHelper.CreateImage(content, 100, 30);
            Session["yzm"] = content;
            HttpCookie yzm = new HttpCookie("yzm",content);
            Response.Cookies.Add(yzm);
            return File(img, "image/jpeg");
        }
        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="yzm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult YanZheng(string yzm)
        {
            string session = Session["yzm"].ToString();
            if (yzm == session && yzm==Request.Cookies["yzm"].Value)
            {
                return Json(new { message ="验证码正确"});
            }
            else
            {
                return Json(new { message ="验证码错误"});
            }
        }
        /// <summary>
        /// 发邮件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendEmail()
        {
            
            try
            {
                
                int times = int.Parse(Request.Params["times"]);
                string content = Request.Params["content"];
                string email = Request.Params["email"];
                Session["email"] = email;
                bool isSend = NetHelper.SendEMail("18037123549@163.com", "李金涛", Session["email"].ToString(), "第" + times + "次发送", content, "18037123549@163.com", "qq1263294262", "smtp.163.com", "", 25);
                if (isSend)
                {
                    return Json(new { success=true});
                }
                return Json(new { success=false});
            }
            catch (Exception ex)
            {
                return Json(new { success=false,msg=ex.Message});
            }
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <returns></returns>        
        public ActionResult GetQRcode(HttpPostedFileBase filename)
        {
            string strcode = Request.Params["code"];
            //var Filename = Server.MapPath("~/Content/"+filename.FileName);
            byte[] code = ImageHelper.CreateQRCode(strcode);
            return File(code,"image/jpeg");
        }
    }
}