using System;
using System.Web.Mvc;
using CourseWarsaw.Models;
using NHibernate.Linq;
using System.Linq;

namespace CourseWarsaw.Controllers
{
	public class ReservationController : NHibernateController
	{
		public ActionResult Today()
		{
			var reservations = session.Query<Reservation>()
				.Where(x=>DateTime.Today >= x.From && x.To <= DateTime.Today)
				.ToList();

			return Json(reservations.Select(x => new
			{
				x.From,
				x.To,
				Other = x.Table.Reservations.Select(y=> new
				{
					y.From,
					y.To
				}).ToArray(),
				x.Name
			}).ToArray(), JsonRequestBehavior.AllowGet);
		}

		public ActionResult Find(string name, int count, DateTime from, DateTime to)
		{
			var tables = session.Query<Table>()
				.Where(x => x.Occupancy >= count)
				.Where(x => x.Reservations.Any(y => y.From >= from && y.To <= to) == false)
				.Take(3)
				.ToList();

			return Json(tables.Select(x => new {x.Occupancy, x.Priority, x.Id}), JsonRequestBehavior.AllowGet);
		}
	}
}