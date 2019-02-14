using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Ticket> Tickets;

        public TicketStatus()
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}