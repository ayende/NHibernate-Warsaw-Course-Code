using System;
using System.Security.Cryptography;
using System.Text;
using NHibernate.Cfg;

namespace CourseWarsaw.Infrastructure
{
	public class StayOffMyLawn : INamingStrategy
	{
		private string Hash(string name)
		{
			using(var md5 = MD5.Create())
			{
				return "`" + BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(name))) + "`";
			}
		}

		public string ClassToTableName(string className)
		{
			return Hash(className);
		}

		public string PropertyToColumnName(string propertyName)
		{
			return Hash(propertyName);
		}

		public string TableName(string tableName)
		{
			return Hash(tableName);
		}

		public string ColumnName(string columnName)
		{
			return Hash(columnName);
		}

		public string PropertyToTableName(string className, string propertyName)
		{
			return Hash(className +"/" + propertyName);
		}

		public string LogicalColumnName(string columnName, string propertyName)
		{
			return Hash(columnName + "/" + propertyName);
		}
	}
}