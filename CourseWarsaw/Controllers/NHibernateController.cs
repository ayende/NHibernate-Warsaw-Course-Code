using System.Web.Mvc;
using NHibernate;

namespace CourseWarsaw.Controllers
{
	public class NHibernateController : Controller
	{
		protected ISession session;

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			session = MvcApplication.SessionFactory.OpenSession();
			session.BeginTransaction();
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			using (session)
			{
				if (filterContext.Exception == null)
					session.Transaction.Commit();
			}
		}
	}
}