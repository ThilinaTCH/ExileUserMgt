using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcIntro.Models;

namespace MvcIntro.Controllers
{
    public class ContactController : Controller
    {
        private UserRepo userRepo = new UserRepo();
        User user = new User();
        Contact conatact = new Contact();

        HttpCookie aCookie;
        string UserName;

        public ContactController()
        {
        }

        public ContactController(UserRepo rpo)
        {
            userRepo = rpo;
        }
        //
        // GET: /Contact/

        public ActionResult Index()
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                UserName = Server.HtmlEncode(aCookie.Value);
                user = userRepo.GetUserByName(UserName);
                var model = user.ContactsList;
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Contact/DeleteAll

        public ActionResult DeleteAll()
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                UserName = Server.HtmlEncode(aCookie.Value);
                user = userRepo.GetUserByName(UserName);
                user.ContactsList=new List<Contact>();
                userRepo.UpdateUser(user);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Contact/Create

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
        // POST: /Contact/Create

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
                    
                    user.ContactsList.Add(newContact);
                    userRepo.UpdateUser(user);
                    return RedirectToAction("Index");
                }
                return View(newContact);
            }
            return RedirectToAction("Index", "Home");
        }


        //
        // GET: /Contact/Edit/5

        public ActionResult Edit(int id)
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                UserName = Server.HtmlEncode(aCookie.Value);
                user = userRepo.GetUserByName(UserName);
                conatact = userRepo.GetUserContactById(user,id);
                return View(conatact);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Contact/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Contact newContact)
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                UserName = Server.HtmlEncode(aCookie.Value);
                user = userRepo.GetUserByName(UserName);
                if (ModelState.IsValid)
                {
                    var contactList = user.ContactsList;
                    contactList.Remove(contactList.SingleOrDefault(x => x.Id == newContact.Id));
                    user.ContactsList.Add(newContact);
                    userRepo.UpdateUser(user);
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(userRepo.GetUserContactById(user,id));
                }
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Contact/Delete/5

        public ActionResult Delete(int id)
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                UserName = Server.HtmlEncode(aCookie.Value);
                user = userRepo.GetUserByName(UserName);
                conatact = userRepo.GetUserContactById(user,id);
                return View(conatact);
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // POST: /Contact/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, Contact newContact)
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                UserName = Server.HtmlEncode(aCookie.Value);
                user = userRepo.GetUserByName(UserName);
                var contactList = user.ContactsList;
                contactList.Remove(contactList.SingleOrDefault(x => x.Id == newContact.Id)); 

                userRepo.UpdateUser(user);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");
        }


        //
        // GET: /Contact/Search

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
        // POST: /Contact/Search

        [HttpPost]
        public ActionResult Search(ContactSearch newContact)
        {
            aCookie = Request.Cookies["loginCookie"];
            if (aCookie != null)
            {
                var searchList = new List<Contact>();
                if (ModelState.IsValid)
                {
                    UserName = Server.HtmlEncode(aCookie.Value);
                    user = userRepo.GetUserByName(UserName);
                    searchList = userRepo.GetSearchedUserContacts(user,newContact.SearchQuery);
                    newContact.UserList = searchList;
                    return View(newContact);
                }
                else
                {
                    newContact.UserList = searchList;
                    return View(newContact);
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
