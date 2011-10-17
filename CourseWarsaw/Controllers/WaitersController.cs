using System.Web.Mvc;
using CourseWarsaw.Models;
using NHibernate.Linq;
using System.Linq;

namespace CourseWarsaw.Controllers
{
	public class WaitersController : NHibernateController
	{
		public ActionResult New(string name)
		{
			session.Save(new Waiter { Name = name });
			return Json(new { Created = true }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult List(int start = 0)
		{
			var results = session.Query<Waiter>()
				.Skip(start * 25)
				.Take(25)
				.ToList();

			return Json(results, JsonRequestBehavior.AllowGet);
		}
	}
}