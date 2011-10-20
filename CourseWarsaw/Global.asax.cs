using System;
using System.Media;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CourseWarsaw.Infrastructure;
using CourseWarsaw.Models;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Mapping;
using Environment = System.Environment;

namespace CourseWarsaw
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}/{id}", // URL with parameters
				new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);

		}
		
		public static ISessionFactory SessionFactory;

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);

			NHibernateProfiler.Initialize();

			var configuration = new Configuration()
				//.SetNamingStrategy(new StayOffMyLawn())
				.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlAuto, "update")
				.SetProperty(NHibernate.Cfg.Environment.Dialect, typeof(MsSql2008Dialect).AssemblyQualifiedName)
				.DataBaseIntegration(properties =>
				{
					properties.ConnectionStringName = Environment.MachineName;
				})
				.AddAssembly(Assembly.GetExecutingAssembly())
				.SetInterceptor(new ScreamYourHeartOut());

			var auditListener = new AuditListener();
			configuration.SetListener(ListenerType.PreUpdate, auditListener);

			var persistentClass = configuration.GetClassMapping(typeof(Waiter));

			//var manyToOne = new ManyToOne(persistentClass.Table);
			//var column = new Column("Current");
			//persistentClass.Table.AddColumn(column);
			//manyToOne.AddColumn(column);
			//manyToOne.ReferencedEntityName = configuration.GetClassMapping(typeof(Waiter)).EntityName;
			//manyToOne.ReferencedPropertyName = "Current";
			//persistentClass.AddProperty(new Property(manyToOne));

			AddDynamicComponentAttribute<bool>(persistentClass, "Rude");
			AddDynamicComponentAttribute<string>(persistentClass, "Phone");


			SessionFactory = configuration 
				.BuildSessionFactory();

			using(var openSession = SessionFactory.OpenSession())
			{
				CitiesCache.Refresh(openSession.Connection);
			}
		}

		private static void AddDynamicComponentAttribute<T>(PersistentClass persistentClass, string name)
		{
			var property = persistentClass.GetProperty("Attributes");
			var component = ((Component) property.Value);
			var propertyValue = new SimpleValue(component.Table)
			{
				TypeName = typeof (T).AssemblyQualifiedName
			};
			var column = new Column(name);
			propertyValue.AddColumn(column);
			var property1 = new Property(propertyValue)
			{
				Name = name
			};
			component.AddProperty(property1);
			component.Table.AddColumn(column);
		}
	}

	public class MyInsertListner : IPreInsertEventListener
	{
		public bool OnPreInsert(PreInsertEvent @event)
		{
			if(@event.Entity is Reservation)
				Console.Beep();
			return false;
		}
	}

	public class ScreamYourHeartOut : EmptyInterceptor
	{
		public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
		{
			int queryCount;
			HttpContext.Current.Items["query-count"] = queryCount = (int) (HttpContext.Current.Items["query-count"] ?? 0) + 1;

			if(queryCount > 10)
			{
				Console.Beep();
			}

			return base.OnPrepareStatement(sql);
		}
	}
}