namespace CourseWarsaw.Models
{
	public class ChangeEvent
	{
		public virtual string EntityName { get; set; }
		public virtual int EntityId { get; set; }
		public virtual string PropertyName { get; set; }
		public virtual string OldValue { get; set; }
		public virtual string NewValue { get; set; }
	}
}