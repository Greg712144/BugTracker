namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }



            context.TicketPriorities.AddOrUpdate(
                p => p.Name,
                    
                    new TicketPriority { Name = "Low" },
                    new TicketPriority { Name = "Medium" },
                    new TicketPriority { Name = "High" },
                    new TicketPriority { Name = "Urgent" }
            );


            context.TicketStatuses.AddOrUpdate(
                p => p.Name,
                    new TicketStatus { Name = "Open" },
                    new TicketStatus { Name = "Closed" },
                    new TicketStatus { Name = "In Progress" },
                    new TicketStatus { Name = "Completed" }
            );


            context.TicketTypes.AddOrUpdate(
                p => p.Name,
                    new TicketType { Name = "Bug" },
                    new TicketType { Name = "Feature Update" },
                    new TicketType { Name = "Documentation Request" }
            );

            context.Projects.AddOrUpdate(
                p => p.Name,
                        new Project { Name = "Case of Bigfoot", Description = "Will you help me catch him?"},
                        new Project { Name = "Understanding Software", Description = "Here we will go in depth into the intricacies of Software Development"},
                        new Project { Name = "BugTracker", Description = "Brief intro to Bug-Tracking" }
                     
                );


            //Create a few users in the project

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "gzambrana93@outlook.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "gzambrana93@outlook.com",
                    Email = "gzambrana93@outlook.com",
                    FirstName = "Gregorio",
                    LastName = "Zambrana",
                    DisplayName = "Greg"
                }, "Abc&123!");

                //Associate a User to a role
            }
            var userId = userManager.FindByEmail("gzambrana93@outlook.com").Id;
            userManager.AddToRole(userId, "Admin");



            if (!context.Users.Any(u => u.Email == "Dev@outlook.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "Dev@outlook.com",
                    Email = "Dev@outlook.com",
                    FirstName = "Dev",
                    LastName = "Zamban",
                    DisplayName = "Dev"
                }, "Abc&123!");

                //Associate a User to a role
            }
            var DevId = userManager.FindByEmail("Dev@outlook.com").Id;
            userManager.AddToRole(userId, "Developer");



            if (!context.Users.Any(u => u.Email == "Dev2@outlook.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "Dev2@outlook.com",
                    Email = "Dev2@outlook.com",
                    FirstName = "Dev2",
                    LastName = "Zambern",
                    DisplayName = "Dev2"
                }, "Abc&123!");

                //Associate a User to a role
            }
            DevId = userManager.FindByEmail("Dev2@outlook.com").Id;
            userManager.AddToRole(userId, "Developer");



            if (!context.Users.Any(u => u.Email == "Dev3@outlook.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "Dev3@outlook.com",
                    Email = "Dev3@outlook.com",
                    FirstName = "Dev3",
                    LastName = "Zamben",
                    DisplayName = "Dev3"
                }, "Abc&123!");

                //Associate a User to a role
            }
            DevId = userManager.FindByEmail("Dev3@outlook.com").Id;
            userManager.AddToRole(userId, "Developer");




            if (!context.Users.Any(u => u.Email == "PM@outlook.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "PM@outlook.com",
                    Email = "PM@outlook.com",
                    FirstName = "PM",
                    LastName = "Zamba",
                    DisplayName = "PM"
                }, "Abc&123!");

                //Associate a User to a role
            }
            var PmId = userManager.FindByEmail("PM@outlook.com").Id;
            userManager.AddToRole(userId, "Project Manager");




            if (!context.Users.Any(u => u.Email == "PM2@outlook.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "PM2@outlook.com",
                    Email = "PM2@outlook.com",
                    FirstName = "PM2",
                    LastName = "Zamber",
                    DisplayName = "PM2"
                }, "Abc&123!");

                //Associate a User to a role
            }
            PmId = userManager.FindByEmail("PM2@outlook.com").Id;
            userManager.AddToRole(userId, "Project Manager");



            if (!context.Users.Any(u => u.Email == "PM3@outlook.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "PM3@outlook.com",
                    Email = "PM3@outlook.com",
                    FirstName = "PM3",
                    LastName = "Za",
                    DisplayName = "PM3"
                }, "Abc&123!");

                //Associate a User to a role
            }
            PmId = userManager.FindByEmail("PM3@outlook.com").Id;
            userManager.AddToRole(userId, "Project Manager");



            if (!context.Users.Any(u => u.Email == "S@outlook.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "S@outlook.com",
                    Email = "S@outlook.com",
                    FirstName = "Sub",
                    LastName = "Za",
                    DisplayName = "Sub"
                }, "Abc&123!");

                //Associate a User to a role
            }
            var SubId = userManager.FindByEmail("S@outlook.com").Id;
            userManager.AddToRole(userId, "Submitter");



            if (!context.Users.Any(u => u.Email == "S2@outlook.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "S2@outlook.com",
                    Email = "S2@outlook.com",
                    FirstName = "Sub2",
                    LastName = "Za",
                    DisplayName = "Sub2"
                }, "Abc&123!");

                //Associate a User to a role
            }
            SubId = userManager.FindByEmail("S2@outlook.com").Id;
            userManager.AddToRole(userId, "Submitter");



            if (!context.Users.Any(u => u.Email == "S3@outlook.com"))
            {

                userManager.Create(new ApplicationUser
                {
                    UserName = "S3@outlook.com",
                    Email = "S3@outlook.com",
                    FirstName = "Sub3",
                    LastName = "Za",
                    DisplayName = "Sub3"
                }, "Abc&123!");

                //Associate a User to a role
            }
            SubId = userManager.FindByEmail("S3@outlook.com").Id;
            userManager.AddToRole(userId, "Submitter");

        }
    }
}
