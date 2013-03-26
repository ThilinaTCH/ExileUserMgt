using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MvcIntro.Models;
using NUnit.Framework;
using OpenQA.Selenium;
using SimpleBrowser.WebDriver;

namespace UnitTestProject1
{
    [TestFixture]
    public class ControllerTest
    {
        private static readonly UserRepo userMgr = new UserRepo();

        private IEnumerable<Contact> ListUsers()
        {
            //to get the contact list of relevent user he needs to login
            createUsr("Jill");
            var user = userMgr.GetUserByName("Jill");
            return user.ContactsList;
        }

        [Test]
        public void ShouldCreateNewUser()
        {
            var newUser = new User();
            newUser.UserName = "Johnso";
            newUser.Password = "123456";
            bool ok;
            if (userMgr.GetUserByName("Johnso") == null)
            {
                ok = userMgr.CreateUser(newUser);
                ok.Should().BeTrue(); //if user cration is successfull it returns true
            }
            else
            {
                ok = userMgr.CreateUser(newUser);
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
            var user = userMgr.GetUserByName("Jill");
            var newContact = new Contact("Mike", "UK");
            user.ContactsList.Add(newContact);
            userMgr.UpdateUser(user);

            Contact createdUser = userMgr.GetUserContactById(newContact.Id);
            createdUser.Name.Should().Be(newContact.Name);
            createdUser.Address.Should().Be(newContact.Address);
        }

        [Test]
        public void ShouldIncludeAddedContacts()
        {
            createUsr("Jill");
            var user = userMgr.GetUserByName("Jill");
            var newContact = new Contact("Johnny", "USA");
            user.ContactsList.Add(newContact);
            userMgr.UpdateUser(user);
            ListUsers().Any(p => p.Id == newContact.Id).Should().BeTrue();
        }

        [Test]
        public void UpdateAllFields()
        {
            createUsr("Jill");
            var user = userMgr.GetUserByName("Jill");
            var oldContact = new Contact("Wills", "Australia");
            user.ContactsList.Add(oldContact);

            var newContact = new Contact("Alice", "Denmark");

            var contactList = user.ContactsList;
            contactList.Remove(contactList.SingleOrDefault(x => x.Id == oldContact.Id));
            user.ContactsList.Add(newContact);
            userMgr.UpdateUser(user);

            var newUser = userMgr.GetUserByName("Jill");
            Contact updatedContact = userMgr.GetUserContactById(newContact.Id);

            updatedContact.Name.Should().Be(newContact.Name);
            updatedContact.Address.Should().Be(newContact.Address);
        }

        [Test]
        public void ShouldRemoveDeletedContacts()
        {
            createUsr("Jill");
            var user = userMgr.GetUserByName("Jill");
            var contactToBeDeleted = new Contact("Michelle", "Germany");
            user.ContactsList.Add(contactToBeDeleted);

            user.ContactsList.Remove(contactToBeDeleted);
            userMgr.UpdateUser(user);

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
