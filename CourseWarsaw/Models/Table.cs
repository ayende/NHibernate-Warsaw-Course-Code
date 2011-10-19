using System.Collections.Generic;

namespace CourseWarsaw.Models
{
	public class Table
	{
		public virtual int Id { get; set; }
		public virtual bool Priority { get; set; }
		public virtual int Occupancy { get; set; }
		public virtual ICollection<Reservation> Reservations { get; set; }

		public virtual Money Outstanding { get; set; }
	}
}