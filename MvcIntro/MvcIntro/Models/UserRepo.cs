using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                var sessionFactory = NHibernateContext.SesionFactory;

                using (var session = sessionFactory.OpenSession())
                {
                    // populate the database
                    using (var transaction = session.BeginTransaction())
                    {
                        // save user
                        session.SaveOrUpdate(newUser);
                        transaction.Commit();
                        isExist = true;
                    }
                }
            }
            return isExist;
        }

        //GetUserByName return user
        public User GetUserByName(String userName)
        {
            List<User> retrievedUser; // new User();
            var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    retrievedUser = (List<User>) session.QueryOver<User>().Where(x => x.UserName == userName).List();
                    transaction.Commit();
                }

            }
            if (retrievedUser.Count > 0)
            {
                return retrievedUser[0];
            }
            return null;
        }

        //ValidateUser method validation return bool
        public bool ValidateUser(String userName, String passWord)
        {
            User storedUser = null; // new User();
            storedUser = GetUserByName(userName);
            if (storedUser != null && storedUser.Password == passWord)
                return true;
            else return false;
        }
    }
}