using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MvcIntro.Models;

namespace MvcIntro.Controllers
{
    public class ContactController : Controller
    {
        private ContactRepo repo = new ContactRepo();
        Contact conatact = new Contact();

        private UserRepo userRepo = new UserRepo();
        User user = new User();

        HttpCookie aCookie;
        string UserName;

        public ContactController()
        {
        }

        public ContactController(ContactRepo rpo)
        {
            repo = rpo;
        }
        //
        // GET: /Movies/

        public ActionResult Index()
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                UserName = Server.HtmlEncode(aCookie.Value);
                user = userRepo.GetUserByName(UserName);
                var model = repo.ContactList(user.UId);
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }


        public ActionResult DeleteAll()
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                UserName = Server.HtmlEncode(aCookie.Value);
                user = userRepo.GetUserByName(UserName);
                repo.DeleteAll(user.UId);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Movies/Create

        public ActionResult Create()
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                return View(new Contact());
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Movies/Create

        [HttpPost]
        public ActionResult Create(Contact newContact)
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {

                if (ModelState.IsValid)
                {
                    UserName = Server.HtmlEncode(aCookie.Value);
                    user = userRepo.GetUserByName(UserName);
                    
                    newContact.UserId = user.UId;
                    newContact.User = user;
                    repo.AddContact(newContact);
                    return RedirectToAction("Index");
                }
                return View(newContact);
            }
            return RedirectToAction("Index", "Home");
        }


        //
        // GET: /Movies/Edit/5

        public ActionResult Edit(int id)
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                conatact = repo.GetContactById(id);
                return View(conatact);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Movies/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Contact newContact)
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                if (ModelState.IsValid)
                {
                    UserName = Server.HtmlEncode(aCookie.Value);
                    user = userRepo.GetUserByName(UserName);
                    newContact.UserId = user.UId;
                    newContact.User = user;
                    repo.UpdateContact(id, newContact);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(repo.GetContactById(id));
                }
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Movies/Delete/5

        public ActionResult Delete(int id)
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                conatact = repo.GetContactById(id);
                return View(conatact);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Movies/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, Contact newUser)
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                repo.DeleteContact(id);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");
        }


        //
        // GET: /Movies/Search

        public ActionResult Search()
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                var searchList = new ContactSearch();
                return View(searchList);
            }
            return RedirectToAction("Index", "Home");

        }

        //
        // POST: /Movies/Search

        [HttpPost]
        public ActionResult Search(ContactSearch newUser)
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                var searchList = new List<Contact>();
                if (ModelState.IsValid)
                {
                    UserName = Server.HtmlEncode(aCookie.Value);
                    user = userRepo.GetUserByName(UserName);
                    searchList = repo.GetSearchedContacts(user.UId,newUser.SearchQuery);
                    newUser.UserList = searchList;
                    return View(newUser);
                }
                else
                {
                    newUser.UserList = searchList;
                    return View(newUser);
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
