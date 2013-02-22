using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentAssertions;
using MvcIntro.Models;
using NUnit.Framework;
using MvcIntro.Controllers;

namespace UnitTestProject1
{
    [TestFixture]
    public class UserControllerTest
    {
        private static UserRepo repo = new UserRepo();
        private UserController controller = new UserController(repo);

        [Test]
        public void ShouldIncludeAddedUsers()
        {
            User newUser = new User("Johnny", "USA");
            repo.AddUser(newUser);

            ListUsers().Any(p => p.Id == newUser.Id).Should().BeTrue();
        }

        [Test]
        public void SaveAllFields()
        {
            User newUser = new User("Mike", "UK");
            repo.AddUser(newUser);

            User createdUser = repo.GetUserById(newUser.Id);
            createdUser.Name.Should().Be(newUser.Name);
            createdUser.Address.Should().Be(newUser.Address);
        }

        [Test]
        public void ShouldRemoveDeletedUsers()
        {
            User userToBeDeleted = new User("Michelle", "Germany");
            repo.AddUser(userToBeDeleted);

            repo.DeleteUser(userToBeDeleted.Id);

            ListUsers().Any(p => p.Id == userToBeDeleted.Id).Should().BeFalse();
        }

        private List<User> ListUsers()
        {
            ViewResult result = controller.Index() as ViewResult;
            return (List<User>)result.Model;
        }        

        [Test]
        public void UpdateAllFields()
        {
            User oldUser = new User("Wills", "Australia");
            repo.AddUser(oldUser);
            User newUser = new User("Alice", "Denmark");
            repo.UpdateUser(oldUser.Id, newUser);

            User updatedUser = repo.GetUserById(oldUser.Id);

            updatedUser.Name.Should().Be(newUser.Name);
            updatedUser.Address.Should().Be(newUser.Address);
        }       
    }
}
