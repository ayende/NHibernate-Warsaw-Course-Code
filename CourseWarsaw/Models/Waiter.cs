using System.Collections;

namespace CourseWarsaw.Models
{
	public class Waiter
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual IDictionary Attributes { get; set; }
		public virtual Reservation Current { get; set; }
	}
}