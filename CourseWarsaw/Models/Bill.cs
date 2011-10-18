using System.Collections;
using NHibernate.Util;

namespace CourseWarsaw.Models
{
    public class Bill
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Table Table { get; set; }
    }
}