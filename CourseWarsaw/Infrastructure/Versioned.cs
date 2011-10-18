using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using NHibernate.Engine;
using NHibernate.Properties;

namespace CourseWarsaw.Infrastructure
{
	public class Versioned
	{
		static readonly ConditionalWeakTable<object, IntHolder> ConditionalWeakTable = new ConditionalWeakTable<object, IntHolder>();

		public class IntHolder
		{
			public int Version;
		}

		public static int Get(object o)
		{
			return ConditionalWeakTable.GetOrCreateValue(o).Version;
		}

		public static void Set(object o, int version)
		{
			ConditionalWeakTable.GetOrCreateValue(o).Version = version;
		}
	}

	public class HiddenVersionAccessor : IPropertyAccessor, IGetter, ISetter
	{
		public IGetter GetGetter(Type theClass, string propertyName)
		{
			return this ;
		}

		public ISetter GetSetter(Type theClass, string propertyName)
		{
			return this;
		}

		public bool CanAccessThroughReflectionOptimizer
		{
			get { return false; }
		}

		public object Get(object target)
		{
			return Versioned.Get(target);
		}

		public object GetForInsert(object owner, IDictionary mergeMap, ISessionImplementor session)
		{
			return Versioned.Get(owner);
		}

		public Type ReturnType
		{
			get { return typeof(int); }
		}

		public void Set(object target, object value)
		{
			Versioned.Set(target, (int)(value ?? 0));
	
		}

		public string PropertyName
		{
			get { return null; }
		}

		public MethodInfo Method
		{
			get { return null; }
		}
	}
}