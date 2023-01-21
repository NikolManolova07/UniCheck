using Microsoft.Ajax.Utilities;
using Microsoft.Reporting.WebForms;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using UniCheckManagementSystem.DAL;
using UniCheckManagementSystem.Infrastructure;
using UniCheckManagementSystem.Models;

namespace UniCheckManagementSystem.Controllers
{
    [AuthenticationFilter]
    public class StudentController : Controller
    {
        private UniCheckDbContext db = new UniCheckDbContext();

        [AuthorizationFilter("Admin")]
        // GET: Student
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? searchSpecialtyId)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FacultyNumberSortParm = String.IsNullOrEmpty(sortOrder) ? "faculty_number_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";

            if (searchString != null)
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
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.FacultyNumber.Contains(searchString));
            }

            ViewBag.Specialties = new SelectList(db.Specialties, "SpecialtyId", "Name");
            if (searchSpecialtyId.HasValue)
                students = students.Where(x => x.SpecialtyId == searchSpecialtyId);

            switch (sortOrder)
            {
                case "faculty_number_desc":
                    students = students.OrderByDescending(s => s.FacultyNumber);
                    break;
                case "Name":
                    students = students.OrderBy(s => s.FirstName);
                    break;
                case "name_desc":
                    students = students.OrderByDescending(s => s.FirstName);
                    break;
                default:  // FacultyNumber ascending 
                    students = students.OrderBy(s => s.FacultyNumber);
                    break;
            }

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(students.ToPagedList(pageNumber, pageSize));
        }

        [AuthorizationFilter("Admin")]
        // GET: Student/Details/5
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

        [AuthorizationFilter("Admin", "Student")]
        // GET: Student/ViewGrades/5
        public ActionResult ViewGrades()
        {
            if (!Convert.ToString(Session["UserName"]).Equals("admin") && !Convert.ToString(Session["UserName"]).IsNullOrWhiteSpace())
            {
                string userName = Convert.ToString(Session["UserName"]);
                Student student = (from s in db.Students
                                   where s.FacultyNumber == userName
                                   select s).FirstOrDefault();
                return View(student);
            }
            return RedirectToAction("Index");
        }

        [AuthorizationFilter("Admin")]
        // GET: Student/Create
        public ActionResult Create()
        {
            ViewBag.SpecialtyId = new SelectList(db.Specialties.OrderByDescending(s => s.Name), "SpecialtyId", "Name");
            return View();
        }

        [AuthorizationFilter("Admin")]
        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,FacultyNumber,FirstName,LastName,EnrollmentDate,ImagePath,ImageFile,SpecialtyId")] Student student)
        {
            try
            {
                if (student.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(student.ImageFile.FileName);
                    string extension = Path.GetExtension(student.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    student.ImagePath = "~/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    student.ImageFile.SaveAs(fileName);
                }
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    TempData["success"] = "Успешно добавен студент!";
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                TempData["error"] = "Неуспешно добавен студент!";
                ModelState.AddModelError("", "Промените не могат да бъдат запазени. Опитайте отново и ако проблемът продължава, обърнете се към Вашия системен администратор.");
            }
            ViewBag.SpecialtyId = new SelectList(db.Specialties.OrderByDescending(s => s.Name), "SpecialtyId", "Name", student.SpecialtyId);
            return View(student);
        }

        [AuthorizationFilter("Admin")]
        // GET: Student/Edit/5
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
            ViewBag.SpecialtyId = new SelectList(db.Specialties.OrderByDescending(s => s.Name), "SpecialtyId", "Name", student.SpecialtyId);
            return View(student);
        }

        [AuthorizationFilter("Admin")]
        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student studentToUpdate = db.Students
                .Include(s => s.Specialty)
                .Where(s => s.StudentId == id)
                .Single();
            if (TryUpdateModel(studentToUpdate, "",
               new string[] { "FacultyNumber", "FirstName", "LastName", "EnrollmentDate", "ImagePath", "ImageFile", "SpecialtyId" }))
            {
                try
                {
                    // Select ImageFile, Exists ImagePath
                    if (studentToUpdate.ImageFile != null && studentToUpdate.ImagePath != null)
                    {
                        string fileName = studentToUpdate.ImagePath.Remove(0, 9);
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        FileInfo fileInfo = new FileInfo(fileName);
                        if (fileInfo != null)
                        {
                            System.IO.File.Delete(fileName);
                            fileInfo.Delete();
                        }
                        fileName = Path.GetFileNameWithoutExtension(studentToUpdate.ImageFile.FileName);
                        string extension = Path.GetExtension(studentToUpdate.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        studentToUpdate.ImagePath = "~/Images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        studentToUpdate.ImageFile.SaveAs(fileName);
                    }

                    // Select ImageFile, Does not exist ImagePath
                    if (studentToUpdate.ImageFile != null && studentToUpdate.ImagePath == null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(studentToUpdate.ImageFile.FileName);
                        string extension = Path.GetExtension(studentToUpdate.ImageFile.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        studentToUpdate.ImagePath = "~/Images/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        studentToUpdate.ImageFile.SaveAs(fileName);
                    }
                    db.SaveChanges();
                    TempData["success"] = "Успешно редактиран студент!";
                    return RedirectToAction("Index");
                }
                catch (DataException)
                {
                    TempData["error"] = "Неуспешно редактиран студент!";
                    ModelState.AddModelError("", "Промените не могат да бъдат запазени. Опитайте отново и ако проблемът продължава, обърнете се към Вашия системен администратор.");
                }
            }
            ViewBag.SpecialtyId = new SelectList(db.Specialties.OrderByDescending(s => s.Name), "SpecialtyId", "Name", studentToUpdate.SpecialtyId);
            return View(studentToUpdate);
        }

        [AuthorizationFilter("Admin")]
        // GET: Student/Delete/5
        public ActionResult Delete(int? id)
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

        [AuthorizationFilter("Admin")]
        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            string fileName = student.ImagePath.Remove(0, 9);
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo != null)
            {
                System.IO.File.Delete(fileName);
                fileInfo.Delete();
            }
            db.Students.Remove(student);
            db.SaveChanges();
            TempData["success"] = "Успешно изтрит студент!";
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
