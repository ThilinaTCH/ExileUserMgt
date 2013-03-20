using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Criterion;

namespace MvcIntro.Models
{
    public class UserRepo
    {
        public bool CreateUser(User newUser)
        {
            User storingUser = GetUserByName(newUser.UserName);
            bool isExist = false;
            if (storingUser == null)
            {
                var session = NHibernateContext.SesionFactory;
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(newUser);
                    transaction.Commit();
                    isExist = true;
                }
            }
            return isExist;
        }


        public User GetUserByName(String userName)
        {
            List<User> retrievedUser = new List<User>();
            var session = NHibernateContext.SesionFactory;
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    retrievedUser = (List<User>)session.QueryOver<User>().Where(x => x.UserName == userName).List();
                }
                catch (Exception)
                {
                }
                transaction.Commit();
            }
            if (retrievedUser.Count > 0)
            {
                User selected = retrievedUser.First();
                return selected;
            }
            return null;
        }

        public void UpdateUser(User m1)
        {
            var session = NHibernateContext.SesionFactory;
            using (var transaction = session.BeginTransaction())
            {
                session.Merge(m1);

                transaction.Commit();
            }
        }

        public bool ValidateUser(String userName, String passWord)
        {
            User storedUser = null;
            storedUser = GetUserByName(userName);
            if (storedUser != null && storedUser.Password == passWord)
                return true;
            else return false;
        }

        public Contact GetUserContactById(User use, int id)
        {
            IList<Contact> Contact = new List<Contact>();

            var session = NHibernateContext.SesionFactory;
            using (var transaction = session.BeginTransaction())
            {
                Contact = session.Get<User>(use.UId).ContactsList;
                transaction.Commit();
            }
            return Contact.First(x => x.Id == id);
        }

        public List<Contact> GetSearchedUserContacts(User user, string searchQuery)
        {
            var result = new List<Contact>();
            var session = NHibernateContext.SesionFactory;
            using (var transaction = session.BeginTransaction())
            {
                result = (List<Contact>)session.QueryOver<Contact>().Where(x => x.Name.IsLike(@"%"+searchQuery+"%")).List();
                //result.AddRange(session.QueryOver<Contact>().Where(x => x.User.UId == user.UId && x.Address.Contains(searchQuery)).List());
                transaction.Commit();
            }
            return result;
        }
    }
}