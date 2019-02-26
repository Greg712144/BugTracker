using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class DataViewModel
    {
        public virtual ICollection<Project> myProjects { get; set; }
        public virtual ICollection<Ticket> myTickets { get; set; }
        public virtual ICollection<ApplicationUser> myUsers { get; set; }
        public string AvatarPath { get; set; }

        public DataViewModel()
        {
            myProjects = new HashSet<Project>();
            myTickets = new HashSet<Ticket>();
            myUsers = new HashSet<ApplicationUser>();
        }


    }
}