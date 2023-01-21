using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniCheckManagementSystem.DAL;
using UniCheckManagementSystem.Models;

namespace UniCheckManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private UniCheckDbContext db = new UniCheckDbContext();

        [HttpGet]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(User model)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users
                    .Where(u => u.UserName == model.UserName && u.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    Session["UserName"] = user.UserName;
                    Session["UserId"] = user.UserId;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Грешно потребителско име или парола.");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("LogIn", "Account");
        }
    }
}