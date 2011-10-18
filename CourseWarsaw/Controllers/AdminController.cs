using System;
using System.Collections;
using System.Web.Mvc;
using CourseWarsaw.Models;
using NHibernate.Linq;
using System.Linq;

namespace CourseWarsaw.Controllers
{
	public class AdminController : NHibernateController
	{
		public ActionResult ListReservations()
		{
			var reservations = session.Query<Reservation>().ToArray();
			return Json(reservations, JsonRequestBehavior.AllowGet);
		}

		public ActionResult NewReservation()
		{
			session.Save(new Reservation
			{
				Name = "Ayende",
				PhoneNumber = "+972",
				From = DateTime.Now,
				To = DateTime.Now,
			});
			return Json(new { Created = true }, JsonRequestBehavior.AllowGet);
		
		}

		public ActionResult NewWaiter(string name)
		{
			session.Save(new Waiter
			{
				Name = name,
				Attributes = new Hashtable
				{
					{"SpeaksPolish", true},
					{"Rude", true},
				}
			});
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