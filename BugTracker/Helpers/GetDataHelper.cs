using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class GetDataHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<Ticket> Tickets()
        {
            var tickets = db.Tickets.ToList();
            return (tickets);
        }

        public List<Project> Projects()
        {
            var projects = db.Projects.ToList();
            return (projects);
        }

        public List<ApplicationUser> Users()
        {
            var users = db.Users.ToList();
            return (users);
        }
    }
}