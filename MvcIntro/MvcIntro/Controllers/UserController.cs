using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcIntro.Models;

namespace MvcIntro.Controllers
{
    public class UserController : Controller
    {
        private UserRepo repo = new UserRepo();

        public UserController()
        {
        }

        public UserController(UserRepo rpo)
        {
            repo = rpo;
        }
        User usr=new User();
        //
        // GET: /User/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /User/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (repo.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                    HttpCookie myCookie = new HttpCookie("loginCookie");
                    // Set the cookie value.
                    myCookie.Value = model.UserName;
                    Response.Cookies.Add(myCookie);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /User/LogOff

        public ActionResult LogOff()
        {
            HttpCookie currentUserCookie = HttpContext.Request.Cookies["loginCookie"];
            HttpContext.Response.Cookies.Remove("loginCookie");
            currentUserCookie.Expires = DateTime.Now.AddDays(-10);
            currentUserCookie.Value = null;
            HttpContext.Response.SetCookie(currentUserCookie);

            FormsAuthentication.SignOut();            
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /User/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /User/Register

        [HttpPost]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                bool stat=repo.CreateUser(model);

                if (stat)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);

                    HttpCookie myCookie = new HttpCookie("loginCookie");
                    // Set the cookie value.
                    myCookie.Value = model.UserName;
                    Response.Cookies.Add(myCookie);

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "User name already exists. Please enter a different user name.");
            }

            return View(model);
        }
    }
}
