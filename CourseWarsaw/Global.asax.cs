using System.Media;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
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

			SessionFactory = new Configuration()
				.SetProperty(NHibernate.Cfg.Environment.Hbm2ddlAuto, "update")
				.SetProperty(NHibernate.Cfg.Environment.Dialect, typeof(MsSql2008Dialect).AssemblyQualifiedName)
				.DataBaseIntegration(properties =>
				{
					properties.ConnectionStringName = Environment.MachineName;
				})
				.AddAssembly(Assembly.GetExecutingAssembly())
				.SetInterceptor(new ScreamYourHeartOut())
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