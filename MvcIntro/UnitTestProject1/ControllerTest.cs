using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FluentAssertions;
using MvcIntro.Controllers;
using MvcIntro.Models;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using SimpleBrowser.WebDriver;

namespace UnitTestProject1
{
    [TestFixture]
    public class ControllerTest
    {
        private static readonly ContactRepo repo = new ContactRepo();
        private readonly ContactController controller = new ContactController(repo);
        private static readonly UserRepo userMgr = new UserRepo();

        private List<Contact> ListUsers()
        {
            var result = controller.Index() as ViewResult;
            return (List<Contact>)result.Model;
        }

        [Test]
        public void ShouldCreateNewUser()
        {
            var newUser = new User();
            newUser.UserName = "Johnson";
            newUser.Password = "123456";
            bool ok = userMgr.CreateUser(newUser);
            ok.Should().BeTrue(); //if user cration is successfull it returns true

        }

        [Test]
        public void SaveAllFields()
        {
            var newUser = new Contact("Mike", "UK");
            repo.AddContact(newUser);

            Contact createdUser = repo.GetContactById(newUser.Id);
            createdUser.Name.Should().Be(newUser.Name);
            createdUser.Address.Should().Be(newUser.Address);
        }

        [Test]
        public void ShouldIncludeAddedContacts()
        {
            //manually login the user before testing this
            logIn();

            var newContact = new Contact("Johnny", "USA");
            repo.AddContact(newContact);

            ListUsers().Should().Contain(p => p.Id == newContact.Id);
        }

        [Test]
        public void UpdateAllFields()
        {
            //manually login the user before testing this
            logIn();

            var oldUser = new Contact("Wills", "Australia");
            repo.AddContact(oldUser);
            var newUser = new Contact("Alice", "Denmark");
            repo.UpdateContact(oldUser.Id, newUser);

            Contact updatedUser = repo.GetContactById(oldUser.Id);

            updatedUser.Name.Should().Be(newUser.Name);
            updatedUser.Address.Should().Be(newUser.Address);
        }

        [Test]
        public void ShouldRemoveDeletedContacts()
        {
            //manually login the user before testing this
            logIn();

            var userToBeDeleted = new Contact("Michelle", "Germany");
            repo.AddContact(userToBeDeleted);

            repo.DeleteContact(userToBeDeleted.Id);

            ListUsers().Any(p => p.Id == userToBeDeleted.Id).Should().BeFalse();
        }

        [Test]
        public void ShouldSaveContact()
        {
            SimpleBrowserDriver browser = logIn();
            //Add user
            browser.Url = "http://localhost:54075/Contact/Create";
            browser.FindElement(By.Id("Name")).SendKeys("Darth Vader");
            browser.FindElement(By.Id("Address")).SendKeys("Death Star");
            browser.FindElement(By.Id("create")).Submit();

            //Search user-existing
            browser.Url = "http://localhost:54075/Contact/Search";
            browser.FindElement(By.Id("SearchQuery")).SendKeys("darth vader");
            browser.FindElement(By.Id("search")).Submit();

            string retrieved = browser.FindElement(By.Id("result00")).Text;
            retrieved.Trim().Should().Be("Darth Vader");

            //Search user-non existing
            browser.Url = "http://localhost:54075/Contact/Search";
            browser.FindElement(By.Id("SearchQuery")).SendKeys("anakin");
            browser.FindElement(By.Id("search")).Submit();

            retrieved = browser.FindElement(By.Id("noResults")).Text;
            retrieved.Trim().Should().Be("No Matches.....");
        }

        private SimpleBrowserDriver logIn()
        {
            var browser = new SimpleBrowserDriver();
            //Create user
            if (userMgr.GetUserByName("Tom") == null)
            {
                browser.Url = "http://localhost:54075/User/Register";
                browser.FindElement(By.Id("UserName")).SendKeys("Tom");
                browser.FindElement(By.Id("Password")).SendKeys("abcdef");
                browser.FindElement(By.Id("ConfirmPassword")).SendKeys("abcdef");
                browser.FindElement(By.Id("register")).Submit();
            }
            else
            {
                browser.Url = "http://localhost:54075/User/LogOn";
                browser.FindElement(By.Id("UserName")).SendKeys("Tom");
                browser.FindElement(By.Id("Password")).SendKeys("abcdef");
                browser.FindElement(By.Id("RememberMe")).Click();
                browser.FindElement(By.Id("logon")).Submit();
            }
            return browser;
        }
    }
}
