using System;

namespace CourseWarsaw.Models
{
	public class Reservation
	{
		public virtual int Id { get; set; }
		public virtual int PeopleCount { get; set; }
		public virtual Table Table { get; set; }
		public virtual string Name { get; set; }
		public virtual DateTime From { get; set; }
		public virtual DateTime To { get; set; }
		public virtual string PhoneNumber { get; set; }
	}
}