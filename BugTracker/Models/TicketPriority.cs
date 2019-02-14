using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models
{
    public class TicketPriority
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Ticket> Tickets;

        public TicketPriority()
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}