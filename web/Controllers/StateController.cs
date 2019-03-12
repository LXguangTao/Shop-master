using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class StateController : Controller
    {
        // GET: State
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CookieWrite()
        {
            HttpCookie c1 = new HttpCookie("movies", HttpUtility.UrlEncode("海绵宝宝,熊出没"));//键值对，汉字内容会出现乱码，可以通过HttpUtility.UrlEncode来进行编码
            c1.Expires = DateTime.Now.AddDays(7);//写此行代码，表示为持久性cookie，注释掉表示为 会话cookie               

            HttpCookie c2 = new HttpCookie("ids");//键
            c2.Value = "10,15"; //值

            HttpCookie c3 = new HttpCookie("musics", HttpUtility.UrlEncode("海草,沙漠骆驼"));
            c3.Expires = DateTime.Now.AddDays(-1);//设置过期时间为昨天，已经过期，会发到客户端，但是 客户端 不会在下次访问时 带回到服务器端

            HttpCookie c4 = new HttpCookie("heros");//键
            c4.Values.Add(HttpUtility.UrlEncode("及时雨"), HttpUtility.UrlEncode("宋江"));//值列表(列表对象也是键值对)
            c4.Values.Add(HttpUtility.UrlEncode("晁天王"), HttpUtility.UrlEncode("晁盖"));//值列表(列表对象也是键值对)

            Response.Cookies.Add(c1);
            Response.Cookies.Add(c2);
            Response.Cookies.Add(c3);
            Response.Cookies.Add(c4);

            return Content("写入成功");
        }

        public ActionResult CookieShow()
        {
            StringBuilder sb = new StringBuilder();
            //读取cookie
            if (Request.Cookies["movies"] != null)
                sb.Append("movies值：" + HttpUtility.UrlDecode((Request.Cookies["movies"].Value))); //汉字内容会出现乱码，可以通过HttpUtility.UrlDecode来进行解码
            if (Request.Cookies["ids"] != null)
                sb.Append("ids值：" + HttpUtility.UrlDecode(Request.Cookies["ids"].Value));
            if (Request.Cookies["musics"] != null)
                sb.Append("musics值：" + HttpUtility.UrlDecode(Request.Cookies["musics"].Value));

            if (Request.Cookies["heros"] != null)
            {
                HttpCookie item = Request.Cookies["heros"];
                sb.Append("heros:");
                foreach (var d in item.Values.AllKeys)
                {
                    sb.Append("key:" + HttpUtility.UrlDecode(d));
                    sb.Append("value:" + HttpUtility.UrlDecode(item.Values[d]));
                }
            }
            return Content(sb.ToString());
        }

        public ActionResult SessionState()
        {

            object s = Session["movies"] = "asdf";
            HttpContext.Application.Lock();
            object t = HttpContext.Application["a"] = "s";
            HttpContext.Application.UnLock();
            return Content(s.ToString()+t.ToString());
        }
    }
}