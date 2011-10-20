using FluentNHibernate.Mapping;

namespace CourseWarsaw.Models
{
    public class TableMap : ClassMap<Table>
    {
        public TableMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Created);
            HasMany<Bill>(x => x.Bills);
        }
         
    }
}