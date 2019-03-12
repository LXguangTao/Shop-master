using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web.Models;

namespace web.Controllers
{
    public class FormTestController : Controller
    {
        // GET: FormTest
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EditUser()
        {
            User u = new Models.User { Id = 100, Birthday = DateTime.Parse("2000/1/1"), Name = "曹玫", Password = "123@qwe", Remark = "大家好，我是<b>小草莓</b>" };
            return View(u);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditUser(User tm, HttpPostedFileBase Photo)
        {
            return Json(tm);
        }
    }
}