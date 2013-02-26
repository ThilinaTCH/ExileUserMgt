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
        private static readonly UserRepo repo = new UserRepo();
        private readonly UserController controller = new UserController(repo);

        private List<User> ListUsers()
        {
            var result = controller.Index() as ViewResult;
            return (List<User>) result.Model;
        }

        [Test]
        public void SaveAllFields()
        {
            var newUser = new User("Mike", "UK");
            repo.AddUser(newUser);

            User createdUser = repo.GetUserById(newUser.Id);
            createdUser.Name.Should().Be(newUser.Name);
            createdUser.Address.Should().Be(newUser.Address);
        }

        [Test]
        public void ShouldIncludeAddedUsers()
        {
            var newUser = new User("Johnny", "USA");
            repo.AddUser(newUser);

            ListUsers().Any(p => p.Id == newUser.Id).Should().BeTrue();
        }

        [Test]
        public void ShouldRemoveDeletedUsers()
        {
            var userToBeDeleted = new User("Michelle", "Germany");
            repo.AddUser(userToBeDeleted);

            repo.DeleteUser(userToBeDeleted.Id);

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

            //Search user
            browser.Url = "http://localhost:54075/User/Search";
            browser.FindElement(By.Id("SearchQuery")).SendKeys("darth vader");
            browser.FindElement(By.Id("search")).Submit();

            string retrieved = browser.FindElement(By.Id("result00")).Text;
            retrieved.Trim().Should().Be("Darth Vader");
        }

        [Test]
        public void UpdateAllFields()
        {
            var oldUser = new User("Wills", "Australia");
            repo.AddUser(oldUser);
            var newUser = new User("Alice", "Denmark");
            repo.UpdateUser(oldUser.Id, newUser);

            User updatedUser = repo.GetUserById(oldUser.Id);

            updatedUser.Name.Should().Be(newUser.Name);
            updatedUser.Address.Should().Be(newUser.Address);
        }
    }
}
