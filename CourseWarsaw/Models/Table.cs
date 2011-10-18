using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Util;

namespace CourseWarsaw.Models
{
    public class Table
    {
        public virtual int Id { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual IList<Bill> Bills { get; set; }

        public Table()
        {
            Bills = new List<Bill>();
        }
    }
}