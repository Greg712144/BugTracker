using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        
 
        

        public virtual ICollection<Ticket> Ticket { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
      



        public Project()
        {
            Ticket = new HashSet<Ticket>();
            Users = new HashSet<ApplicationUser>();
            
       
        }


    }
}