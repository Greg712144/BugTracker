using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class HistoryHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void AddHistory(Ticket oldTicket, Ticket newTicket)
        {


            if(oldTicket.Title != newTicket.Title)
           {
                var history = CreateTicketHistory("Title", oldTicket.Title, newTicket.Title, newTicket.Id);
                db.TicketHistories.Add(history);
               
           }

            if(oldTicket.Description != newTicket.Description)
            {
                var history = CreateTicketHistory("Description", oldTicket.Description, newTicket.Description, newTicket.Id);
                db.TicketHistories.Add(history);

            }

            if(oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                var history = CreateTicketHistory("TicketTypeId", oldTicket.TicketTypeId.ToString(), newTicket.TicketTypeId.ToString(), newTicket.Id);
                db.TicketHistories.Add(history);

            }

            if(oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var history = CreateTicketHistory("TicketPriorityId", oldTicket.TicketPriorityId.ToString(), newTicket.TicketPriorityId.ToString(), newTicket.Id);
                db.TicketHistories.Add(history);

            }

            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var history = CreateTicketHistory("TicketStatusId", oldTicket.TicketStatusId.ToString(), newTicket.TicketStatusId.ToString(), newTicket.Id);
                db.TicketHistories.Add(history);

            }

            db.SaveChanges();
  
        }

        public TicketHistory CreateTicketHistory(string propertyName, string oldValue, string newValue, int ticketId)
        {
            var history = new TicketHistory
            {
                Changed = DateTime.Now,
                TicketId = ticketId,
                PropertyName = propertyName,
                OldValue = oldValue,
                NewValue = newValue,
                UserId = HttpContext.Current.User.Identity.GetUserId()
            };

            return history;
        }
    }
}