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
    public class SpecialtyController : Controller
    {
        private UniCheckDbContext db = new UniCheckDbContext();

        // GET: Specialty
        public ActionResult Index()
        {
            return View(db.Specialties.ToList());
        }

        // GET: Specialty/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specialty specialty = db.Specialties.Find(id);
            if (specialty == null)
            {
                return HttpNotFound();
            }
            return View(specialty);
        }

        // GET: Specialty/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Specialty/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SpecialtyId,Name,ProfessionalStream,Degree,Qualification,ModeOfStudy,Length")] Specialty specialty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Specialties.Add(specialty);
                    db.SaveChanges();
                    TempData["success"] = "Успешно добавена специалност!";
                    return RedirectToAction("Index");

                }
            }
            catch (DataException)
            {
                TempData["error"] = "Неуспешно добавена специалност!";
                ModelState.AddModelError("", "Промените не могат да бъдат запазени. Опитайте отново и ако проблемът продължава, обърнете се към Вашия системен администратор.");
            }
            return View(specialty);
        }

        // GET: Specialty/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specialty specialty = db.Specialties.Find(id);
            if (specialty == null)
            {
                return HttpNotFound();
            }
            return View(specialty);
        }

        // POST: Specialty/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SpecialtyId,Name,ProfessionalStream,Degree,Qualification,ModeOfStudy,Length")] Specialty specialty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(specialty).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["success"] = "Успешно редактирана специалност!";
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                TempData["error"] = "Неуспешно редактирана специалност!";
                ModelState.AddModelError("", "Промените не могат да бъдат запазени. Опитайте отново и ако проблемът продължава, обърнете се към Вашия системен администратор.");
            }
            return View(specialty);
        }

        // GET: Specialty/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specialty specialty = db.Specialties.Find(id);
            if (specialty == null)
            {
                return HttpNotFound();
            }
            return View(specialty);
        }

        // POST: Specialty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Specialty specialty = db.Specialties.Find(id);
            db.Specialties.Remove(specialty);
            db.SaveChanges();
            TempData["success"] = "Успешно изтрита специалност!";
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
