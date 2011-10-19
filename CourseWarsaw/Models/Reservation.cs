using System;
using System.Collections.Concurrent;
using System.Data;
using NHibernate;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

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
		public virtual City City { get; set; }
	}

	public class City
	{
		public string Name { get; set; }
		public int Id { get; set; }
	}

	public static class CitiesCache
	{
		static readonly ConcurrentDictionary<int, City> cities = new ConcurrentDictionary<int, City>();

		public static void Refresh(IDbConnection connection)
		{
			using(var cmd = connection.CreateCommand())
			{
				cmd.CommandText = "select Id, Name from Cities";
				using(var reader = cmd.ExecuteReader())
				{
					while(reader.Read())
					{
						var city = new City
						{
							Id = reader.GetInt32(0),
							Name = reader.GetString(1)
						};
						cities.AddOrUpdate(city.Id, city, (i, _) => city);
					}
				}
			}
		}

		public static City Get(int id)
		{
			City city;
			if (cities.TryGetValue(id, out city) == false)
				return null;
			return new City
			{
				Id = city.Id,
				Name = city.Name
			};
		}
	}

	public class CityType : IUserType
	{
		public bool Equals(object x, object y)
		{
			if (x == null && y == null)
				return true;
			if (x == null || y == null)
				return false;

			return ((City) x).Id == ((City) y).Id;

		}

		public int GetHashCode(object x)
		{
			return ((City) x).Id;
		}

		public object NullSafeGet(IDataReader rs, string[] names, object owner)
		{
			var cityId = (int)(NHibernateUtil.Int32.NullSafeGet(rs, names) ?? -1);

			return CitiesCache.Get(cityId);
		}

		public void NullSafeSet(IDbCommand cmd, object value, int index)
		{
			var c = value as City;
			NHibernateUtil.Int32.NullSafeSet(cmd, c == null ? (object)null : c.Id, index);
		}

		public object DeepCopy(object value)
		{
			var city = ((City)value);
			return new City
			{
				Name = city.Name,
				Id = city.Id
			};
		}

		public object Replace(object original, object target, object owner)
		{
			return DeepCopy(original);
		}

		public object Assemble(object cached, object owner)
		{
			throw new NotImplementedException();
		}

		public object Disassemble(object value)
		{
			throw new NotImplementedException();
		}

		public SqlType[] SqlTypes
		{
			get
			{
				return new[]
				{
					NHibernateUtil.Int32.SqlType
				};
			}
		}

		public Type ReturnedType
		{
			get { return typeof(City); }
		}

		public bool IsMutable
		{
			get { return false; }
		}
	}
}