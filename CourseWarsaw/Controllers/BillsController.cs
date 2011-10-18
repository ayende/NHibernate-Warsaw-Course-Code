using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CourseWarsaw.Models;
using NHibernate;
using NHibernate.Linq;

namespace CourseWarsaw.Controllers
{
    public class BillsController : NHibernateController
    {
        private const string holdKey = "holdKey";
        private ISessionFactory factory = MvcApplication.SessionFactory;
        

        public ActionResult List()
        {
            List<Bill> bills = session.Query<Bill>().ToList();

            return Json(bills, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            var billToAdd = new Bill {Name = GenerateName()};
            session.Save(billToAdd);
            
            return Content("Added " + billToAdd.Name);
        }

        
        public ActionResult Hold(int id)
        {
            var bill = session.Get<Bill>(id);
            HttpContext.Session.Add(holdKey, bill);

            return Content("Holding " + bill.Id);
        }

        public ActionResult Get(int id)
        {
            var bill = session.Get<Bill>(id);
            return Json(bill);
        }

        public ActionResult GetHold(int id)
        {
            var bill = (Bill)HttpContext.Session[holdKey];
            return Json(bill);
        }

        public ActionResult Update(int id)
        {
            var bill = session.Get<Bill>(id);
            bill.Name = GenerateName();

            return Content("Updated");
        }


        public ActionResult UpdateHold(int id)
        {
            var bill = (Bill)HttpContext.Session[holdKey];
            bill.Name = GenerateName();

            return Content("Updated hold");
        }

        public ActionResult Start()
        {
            var session1 = factory.OpenSession();
            var session2 = factory.OpenSession();

            //get
            var billS1 = session1.Get<Bill>(1);
            var billS2 = session2.Get<Bill>(1);

            //make concurrency error on 2
            billS2.Name = GenerateName();
            session2.Flush();
            session2.Dispose();

            //make concurrency error on 1
            billS1.Name = GenerateName();
            session1.Flush();
            session1.Dispose();


            return Content("Done");
        }

        private string GenerateName()
        {
            return Guid.NewGuid().ToString("n").Substring(0, 5);
        }

    }
}