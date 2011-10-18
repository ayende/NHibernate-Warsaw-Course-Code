using FluentNHibernate.Mapping;

namespace CourseWarsaw.Models
{
    public class BillMap : ClassMap<Bill>
    {
        public BillMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            References(x => x.Table);
        }
    }
}