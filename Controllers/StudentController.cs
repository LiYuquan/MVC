using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.Models;
using ContosoUniversity.DAL;

using PagedList;

namespace ContosoUniversity.Controllers
{
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: /Student/
        /// <summary>
        /// 实现排序功能
        /// </summary>
        /// <param name="sortOrder">排序依据</param>
        /// <returns>执行结果</returns>
        //public ActionResult Index(string sortOrder)
        //{
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
        //    var students = from s in db.Students
        //                   select s;
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            students = students.OrderByDescending(s => s.LastName);
        //            break;
        //        case "Date":
        //            students = students.OrderBy(s => s.EnrollmentDate);
        //            break;
        //        case "date_desc":
        //            students = students.OrderByDescending(s => s.EnrollmentDate);
        //            break;
        //        default:
        //            students = students.OrderBy(s => s.LastName);
        //            break;
        //    }
        //    return View(students.ToList());
        //}

        /// <summary>
        /// 实现排序和搜索功能
        /// </summary>
        /// <param name="sortOrder">排序依据</param>
        /// <param name="searchString">关键字</param>
        /// <returns>执行结果</returns>
        //public ActionResult Index(string sortOrder,string searchString)
        //{
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
        //    var students = from s in db.Students
        //                   select s;
        //    if(!string.IsNullOrEmpty(searchString))
        //    {
        //        students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
        //            || s.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
        //    }
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            students = students.OrderByDescending(s => s.LastName);
        //            break;
        //        case "Date":
        //            students = students.OrderBy(s => s.EnrollmentDate);
        //            break;
        //        case "date_desc":
        //            students = students.OrderByDescending(s => s.EnrollmentDate);
        //            break;
        //        default:
        //            students = students.OrderBy(s => s.LastName);
        //            break;
        //    }
        //    return View(students.ToList());
        //}


        public ActionResult Index(string sortOrder, string currentFilter, string searchString , int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;


            var students = from s in db.Students
                           select s;
            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                    || s.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(students.ToPagedList(pageNumber,pageSize) );
        }
        // GET: /Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: /Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(DataException)
            {
                ModelState.AddModelError("", "保存数据时出现错误，请重试。如果问题依旧存在请联系管理员。");
            }
            return View(student);
        }

        // GET: /Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Student/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(DataException)
            {
                ModelState.AddModelError("","无法保存变更，请重试，如果问题依旧，请联系管理员。");

            }
            return View(student);
        }

        // GET: /Student/Delete/5
        public ActionResult Delete(int? id,bool? saveChangeError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(saveChangeError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "删除错误，请重试。";
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try 
	        {	        
		        //Student student = db.Students.Find(id);
                    //db.Students.Remove(student);
                Student studentToDelete = new Student() { ID = id };
                    db.Entry(studentToDelete).State = EntityState.Deleted;
                    db.SaveChanges();
                    
	        }
	        catch (DataException)
	        {
		
		        return RedirectToAction("Delete",new {id = id,saveChangesError= true});
	        }
            return RedirectToAction("Index");
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
