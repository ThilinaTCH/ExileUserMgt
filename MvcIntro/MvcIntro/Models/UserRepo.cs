﻿using System;
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

        public void UpdateUser(User m1)
        {
            var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(m1);

                    transaction.Commit();
                }
            }
            //replace existing record with this in DB table

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

        public Contact GetUserContactById(User use,int id)
        {
            IList<Contact> Contact = new List<Contact>();

            // create our NHibernate session factory
            var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    Contact = session.Get<User>(use.UId).ContactsList;
                    transaction.Commit();
                }

            }

            return Contact.First(x=>x.Id==id);
        }

        public List<Contact> GetSearchedUserContacts(User User, string searchQuery)
        {
            var result = new List<Contact>();
            var all = User.ContactsList;
            foreach (var Contacte in all)
            {
                if (Contacte.Name.ToLower().Contains(searchQuery.ToLower()) || Contacte.Address.ToLower().Contains(searchQuery.ToLower()))
                    result.Add(Contacte);
            }
            return result;
        }
    }
}