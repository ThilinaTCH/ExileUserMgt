using FluentNHibernate.Mapping;

namespace MvcIntro.Models
{
    public class ContactMap : ClassMap<Contact>
    {
        public ContactMap()
        {
            Id(x => x.Id);
            Map(x=>  x.UserId);
            Map(x => x.Name);
            Map(x => x.Address);
        }
    }
}