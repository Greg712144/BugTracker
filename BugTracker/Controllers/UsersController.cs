using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private UserRolesHelper roleHelper = new UserRolesHelper();

        private ProjectHelper projHelper = new ProjectHelper();

        // GET: Users
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: Users/Create
       
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,DisplayName,AvatarPath,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(applicationUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var currentRole = roleHelper.ListUserRoles(id).FirstOrDefault();

            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name", currentRole);
            return View(applicationUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName, LastName, Email, DisplayName,EmailConfirmed,PhoneNumber,PhoneNumberConfirmed")] ApplicationUser applicationUser, string roles)
        {
            if (ModelState.IsValid)
            {

                //1st get list of roles user occupies
             
                var currentRoles = roleHelper.ListUserRoles(applicationUser.Id);
                foreach(var role in currentRoles)
                {
                    //Removes users from all and any role they occupy
                    roleHelper.RemoveUserFromRole(applicationUser.Id, role);
                }

                if (!string.IsNullOrEmpty(roles))
                    roleHelper.AddUserToRole(applicationUser.Id, roles);

                
                //db.Entry(applicationUser).State = EntityState.Modified;
                db.Users.Attach(applicationUser);
                applicationUser.UserName = applicationUser.Email;
                
                // times 7 for the properties that are subject to change, this allows for only for the ones to be changed instead of losing the information 
                db.Entry(applicationUser).Property(x => x.FirstName).IsModified = true;
                db.Entry(applicationUser).Property(x => x.LastName).IsModified = true;
                db.Entry(applicationUser).Property(x => x.DisplayName).IsModified = true;
                db.Entry(applicationUser).Property(x => x.AvatarPath).IsModified = true;
                db.Entry(applicationUser).Property(x => x.Email).IsModified = true;
                db.Entry(applicationUser).Property(x => x.PhoneNumber).IsModified = true;
                db.Entry(applicationUser).Property(x => x.UserName).IsModified = true;
                

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
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
