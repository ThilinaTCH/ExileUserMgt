using System.Collections;
using System.Collections.Generic;

namespace MvcIntro.Models
{
    public class ContactRepo
    {
        public void AddContact(Contact m1)
        {
            SaveContacts(m1);
        }

        public void DeleteContact(int id)
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
            //delete Contact who have Id=id
        }
        public void UpdateContact(int id, Contact m1)
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

        public void DeleteAll(int uid)
        {
             var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    session.CreateQuery("delete from Contact where UserId=\""+uid+"\"").ExecuteUpdate();
                    transaction.Commit();
                }
            }
        }

        public Contact GetContactById(int id)
        {
            var Contact = new Contact("testName", "testAddress",0);

            // create our NHibernate session factory
             var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    Contact = session.Get<Contact>(id);
                    transaction.Commit();
                }

            }

            return Contact;
        }

        public IList<Contact> ContactList(int uid)
        {
            return LoadContacts(uid);
        }

        private void SaveContacts(Contact m2)
        {
             var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    // save Contact
                    session.SaveOrUpdate(m2);
                    transaction.Commit();
                }
            }
        }

        private IList<Contact> LoadContacts(int uid)
        {
            IList<Contact> contacts = new List<Contact>();
            var sessionFactory = NHibernateContext.SesionFactory;

            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    contacts = (IList<Contact>)(session.Get<User>(uid).ContactsList);
                    transaction.Commit();
                }
            }
            return contacts;
        }

        public List<Contact> GetSearchedContacts(int uid,string p)
        {
            var result = new List<Contact>();
            var all = LoadContacts(uid);
            foreach (var Contacte in all)
            {
                if (Contacte.Name.ToLower().Contains(p.ToLower()) || Contacte.Address.ToLower().Contains(p.ToLower()))
                    result.Add(Contacte);
            }
            return result;
        }
    }
}
