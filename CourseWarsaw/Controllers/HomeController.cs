using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CourseWarsaw.Models;
using NHibernate;

namespace CourseWarsaw.Controllers
{
    public class HomeController : Controller
    {
     

        public ActionResult Start()
        {
            return Content("Started");
        }
    }
}