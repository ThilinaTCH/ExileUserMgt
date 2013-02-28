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
            if (storingUser==null)
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
            List<User> retrievedUser=new List<User>();
            var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        retrievedUser = (List<User>) session.QueryOver<User>().Where(x => x.UserName == userName).List();
                    }
                    catch (Exception)
                    {
                    }
                    transaction.Commit();
                }
            }
            if (retrievedUser.Count > 0)
            {
                User selected = retrievedUser.First();
                return selected;
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