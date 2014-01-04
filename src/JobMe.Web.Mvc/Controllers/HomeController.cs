using JobMe.Web.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobMe.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Certificates()
        {
            return View();
        }
        public ActionResult Sync()
        {
            GlobalFilters.Filters.Add(SyncAttribute.Instance);
            return View();
        }
        public ActionResult Unsync()
        {
            GlobalFilters.Filters.Remove(SyncAttribute.Instance);
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize]
        public ActionResult Chat()
        {
            ViewBag.Message = "Your chat page.";

            return View();
        }
    }
}