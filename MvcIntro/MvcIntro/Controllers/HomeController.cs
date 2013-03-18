using System.Web.Mvc;

namespace MvcIntro.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ExileContactMgt web portal!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
