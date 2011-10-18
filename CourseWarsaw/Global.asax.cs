using System.Media;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Data;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
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
                "BIlls", // Route name
                "bills", // URL with parameters
                new { controller = "bills", action = "list" } // Parameter defaults
            );

			routes.MapRoute(
				"Default", // Route name
				"{controller}/{action}", // URL with parameters
				new { controller = "Home", action = "Start"} // Parameter defaults
			);

		}
		
		public static ISessionFactory SessionFactory;

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);

			NHibernateProfiler.Initialize();


            //SessionFactory = new Configuration()
            //    .SetProperty(NHibernate.Cfg.Environment.Hbm2ddlAuto, "create")
            //    .SetProperty(NHibernate.Cfg.Environment.Dialect, typeof(MsSql2008Dialect).AssemblyQualifiedName)
            //    .DataBaseIntegration(properties =>
            //    {

            //        properties.ConnectionStringName = "Reduction";
            //    })
            //    .AddAssembly(Assembly.GetExecutingAssembly())
            //    .SetInterceptor(new ScreamYourHeartOut())
            //    .BuildSessionFactory();


            SessionFactory = Fluently.Configure()
		        .Database(MsSqlConfiguration.MsSql2008.ConnectionString(x => x.FromConnectionStringWithKey("Reduction")))
                .Mappings(x => x.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .ExposeConfiguration(x=> new SchemaExport(x).Create(true,true))
		        .BuildSessionFactory();
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
				for (int i = 0; i < queryCount-10; i++)
				{
					new SoundPlayer(@"C:\Users\Ayende\scream.wav").Play();
				}
			}

			return base.OnPrepareStatement(sql);
		}
	}
}