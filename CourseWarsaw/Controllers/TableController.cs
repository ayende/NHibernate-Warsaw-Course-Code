using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CourseWarsaw.Models;
using NHibernate;
using NHibernate.Linq;

namespace CourseWarsaw.Controllers
{
    public class TableController : NHibernateController
    {
        public ActionResult List()
        {
            var tables = session.Query<Table>().ToList();
            
            return Json(tables);
        }

        public ActionResult Add()
        {
            var table = new Table() { Created = DateTime.Now };
            session.Save(table);
            
            return Content("Added " + table);
        }

        public ActionResult Load(int id)
        {
            var table = session.Get<Table>(id);
            return Json(new { table.Id, table.Created });
        }

        public ActionResult Get(int id)
        {
            var table = session.Get<Table>(id);
            return Json(new { table.Id, table.Created });
        }

        public ActionResult GetAll(int id)
        {
            var table = session.Get<Table>(id);
            return Json(new { table.Id, table.Created, table.Bills.Count });
        }
    }
}