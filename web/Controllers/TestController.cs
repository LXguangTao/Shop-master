using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;

namespace web.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Log()
        {
            LogHelper.Default.Trace("这是Test/Log记录Trace等级的信息");
            LogHelper.Default.Debug("这是Test/Log记录Debug等级的信息");
            LogHelper.Default.Info("这是Test/Log记录Info等级的信息");
            LogHelper.Default.Warn("这是Test/Log记录Warn等级的信息");
            LogHelper.Default.Error("这是Test/Log记录Error等级的信息");
            LogHelper.Default.Fatal("这是Test/Log记录Fatal等级的信息");
            return Content("记录完毕");
        }

        public ActionResult LogExp1()
        {
            try
            {
                int[] arr = { 10, 12 };
                int a = arr[3];
            }
            catch (Exception ex)
            {
                LogHelper.Default.Debug(ex.StackTrace.ToString(), ex);
                LogHelper.Default.Error(ex.StackTrace.ToString(), ex);
                LogHelper.Default.Fatal(ex.StackTrace.ToString(), ex);
            }
            return Content("记录了异常");
        }
        public ActionResult LogExp2()
        {
            LogHelper logger = new LogHelper("logAll");
            try
            {
                int a = 10, b = 0;
                int c = a / b;
            }
            catch (Exception ex)
            {
                logger.Debug(ex.StackTrace.ToString(), ex);
                logger.Error(ex.StackTrace.ToString(), ex);
                logger.Fatal(ex.StackTrace.ToString(), ex);
            }
            return Content("记录了异常");
        }
    }
}