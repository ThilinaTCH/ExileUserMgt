using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentAssertions;
using MvcIntro.Controllers;
using MvcIntro.Models;
using NUnit.Framework;
using OpenQA.Selenium;
using SimpleBrowser.WebDriver;

namespace UnitTestProject1
{
    [TestFixture]
    public class UserControllerTest
    {
        private static readonly ContactRepo repo = new ContactRepo();
        private readonly ContactController controller = new ContactController(repo);

        private List<Contact> ListUsers()
        {
            var result = controller.Index() as ViewResult;
            return (List<Contact>) result.Model;
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
        public void ShouldIncludeAddedUsers()
        {
            var newUser = new Contact("Johnny", "USA");
            repo.AddContact(newUser);

            ListUsers().Should().Contain(p => p.Id == newUser.Id);
        }

        [Test]
        public void UpdateAllFields()
        {
            var oldUser = new Contact("Wills", "Australia");
            repo.AddContact(oldUser);
            var newUser = new Contact("Alice", "Denmark");
            repo.UpdateContact(oldUser.Id, newUser);

            Contact updatedUser = repo.GetContactById(oldUser.Id);

            updatedUser.Name.Should().Be(newUser.Name);
            updatedUser.Address.Should().Be(newUser.Address);
        }

        [Test]
        public void ShouldRemoveDeletedUsers()
        {
            var userToBeDeleted = new Contact("Michelle", "Germany");
            repo.AddContact(userToBeDeleted);

            repo.DeleteContact(userToBeDeleted.Id);

            ListUsers().Any(p => p.Id == userToBeDeleted.Id).Should().BeFalse();
        }

        [Test]
        public void ShouldSavePerson()
        {
            var browser = new SimpleBrowserDriver();

            //Add user
            browser.Url = "http://localhost:54075/User/Create";
            browser.FindElement(By.Id("Name")).SendKeys("Darth Vader");
            browser.FindElement(By.Id("Address")).SendKeys("Death Star");
            browser.FindElement(By.Id("create")).Submit();

            //Search user-existing
            browser.Url = "http://localhost:54075/User/Search";
            browser.FindElement(By.Id("SearchQuery")).SendKeys("darth vader");
            browser.FindElement(By.Id("search")).Submit();

            string retrieved = browser.FindElement(By.Id("result00")).Text;
            retrieved.Trim().Should().Be("Darth Vader");

            //Search user-non existing
            browser.Url = "http://localhost:54075/User/Search";
            browser.FindElement(By.Id("SearchQuery")).SendKeys("anakin");
            browser.FindElement(By.Id("search")).Submit();

            retrieved = browser.FindElement(By.Id("noResults")).Text;
            retrieved.Trim().Should().Be("No Matches.....");
        }
    }
}
