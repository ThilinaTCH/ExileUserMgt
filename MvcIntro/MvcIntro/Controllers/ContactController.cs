using System.Collections.Generic;
using System.Web.Mvc;
using MvcIntro.Models;

namespace MvcIntro.Controllers
{
    public class ContactController : Controller
    {
        private ContactRepo repo=new ContactRepo();

        public ContactController()
        {
        }

        public ContactController(ContactRepo rpo)
        {
            repo = rpo;
        }
        Contact usr=new Contact();
        //
        // GET: /Movies/

        public ActionResult Index()
        {
            var model = repo.UserList();
            return View(model);
        }

        //
        // GET: /Movies/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult DeleteAll()
        {
            repo.DeleteAll();
            return RedirectToAction("Index");
        }

        //
        // GET: /Movies/Create

        public ActionResult Create()
        {
            return View(new Contact());
        }

        //
        // POST: /Movies/Create

        [HttpPost]
        public ActionResult Create(Contact newUser)
        {
            if (ModelState.IsValid)
            {
                repo.AddUser(newUser);
                return RedirectToAction("Index");
            }
            return View(newUser);
        }


        //
        // GET: /Movies/Edit/5

        public ActionResult Edit(int id)
        {
            usr = repo.GetUserById(id);
            return View(usr);
        }

        //
        // POST: /Movies/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, Contact newUser)
        {
            if (ModelState.IsValid)
            {
                repo.UpdateUser(id,newUser);
                return RedirectToAction("Index");
            }
            else
            {
                return View(repo.GetUserById(id));
            }
        }

        //
        // GET: /Movies/Delete/5

        public ActionResult Delete(int id)
        {
            usr = repo.GetUserById(id);
            return View(usr);
        }

        //
        // POST: /Movies/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, Contact newUser)
        {
            repo.DeleteUser(id);
            return RedirectToAction("Index");
        }


        //
        // GET: /Movies/Search

        public ActionResult Search()
        {
            var searchList = new ContactSearch();
            return View(searchList);
        }

        //
        // POST: /Movies/Search

        [HttpPost]
        public ActionResult Search(ContactSearch newUser)
        {
            var searchList = new List<Contact>();
            if (ModelState.IsValid)
            {
                searchList=repo.GetSearchedUsers(newUser.SearchQuery);
                newUser.UserList = searchList;
                return View(newUser);
            }
            else
            {
                newUser.UserList = searchList;
                return View(newUser);
            }
        }

    }
}
