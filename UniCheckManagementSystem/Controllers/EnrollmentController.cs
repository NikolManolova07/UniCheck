using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using UniCheckManagementSystem.DAL;
using UniCheckManagementSystem.Infrastructure;
using UniCheckManagementSystem.Models;

namespace UniCheckManagementSystem.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter("Admin")]
    public class EnrollmentController : Controller
    {
        private UniCheckDbContext db = new UniCheckDbContext();

        // GET: Enrollment
        public ActionResult Index(int? searchCourseId)
        {
            var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Student);
            ViewBag.Courses = new SelectList(db.Courses, "CourseId", "Title");
            if (searchCourseId.HasValue)
                enrollments = enrollments.Where(x => x.CourseId == searchCourseId);
            return View(enrollments.ToList());
        }

        // GET: Enrollment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollment/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses.OrderBy(c => c.Title), "CourseId", "Title");
            ViewBag.StudentId = new SelectList(db.Students.OrderBy(s => s.FacultyNumber), "StudentId", "FacultyNumber");
            return View();
        }

        // POST: Enrollment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnrollmentId,CourseId,StudentId,Grade,ExamDate")] Enrollment enrollment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Enrollments.Add(enrollment);
                    db.SaveChanges();
                    TempData["success"] = "Успешно добавен изпит!";
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                TempData["error"] = "Неуспешно добавен изпит!";
                ModelState.AddModelError("", "Промените не могат да бъдат запазени. Опитайте отново и ако проблемът продължава, обърнете се към Вашия системен администратор.");
            }
            ViewBag.CourseId = new SelectList(db.Courses.OrderBy(c => c.Title), "CourseId", "Title");
            ViewBag.StudentId = new SelectList(db.Students.OrderBy(s => s.FacultyNumber), "StudentId", "FacultyNumber");
            return View(enrollment);
        }

        // GET: Enrollment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses.OrderBy(c => c.Title), "CourseId", "Title", enrollment.CourseId);
            ViewBag.StudentId = new SelectList(db.Students.OrderBy(s => s.FacultyNumber), "StudentId", "FacultyNumber", enrollment.StudentId);
            return View(enrollment);
        }

        // POST: Enrollment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnrollmentId,CourseId,StudentId,Grade,ExamDate")] Enrollment enrollment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(enrollment).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["success"] = "Успешно редактиран изпит!";
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                TempData["error"] = "Неуспешно редактиран изпит!";
                ModelState.AddModelError("", "Промените не могат да бъдат запазени. Опитайте отново и ако проблемът продължава, обърнете се към Вашия системен администратор.");
            }
            ViewBag.CourseId = new SelectList(db.Courses.OrderBy(c => c.Title), "CourseId", "Title", enrollment.CourseId);
            ViewBag.StudentId = new SelectList(db.Students.OrderBy(s => s.FacultyNumber), "StudentId", "FacultyNumber", enrollment.StudentId);
            return View(enrollment);
        }

        // GET: Enrollment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
            db.SaveChanges();
            TempData["success"] = "Успешно изтрит изпит!";
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
