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
            string a = null;
            string b;
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult GetExcelData()
        {
            string url = Server.MapPath("~/Content/test.xlsx");
            DataSet ds = ExcelHelper.ImportDataFromExcell(url);
            StringBuilder sb = new StringBuilder("<table><thead>");
            DataTable dt = ExcelHelper.ExcelToDataTable(url,"商品",true);
            foreach (DataColumn t in dt.Columns)
            {
                sb.AppendFormat("<th>{0}</th>",t.ColumnName);
            }
            sb.AppendFormat("</thead><tbody>");
            foreach (DataRow w in dt.Rows)
            {
                sb.AppendFormat("<tr>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sb.AppendFormat("<td>{0}</td>",w[i]);
                }
                sb.AppendFormat("</tr>");
            }
            sb.AppendFormat("</tbody></table>");
            return Content(sb.ToString());
        }
        [HttpPost]
        public ActionResult CreateUser(String un, HttpPostedFileBase up)
        {
            string upstring = up.FileName;            
            string upname = DateTime.Now.ToString("yyyyMMddhhmmss");
            string type = upstring.Substring(upstring.LastIndexOf("."));
            var fileName = Path.Combine(Request.MapPath("~/Content/"));
            try
            {
                up.SaveAs(fileName+upname+type);
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
            char i = char.Parse(Request.Params["i"]);
            string appkey = "ab1af8f388a9f0a505a14167a76f457c";
            string url = "http://op.juhe.cn/yuntongxun/voice";
            var parameters = new SortedDictionary<string, string>();
            parameters.Add("valicode", StringHelper.Strings(i,4)); //验证码内容，字母、数字 4-8位
            parameters.Add("to", "17752554413"); //接收手机号码
            parameters.Add("playtimes", "3"); //验证码播放次数，默认3
            parameters.Add("key", appkey);//你申请的key
            parameters.Add("dtype", ""); //返回数据的格式,xml或json，默认json
            string jsonMsg = WebHelper.GetRequestData( parameters, url);//发送
            return Json(new { data = jsonMsg});
        }
    }
}