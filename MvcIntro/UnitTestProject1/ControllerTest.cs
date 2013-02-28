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
        private static readonly UserRepo userMgr = new UserRepo();

        private List<Contact> ListUsers()
        {
            //to get the contact list of relevent user he needs to login
            createUsr("Jill");
            var user = userMgr.GetUserByName("Jill");
            return repo.ContactList(user.UId);
        }

        [Test]
        public void ShouldCreateNewUser()
        {
            var newUser = new User();
            newUser.UserName = "Johnso";
            newUser.Password = "123456";
            bool ok = userMgr.CreateUser(newUser);
            if (userMgr.GetUserByName("Johnso") == null)
            {
                ok.Should().BeTrue(); //if user cration is successfull it returns true
            }
            else
            {
                ok.Should().BeFalse();
            }

        }

        private void createUsr(string username)
        {
            var newUser = new User();
            newUser.UserName = username;
            newUser.Password = "123456";
            userMgr.CreateUser(newUser);
        }
        [Test]
        public void SaveAllFields()
        {
            createUsr("Jill");
            var user=userMgr.GetUserByName("Jill");
            var newContact = new Contact("Mike", "UK",user.UId);
            repo.AddContact(newContact);

            Contact createdUser = repo.GetContactById(newContact.Id);
            createdUser.Name.Should().Be(newContact.Name);
            createdUser.Address.Should().Be(newContact.Address);
        }

        [Test]
        public void ShouldIncludeAddedContacts()
        {
            createUsr("Jill");
            var user = userMgr.GetUserByName("Jill");
            var newContact = new Contact("Johnny", "USA",user.UId);
            repo.AddContact(newContact);

            ListUsers().Should().Contain(p => p.Id == newContact.Id);
        }

        [Test]
        public void UpdateAllFields()
        {
            createUsr("Jill");
            var user = userMgr.GetUserByName("Jill");
            var oldContact = new Contact("Wills", "Australia",user.UId);
            repo.AddContact(oldContact);
            var newContact = new Contact("Alice", "Denmark",user.UId);
            repo.UpdateContact(oldContact.Id, newContact);

            Contact updatedUser = repo.GetContactById(oldContact.Id);

            updatedUser.Name.Should().Be(newContact.Name);
            updatedUser.Address.Should().Be(newContact.Address);
        }

        [Test]
        public void ShouldRemoveDeletedContacts()
        {
            createUsr("Jill");
            var user = userMgr.GetUserByName("Jill");
            var contactToBeDeleted = new Contact("Michelle", "Germany",user.UId);
            repo.AddContact(contactToBeDeleted);

            repo.DeleteContact(contactToBeDeleted.Id);

            ListUsers().Any(p => p.Id == contactToBeDeleted.Id).Should().BeFalse();
        }

        [Test]
        public void ShouldSaveContact()
        {
            var browser = new SimpleBrowserDriver();
            //Create user
            if (userMgr.GetUserByName("Wills") == null)
            {
                browser.Url = "http://localhost:54075/User/Register";
                browser.FindElement(By.Id("UserName")).SendKeys("Wills");
                browser.FindElement(By.Id("Password")).SendKeys("abcdef");
                browser.FindElement(By.Id("ConfirmPassword")).SendKeys("abcdef");
                browser.FindElement(By.Id("register")).Submit();
            }
            else
            {
                browser.Url = "http://localhost:54075/User/LogOn";
                browser.FindElement(By.Id("UserName")).SendKeys("Wills");
                browser.FindElement(By.Id("Password")).SendKeys("abcdef");
                browser.FindElement(By.Id("RememberMe")).Click();
                browser.FindElement(By.Id("logon")).Submit();
            }
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
    }
}
