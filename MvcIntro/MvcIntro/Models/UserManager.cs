using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcIntro.Models
{
    public class UserManager
    {
        public bool CreateUser(User newUser)
        {
            User storingUser = GetUserByName(newUser.UserName);
            bool isExist = true;
            if (storingUser != null)
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
                        isExist = false;
                    }
                }
            }
            return isExist;
        }

        //GetUserByName return user
        public User GetUserByName(String userName)
        {
            User retrievedUser = null; // new User();
            var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    retrievedUser = session.Get<User>(userName);
                    transaction.Commit();
                }

            }
            return retrievedUser;
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