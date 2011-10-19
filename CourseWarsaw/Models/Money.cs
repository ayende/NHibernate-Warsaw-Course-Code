namespace CourseWarsaw.Models
{
	public class Money
	{
		public string Currency { get; set; }
		public decimal Amount { get; set; }

		public bool Equals(Money other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other.Currency, Currency) && other.Amount == Amount;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (Money)) return false;
			return Equals((Money) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((Currency != null ? Currency.GetHashCode() : 0)*397) ^ Amount.GetHashCode();
			}
		}
	}
}