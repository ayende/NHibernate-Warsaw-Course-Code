using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using CourseWarsaw.Models;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using NHibernate.Properties;
using NHibernate.Type;

namespace CourseWarsaw.Infrastructure
{
	public class AuditListener : IPreUpdateEventListener
	{
		public bool OnPreUpdate(PreUpdateEvent @event)
		{
			using (var childSession = @event.Session.GetSession(EntityMode.Poco))
			{
				for (var i = 0; i < @event.Persister.PropertyNames.Length; i++)
				{
					if(@event.Persister.VersionProperty == i)
						continue;

					var propName = @event.Persister.PropertyNames[i];
					IType propertyType = @event.Persister.PropertyTypes[i];
					var same = propertyType.IsEqual(@event.OldState[i], @event.State[i], EntityMode.Poco);
					if (same)
						continue;


					var changeEvent = new ChangeEvent
					{
						EntityName = @event.Persister.EntityName,
						EntityId = (int)@event.Id,
						NewValue = (@event.State[i] ?? "null").ToString(),
						OldValue = (@event.OldState[i] ?? "null").ToString(),
						PropertyName = propName
					};
					childSession.Save(changeEvent);
				}

				childSession.Flush();
			}

			return false;
		}
	}

	public class HiddenPropertyAccessor : IPropertyAccessor
	{
		static readonly ConditionalWeakTable<object, Hashtable> hiddenprops = new ConditionalWeakTable<object, Hashtable>();

		public IGetter GetGetter(Type theClass, string propertyName)
		{
			return new HiddenProperty(propertyName, typeof(object));
		}

		public ISetter GetSetter(Type theClass, string propertyName)
		{
			return new HiddenProperty(propertyName, typeof(object));
		}

		public bool CanAccessThroughReflectionOptimizer
		{
			get { return false; }
		}

		public class HiddenProperty : IGetter, ISetter
		{
			private readonly string propertyName;
			private readonly Type propertyType;

			public HiddenProperty(string propertyName, Type propertyType)
			{
				this.propertyName = propertyName;
				this.propertyType = propertyType;
			}

			public object Get(object target)
			{
				return hiddenprops.GetOrCreateValue(target)[propertyName];
			}

			public object GetForInsert(object owner, IDictionary mergeMap, ISessionImplementor session)
			{
				return Get(owner);
			}

			public Type ReturnType
			{
				get { return propertyType; }
			}

			public void Set(object target, object value)
			{
				hiddenprops.GetOrCreateValue(target)[propertyName] = value;
			}

			public string PropertyName
			{
				get { return propertyName; }
			}

			public MethodInfo Method
			{
				get { return null; }
			}
		}
	}
}