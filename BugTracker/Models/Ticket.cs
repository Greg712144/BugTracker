﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int TicketTypeId { get; set; }
        public int TicketStatusId { get; set; }
        public int TicketPriorityId { get; set; }

       
        public string OwnerUserId { get; set; }
        public string AssignedToUserId { get; set; }
        public string AssignedToUserTwoId { get; set; }

        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string Role { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
       


        //Naviagtional Properties
        //Parent Sections
        public virtual Project Project { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }

        
        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }
        public virtual ApplicationUser AssignedToUserTwo { get; set; }


        //Children Sections

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }


        //Constructor to instantiate each ICollection

        public Ticket ()
        {
            Users = new HashSet<ApplicationUser>();
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketComments = new HashSet<TicketComment>();
            TicketHistories = new HashSet<TicketHistory>();
            TicketNotifications = new HashSet<TicketNotification>();


        }


    }
}