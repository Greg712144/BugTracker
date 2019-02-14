﻿using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private ProjectHelper projHelper = new ProjectHelper();
        private TicketHelper tickHelper = new TicketHelper();

        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult AssignRole()
        {
            //I want to load a Viewbag that holds each of th Users in the system    
            ViewBag.Users = new SelectList(db.Users, "Id", "FirstName");

            //I want to load a Viewbag that holds each of th Roles in the system
            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name");

            //I want to load a Viewbag that holds eack of the projects in the system
            ViewBag.Projects = new MultiSelectList(db.Projects, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignRole(string users, string roles)
        {
            if (ModelState.IsValid)
            {

                var currentUser = db.Users.Find(users).Id;
                var currentRoles = roleHelper.ListUserRoles(users);
                foreach (var role in currentRoles)
                { 
                    //Removes users from all and any role they occupy
                    roleHelper.RemoveUserFromRole(currentUser, role);
                
                    
                }

                if (!string.IsNullOrEmpty(roles))
                { 
                    roleHelper.AddUserToRole(currentUser, roles);
                   

                }
            }
                return RedirectToAction("Index", "Users");
            
        }

        //Get
        [Authorize(Roles = "Admin, Project Manager")]
        public ActionResult AssignProject()
        {
            var Pm = roleHelper.UsersInRole("ProjectManager");
            ViewBag.ProjectManager = new SelectList(Pm, "Id", "Email");

            var Dev = roleHelper.UsersInRole("Developer");
            ViewBag.Developers = new MultiSelectList(Dev, "Id", "Email");

            var Sub = roleHelper.UsersInRole("Submitter");
            ViewBag.Submitters = new MultiSelectList(Sub, "Id", "Email");


            //I want to load a Viewbag that holds each of the Projects in the system
            ViewBag.Project = new SelectList(db.Projects, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignProject(int project, string ProjectManager, List<string> Developers, List<string> Submitters)
        {
            if (ModelState.IsValid)
            {
                var projUsers = projHelper.UsersOnProject(project);
              
                
                foreach(var user in projUsers.ToList())
                {
                    projHelper.RemoveUserFromProject(user.Id, project);
                }

                projHelper.AddUserToProject(ProjectManager, project);
                
                if (Developers != null)
                {
                    foreach (var user in Developers)
                    {
                        projHelper.AddUserToProject(user, project);
                    }
                }


                if (Submitters != null)
                {
                    foreach (var user in Submitters)
                    {
                        projHelper.AddUserToProject(user, project);
                    }
                }

                return RedirectToAction("AP", "Projects");
            }
            return RedirectToAction("AP", "Projects");

        }

        //Get
        [Authorize(Roles = "Admin")]
        public ActionResult AssignTicket()
        {
            var PmId = roleHelper.UsersInRole("ProjectManager");
            ViewBag.ProjectManager = new SelectList(PmId, "Id", "FirstName");

            var DevId = roleHelper.UsersInRole("Developer");
            ViewBag.Developer = new SelectList(DevId, "Id", "FirstName");

            //I want to load a Viewbag that holds each of the Projects in the system
            ViewBag.Ticket = new SelectList(db.Tickets, "Id", "Title");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTicket(int ticket, string ProjectManager, string Developer)
        {
            if (ModelState.IsValid)
            {
                var myTicket = db.Tickets.Find(ticket);
                db.Tickets.Attach(myTicket);
                myTicket.AssignedToUserId = ProjectManager;
                myTicket.AssignedToUserTwoId = Developer;

                db.SaveChanges();


            }

            

            return RedirectToAction("Index", "Tickets");

        }

    }
}