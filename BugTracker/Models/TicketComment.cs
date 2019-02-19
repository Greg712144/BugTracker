using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }

        public int TicketId { get; set; }
        public string UserId { get; set; }

        [AllowHtml]
        public string CommentBody { get; set; }
        public DateTime Created { get; set; }
        public bool Read { get; set; }

        //Navigational Properties
        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}