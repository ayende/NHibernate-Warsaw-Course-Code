using System;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace CourseWarsaw.Models
{
	public class MoneyType : IUserType
	{
		public new bool Equals(object x, object y)
		{
			return object.Equals(x, y);
		}

		public int GetHashCode(object x)
		{
			return x.GetHashCode();
		}

		public object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			object currency = rs[names[0]];
			object amount = rs[names[1]];

			if(currency == DBNull.Value && amount == DBNull.Value)
			{
				return null;
			}

			return new Money
			{
				Amount = (decimal) amount,
				Currency = (string) currency
			};
		}

		public void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			var money = ((Money)value);
			((IDataParameter) cmd.Parameters[index]).Value = money.Currency;
			((IDataParameter)cmd.Parameters[index + 1]).Value = money.Amount;
		}

		public object DeepCopy(object value)
		{
			var money = ((Money) value);
			return new Money
			{
				Amount = money.Amount,
				Currency = money.Currency,
			};
		}

		public object Replace(object original, object target, object owner)
		{
			return DeepCopy(original);
		}

		public object Assemble(object cached, object owner)
		{
			var arr = (object[]) cached;
			return new Money
			{
				Currency = (string) arr[0],
				Amount = (decimal) arr[1]
			};
		}

		public object Disassemble(object value)
		{
			var money = (Money) value;
			return new object[]
			{
				money.Currency,
				money.Amount
			};
		}

		public SqlType[] SqlTypes
		{
			get
			{
				return new[]
				{
					NHibernateUtil.String.SqlType,
					NHibernateUtil.Decimal.SqlType
				};
			}
		}

		public Type ReturnedType
		{
			get { return typeof (Money); }
		}

		public bool IsMutable
		{
			get { return true; }
		}
	}
}