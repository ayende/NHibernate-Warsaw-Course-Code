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

namespace CourseWarsaw.Infrastructure
{
	public class AuditListener : IPreInsertEventListener, IPreUpdateEventListener
	{
		public bool OnPreInsert(PreInsertEvent @event)
		{
            if(@event.GetType() != typeof(Event))
            {
                Set("CreatedAt", DateTime.Now, @event.Persister, @event.Entity, @event.State);
                Set("CreatedBy", WindowsIdentity.GetCurrent().Name, @event.Persister, @event.Entity, @event.State);
            }
			return false;
		}

		public bool OnPreUpdate(PreUpdateEvent @event)
		{


            if (@event.GetType() != typeof(Event))
            {
                Set("ModifiedAt", DateTime.Now, @event.Persister, @event.Entity, @event.State);
                Set("ModifiedBy", WindowsIdentity.GetCurrent().Name, @event.Persister, @event.Entity, @event.State);
                
                int i = 0;
                foreach (var propName in @event.Persister.PropertyNames)
                {
                    if (@event.OldState[i] != @event.State[i])
                    {
                        var changeEvent = new Event
                        {
                            Message = string.Format("{0}#{1}.{2} at {3}",
                                                    @event.Entity.GetType().Name,
                                                    @event.Id,
                                                    propName,
                                                    DateTime.Now)
                        };
                        @event.Session.Save(changeEvent);
                    }
                    i++;
                }
            }

		    return false;
		}

		private void Set(string name, object val, IEntityPersister entityPersister, object entity, object[] objects)
		{
			var index = Array.IndexOf(entityPersister.PropertyNames, name);

			entityPersister.SetPropertyValue(entity, index, val, EntityMode.Poco);

			objects[index] = val;

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