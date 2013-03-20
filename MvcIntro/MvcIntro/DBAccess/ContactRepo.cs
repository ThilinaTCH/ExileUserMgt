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
            var session = NHibernateContext.SesionFactory;
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(session.Get<Contact>(id));

                transaction.Commit();
            }
        }
        public void UpdateContact(int id, Contact m1)
        {
            var session = NHibernateContext.SesionFactory;
            using (var transaction = session.BeginTransaction())
            {
                m1.Id = id;
                session.Merge(m1);

                transaction.Commit();
            }
        }

        public void DeleteAll(int uid)
        {
            var session = NHibernateContext.SesionFactory;
            using (var transaction = session.BeginTransaction())
            {
                session.CreateQuery("delete from Contact where UserId=\"" + uid + "\"").ExecuteUpdate();
                transaction.Commit();
            }
        }

        public Contact GetContactById(int id)
        {
            var Contact = new Contact("testName", "testAddress");

            var session = NHibernateContext.SesionFactory;
            using (var transaction = session.BeginTransaction())
            {
                Contact = session.Get<Contact>(id);
                transaction.Commit();
            }
            return Contact;
        }

        public IList<Contact> ContactList(int uid)
        {
            return LoadContacts(uid);
        }

        private void SaveContacts(Contact m2)
        {
            var session = NHibernateContext.SesionFactory;
            using (var transaction = session.BeginTransaction())
            {
                session.Merge(m2);
                transaction.Commit();
            }
        }

        private IList<Contact> LoadContacts(int uid)
        {
            IList<Contact> contacts = new List<Contact>();
            var session = NHibernateContext.SesionFactory;
            using (var transaction = session.BeginTransaction())
            {
                contacts = session.Get<User>(uid).ContactsList;
                transaction.Commit();
            }
            return contacts;
        }
    }
}
