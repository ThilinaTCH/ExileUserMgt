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
        private static ISession _sessionInstance;


        public static ISession SesionFactory
        {
            get
            {

                if (_sessionInstance == null)
                {
                    _sessionInstance = CreateSessionFactory().OpenSession();
                }
                return _sessionInstance;
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                .ConnectionString(x => x.Is("Server=localhost;User ID=sa;password=eXile123;Database= ExileContactMgt;Integrated Security=SSPI;"))
                          .UseReflectionOptimizer())
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<NHibernateContext>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))                
                .BuildSessionFactory();

        }
    }
}