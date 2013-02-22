using FluentNHibernate.Mapping;

namespace MvcIntro.Models
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Address);
        }
    }
}