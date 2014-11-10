using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.ViewModels;

namespace ContosoUniversity.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            IQueryable<EnrollmentDataGroup> data = from student in db.Students
                                                   group student by student.EnrollmentDate
                                                       into dataGroup
                                                       select new EnrollmentDataGroup()
                                                       {
                                                           EnrollmentDate = dataGroup.Key,
                                                           StudentCount = dataGroup.Count()
                                                       };
                                                   return View(data.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        private SchoolContext db = new SchoolContext();

        protected override void Dispose(bool disposing)
        {
                    db.Dispose();
 	         base.Dispose(disposing);
        }
    }
}