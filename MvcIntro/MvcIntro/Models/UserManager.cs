using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcIntro.Models
{
    public class UserManager
    {
        public void AddUser(User m1)
        {
            SaveUsers(m1);
        }

        public void DeleteUser(int id)
        {
            // create our NHibernate session factory
            var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    // save both stores, this saves everything else via cascading
                    session.Delete(session.Load<User>(id));

                    transaction.Commit();
                }
            }
            //delete user who have Id=id
        }
        public void UpdateUser(int id, User m1)
        {
            var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    m1.Id = id;
                    session.Update(m1);

                    transaction.Commit();
                }
            }
            //replace existing record with this in DB table

        }

        public void DeleteAll()
        {
            var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    session.CreateQuery("delete from User").ExecuteUpdate();
                    transaction.Commit();
                }
            }
        }

        public User GetUserById(int id)
        {
            var user = new User("testName", "testAddress");

            // create our NHibernate session factory
            var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    user = session.Get<User>(id);
                    transaction.Commit();
                }

            }

            return user;
        }

        public List<User> UserList()
        {
            return LoadUsers();
        }

        private void SaveUsers(User m2)
        {
            var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    // save user
                    session.SaveOrUpdate(m2);
                    transaction.Commit();
                }
            }
        }

        private List<User> LoadUsers()
        {
            List<User> users = new List<User>();
            var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    users = (List<User>)session.CreateCriteria(typeof(User)).List<User>();
                    transaction.Commit();
                }
            }
            return users;
        }

        public List<User> GetSearchedUsers(string p)
        {
            var result = new List<User>();
            var all = LoadUsers();
            foreach (var Usere in all)
            {
                if (Usere.Name.ToLower().Contains(p.ToLower()) || Usere.Address.ToLower().Contains(p.ToLower()))
                    result.Add(Usere);
            }
            return result;
        }
    }
}