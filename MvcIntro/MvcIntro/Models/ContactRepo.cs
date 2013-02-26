using System.Collections.Generic;

namespace MvcIntro.Models
{
    public class ContactRepo
    {
        public void AddUser(Contact m1)
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
                    session.Delete(session.Load<Contact>(id));

                    transaction.Commit();
                }
            }
            //delete user who have Id=id
        }
        public void UpdateUser(int id, Contact m1)
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

        public Contact GetUserById(int id)
        {
            var user = new Contact("testName", "testAddress");

            // create our NHibernate session factory
             var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    user = session.Get<Contact>(id);
                    transaction.Commit();
                }

            }

            return user;
        }

        public List<Contact> UserList()
        {
            return LoadUsers();
        }

        private void SaveUsers(Contact m2)
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

        private List<Contact> LoadUsers()
        {
            List<Contact> users = new List<Contact>();
            var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    users = (List<Contact>)session.CreateCriteria(typeof(Contact)).List<Contact>();
                    transaction.Commit();
                }
            }
            return users;
        }

        public List<Contact> GetSearchedUsers(string p)
        {
            var result = new List<Contact>();
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
