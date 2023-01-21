using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using UniCheckManagementSystem.DAL;
using UniCheckManagementSystem.Infrastructure;
using UniCheckManagementSystem.Migrations;
using UniCheckManagementSystem.Models;
using UniCheckManagementSystem.ViewModels;

namespace UniCheckManagementSystem.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter("Admin")]
    public class LecturerController : Controller
    {
        private UniCheckDbContext db = new UniCheckDbContext();

        // GET: Lecturer
        public ActionResult Index(int? id, int? courseId)
        {
            var viewModel = new LecturerIndexData();
            viewModel.Lecturers = db.Lecturers
                .Include(l => l.Department)
                .Include(l => l.Courses)
                .OrderBy(l => l.LastName);

            if (id != null)
            {
                ViewBag.LecturerId = id.Value;
                viewModel.Courses = viewModel.Lecturers.Where(
                    l => l.LecturerId == id.Value).Single().Courses;
            }

            if (courseId != null)
            {
                ViewBag.CourseId = courseId.Value;
                viewModel.Enrollments = viewModel.Courses.Where(
                    x => x.CourseId == courseId).Single().Enrollments;
            }

            return View(viewModel);
        }

        // GET: Lecturer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecturer lecturer = db.Lecturers.Find(id);
            if (lecturer == null)
            {
                return HttpNotFound();
            }
            return View(lecturer);
        }

        // GET: Lecturer/Create
        public ActionResult Create()
        {
            var lecturer = new Lecturer();
            lecturer.Courses = new List<Models.Course>();
            PopulateAssignedCourseData(lecturer);
            ViewBag.DepartmentId = new SelectList(db.Departments.OrderBy(d => d.Name), "DepartmentId", "Name");
            return View();
        }

        // POST: Lecturer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LecturerId,FirstName,LastName,HireDate,Office,ImagePath,ImageFile,DepartmentId")] Lecturer lecturer, string[] selectedCourses)
        {
            try
            {
                if (selectedCourses != null)
                {
                    lecturer.Courses = new List<Models.Course>();
                    foreach (var course in selectedCourses)
                    {
                        var courseToAdd = db.Courses.Find(int.Parse(course));
                        lecturer.Courses.Add(courseToAdd);
                    }
                }
                if (lecturer.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(lecturer.ImageFile.FileName);
                    string extension = Path.GetExtension(lecturer.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    lecturer.ImagePath = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    lecturer.ImageFile.SaveAs(fileName);
                }
                if (ModelState.IsValid)
                {
                    db.Lecturers.Add(lecturer);
                    db.SaveChanges();
                    TempData["success"] = "Успешно добавен преподавател!";
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                TempData["error"] = "Неуспешно добавен преподавател!";
                ModelState.AddModelError("", "Промените не могат да бъдат запазени. Опитайте отново и ако проблемът продължава, обърнете се към Вашия системен администратор.");
            }
            PopulateAssignedCourseData(lecturer);
            ViewBag.DepartmentId = new SelectList(db.Departments.OrderBy(d => d.Name), "DepartmentId", "Name", lecturer.DepartmentId);
            return View(lecturer);
        }

        // GET: Lecturer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecturer lecturer = db.Lecturers
                .Include(l => l.Department)
                .Include(l => l.Courses)
                .Where(l => l.LecturerId == id)
                .Single();
            PopulateAssignedCourseData(lecturer);
            if (lecturer == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments.OrderBy(d => d.Name), "DepartmentId", "Name", lecturer.DepartmentId);
            return View(lecturer);
        }

        // POST: Lecturer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var lecturerToUpdate = db.Lecturers
               .Include(l => l.Department)
               .Include(l => l.Courses)
               .Where(l => l.LecturerId == id)
               .Single();

            if (TryUpdateModel(lecturerToUpdate, "",
               new string[] { "FirstName", "LastName", "HireDate", "Office", "ImagePath", "ImageFile", "DepartmentId" }))
            {
                try
                {
                    // Select ImageFile, Exists ImagePath
                    if (lecturerToUpdate.ImageFile != null && lecturerToUpdate.ImagePath != null)
                    {
                        string fileName = lecturerToUpdate.ImagePath.Remove(0, 9);
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        FileInfo fileInfo = new FileInfo(fileName);
                        if (fileInfo != null)
                        {
                            System.IO.File.Delete(fileName);
                            fileInfo.Delete();
                        }
                        fileName = Path.GetFileNameWithoutExtension(lecturerToUpdate.ImageFile.FileName);
                        string extension = Path.GetExtension(lecturerToUpdate.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        lecturerToUpdate.ImagePath = "~/Images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        lecturerToUpdate.ImageFile.SaveAs(fileName);
                    }

                    // Select ImageFile, Does not exist ImagePath
                    if (lecturerToUpdate.ImageFile != null && lecturerToUpdate.ImagePath == null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(lecturerToUpdate.ImageFile.FileName);
                        string extension = Path.GetExtension(lecturerToUpdate.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        lecturerToUpdate.ImagePath = "~/Images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        lecturerToUpdate.ImageFile.SaveAs(fileName);
                    }
                    UpdateInstructorCourses(selectedCourses, lecturerToUpdate);
                    db.SaveChanges();
                    TempData["success"] = "Успешно редактиран преподавател!";
                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    TempData["error"] = "Неуспешно редактиран преподавател!";
                    ModelState.AddModelError("", "Промените не могат да бъдат запазени. Опитайте отново и ако проблемът продължава, обърнете се към Вашия системен администратор.");
                }
            }
            PopulateAssignedCourseData(lecturerToUpdate);
            ViewBag.DepartmentId = new SelectList(db.Departments.OrderBy(d => d.Name), "DepartmentId", "Name", lecturerToUpdate.DepartmentId);
            return View(lecturerToUpdate);
        }

        // GET: Lecturer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecturer lecturer = db.Lecturers.Find(id);
            if (lecturer == null)
            {
                return HttpNotFound();
            }
            return View(lecturer);
        }

        // POST: Lecturer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lecturer lecturer = db.Lecturers
                          .Include(l => l.Department)
                          .Where(i => i.LecturerId == id)
                          .Single();

            string fileName = lecturer.ImagePath.Remove(0, 9);
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo != null)
            {
                System.IO.File.Delete(fileName);
                fileInfo.Delete();
            }

            db.Lecturers.Remove(lecturer);
            db.SaveChanges();
            TempData["success"] = "Успешно изтрит преподавател!";
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

        private void PopulateAssignedCourseData(Lecturer lecturer)
        {
            var allCourses = db.Courses;
            var lecturerCourses = new HashSet<int>(lecturer.Courses.Select(c => c.CourseId));
            var viewModel = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                viewModel.Add(new AssignedCourseData
                {
                    CourseId = course.CourseId,
                    Title = course.Title,
                    Assigned = lecturerCourses.Contains(course.CourseId)
                });
            }
            ViewBag.Courses = viewModel;
        }

        private void UpdateInstructorCourses(string[] selectedCourses, Lecturer lecturerToUpdate)
        {
            if (selectedCourses == null)
            {
                lecturerToUpdate.Courses = new List<Models.Course>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var lecturerCourses = new HashSet<int>
                (lecturerToUpdate.Courses.Select(c => c.CourseId));
            foreach (var course in db.Courses)
            {
                if (selectedCoursesHS.Contains(course.CourseId.ToString()))
                {
                    if (!lecturerCourses.Contains(course.CourseId))
                    {
                        lecturerToUpdate.Courses.Add(course);
                    }
                }
                else
                {
                    if (lecturerCourses.Contains(course.CourseId))
                    {
                        lecturerToUpdate.Courses.Remove(course);
                    }
                }
            }
        }
    }
}
