using System.Web.Mvc;
using CourseWarsaw.Models;
using NHibernate.Linq;
using System.Linq;

namespace CourseWarsaw.Controllers
{
	public class AdminController : NHibernateController
	{
		public ActionResult NewWaiter(string name)
		{
			session.Save(new Waiter { Name = name });
			return Json(new { Created = true }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ListWaiters(int start = 0)
		{
			var results = session.Query<Waiter>()
				.Skip(start * 25)
				.Take(25)
				.ToList();

			return Json(results, JsonRequestBehavior.AllowGet);
		}

		public ActionResult NewTable(bool priority)
		{
			session.Save(new Table() { Priority = priority });
			return Json(new { Created = true }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ListTables(int start = 0)
		{
			var results = session.Query<Table>()
				.Skip(start * 25)
				.Take(25)
				.ToList();

			return Json(results, JsonRequestBehavior.AllowGet);
		}
	}
}