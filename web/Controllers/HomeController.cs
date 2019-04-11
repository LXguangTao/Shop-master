using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Utilities;
using web.Models;

namespace web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [HandleError(ExceptionType = typeof(System.Exception),View ="Error")]
        [OutputCache(Duration =5)]
        [LogExceptionFilter]
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult GetExcelData()
        {
            string url = Server.MapPath("~/Content/test.xlsx");
            
           //StringBuilder sb = new StringBuilder("<table><thead>");
            DataTable dt = ExcelHelper.ExcelToDataTable(url,"商品",true);
            //foreach (DataColumn t in dt.Columns)
            //{
            //    sb.AppendFormat("<th>{0}</th>", t.ColumnName);
            //}
            //sb.AppendFormat("</thead><tbody>");
            return View(dt);
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="un"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateUser(String un, HttpPostedFileBase up)
        {
            List<string> FileType = new List<string>()
            {
                "jpg","jpeg","png","gif","svg"
            };
            string upstring = up.FileName;
            string upname = DateTime.Now.ToString("yyyyMMddhhmmss");
            string type = upstring.Substring(upstring.LastIndexOf(".")+1).ToLower();
            var fileName = Path.Combine(Request.MapPath("~/Content/"));
            try
            {
                if (up.ContentLength> 1024 *1024)
                {
                    return Json(new { success=false,message="文件大于1MB"});
                }
                int i = 0;
                foreach (var item in FileType)
                {
                    if (type == item)
                    {
                        up.SaveAs(fileName + upname + "." + type);
                    }
                    else
                    {
                        i++;
                    }
                }
                if (i==FileType.Count())
                {
                    return Json(new { success=false,message="不是图片文件"});
                }
                string d = string.Format("用户【{0}】的照片已经保存成功",un);
                return Json(new { success=true,message=d,remark="/Content/"+upname+type,s=up.FileName});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message, remark = "" });
            }
        }
        public ActionResult EchartData()
        {
            List<string> categories = new List<string>() {
               "衬衫","羊毛衫","雪纺衫","裤子","高跟鞋","袜子"
            };
            List<int> data = new List<int>() {
                5, 20, 36, 10, 10, 20
            };
            return Json(new { categories=categories,data=data},JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SendMessage()
        {
            string who = Request.Params["who"];
            string msg = SendTelCode.SendMsg(who,"","1234");
            return Content(msg);
        }
    }
}