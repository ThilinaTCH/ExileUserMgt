using FluentNHibernate.Mapping;

namespace MvcIntro.Models
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.UId);
            Map(x => x.UserName);
            Map(x => x.Password);
            HasMany<Contact>(x => x.ContactsList).Cascade.AllDeleteOrphan().Not.LazyLoad();
        }
    }
}