using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class NotificationViewModel
    {
        public List<Ticket> Tickets { get; set; }
        public List<TicketNotification> Notifications { get; set; }
    }
}