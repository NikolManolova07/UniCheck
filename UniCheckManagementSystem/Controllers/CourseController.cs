using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UniCheckManagementSystem.DAL;
using UniCheckManagementSystem.Infrastructure;
using UniCheckManagementSystem.Models;

namespace UniCheckManagementSystem.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter("Admin")]
    public class CourseController : Controller
    {
        private UniCheckDbContext db = new UniCheckDbContext();

        // GET: Course
        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CreditsSortParm = String.IsNullOrEmpty(sortOrder) ? "credits_desc" : "";
            var courses = from c in db.Courses
                          select c;
            switch (sortOrder)
            {
                case "credits_desc":
                    courses = courses.OrderByDescending(c => c.Credits);
                    break;
                default: // Credits ascending
                    courses = courses.OrderBy(c => c.Credits);
                    break;
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(courses.ToPagedList(pageNumber, pageSize));
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseId,Title,Credits,Description")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Courses.Add(course);
                    db.SaveChanges();
                    TempData["success"] = "Успешно добавен курс!";
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                TempData["error"] = "Неуспешно добавен курс!";
                ModelState.AddModelError("", "Промените не могат да бъдат запазени. Опитайте отново и ако проблемът продължава, обърнете се към Вашия системен администратор.");
            }
            return View(course);
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseId,Title,Credits,Description")] Course course)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(course).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["success"] = "Успешно редактиран курс!";
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                TempData["error"] = "Неуспешно редактиран курс!";
                ModelState.AddModelError("", "Промените не могат да бъдат запазени. Опитайте отново и ако проблемът продължава, обърнете се към Вашия системен администратор.");
            }
            return View(course);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            TempData["success"] = "Успешно изтрит курс!";
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
