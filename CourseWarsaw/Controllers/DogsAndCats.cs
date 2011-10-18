using System;
using System.Web.Mvc;
using CourseWarsaw.Models;
using NHibernate.Linq;
using System.Linq;

namespace CourseWarsaw.Controllers
{
	public class DogsAndCats : NHibernateController
	{
		public ActionResult Create()
		{
			var cat = new Cat
			{
				AllowInResturant = true,
				LastHairBall = DateTime.Now
			};

			session.Save(cat);

			var dog = new Dog
			{
				AllowInResturant = false,
				Bark = "Woof!"
			};

			session.Save(dog);


			return Json(new { Done = "yes" },JsonRequestBehavior.AllowGet);
		}

		public ActionResult Find(int id)
		{
			return Json(session.Get<Animal>(id), JsonRequestBehavior.AllowGet);
		}

		public ActionResult FindCat(int id)
		{
			return Json(session.Get<Cat>(id), JsonRequestBehavior.AllowGet);
		}

		public ActionResult FindDog(int id)
		{
			return Json(session.Get<Dog>(id), JsonRequestBehavior.AllowGet);
		}

		public ActionResult FindDogs(bool rest, string bark)
		{
			var array = session.Query<Dog>().Where(x=>x.AllowInResturant == rest && x.Bark == bark).ToArray();
			return Json(array, JsonRequestBehavior.AllowGet);
		}

		public ActionResult FindAnimals(bool rest)
		{
			var array = session.Query<Animal>().Where(x => x.AllowInResturant == rest).ToArray();
			return Json(array, JsonRequestBehavior.AllowGet);
		}
	
	}
}