using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using UniCheckManagementSystem.DAL;

namespace UniCheckManagementSystem.Infrastructure
{
    public class AuthorizationFilter : AuthorizeAttribute
    {
        private UniCheckDbContext db = new UniCheckDbContext();
        private readonly string[] allowedRoles;

        public AuthorizationFilter(params string[] roles)
        {
            allowedRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var userId = (int)httpContext.Session["UserId"];
            if (!userId.ToString().IsNullOrWhiteSpace())
            {
                var userRole = (from u in db.Users
                                join r in db.Roles on u.RoleId equals r.RoleId
                                where u.UserId == userId
                                select new
                                {
                                    r.Name
                                }).FirstOrDefault();
                foreach (var role in allowedRoles)
                {
                    if (role == userRole.Name) return true;
                }
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Redirecting the user to the UnAuthorized View of Home Controller.
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    { "controller", "Home" },
                    { "action", "UnAuthorized" }
            });
        }
    }
}