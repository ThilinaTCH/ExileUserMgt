using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcIntro.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ExileUserMgt web portal!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
