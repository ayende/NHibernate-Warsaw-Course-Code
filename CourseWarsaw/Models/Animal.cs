using System;

namespace CourseWarsaw.Models
{
	public class Animal
	{
		public virtual int Id { get; set; }
		public virtual bool AllowInResturant { get; set; }
	}

	public class Dog : Animal
	{
		public virtual string Bark { get; set; }
	}

	public class Cat : Animal
	{
		public virtual DateTime LastHairBall { get; set; }
	}
}