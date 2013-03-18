using System.Configuration;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace MvcIntro.Models
{
    class NHibernateContext
    {
        private static ISessionFactory _sessionInstance;


        public static ISessionFactory SesionFactory
        {
            get
            {

                if (_sessionInstance == null)
                {
                    _sessionInstance = CreateSessionFactory();
                }
                return _sessionInstance;
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            //string Server = ConfigurationManager.AppSettings["NHiber:dbServer"];
            //string UserId = ConfigurationManager.AppSettings["NHiber:userId"];
            //string Password = ConfigurationManager.AppSettings["NHiber:password"];
            //string database = ConfigurationManager.AppSettings["NHiber:dbName"];
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                //.ConnectionString(x => x.Is("Server=" + Server + ";User ID=" + UserId + ";password=" + Password + ";Database=" + database + ";Integrated Security=SSPI;"))
                .ConnectionString(x => x.Is("Server=localhost;User ID=sa;password=eXile123;Database= NHibaernateDemo;Integrated Security=SSPI;"))
                          .UseReflectionOptimizer())
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<NHibernateContext>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))                
                .BuildSessionFactory();

        }
    }
}