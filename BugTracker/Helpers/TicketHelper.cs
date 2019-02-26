using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    public class TicketHelper
    {
        private UserRolesHelper rolehelper = new UserRolesHelper();
        ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserOnTicket(string userId, int ticketId)
        {

            var ticket = db.Tickets.Find(ticketId);
            var flag = ticket.Users.Any(u => u.Id == userId);
            return (flag);
        }

   
        public void AddUserToTicket(string userId, int ticketId)
        {
            if(!IsUserOnTicket(userId, ticketId))
            {
                Ticket tick = db.Tickets.Find(ticketId);
                var newUser = db.Users.Find(userId);

                tick.Users.Add(newUser);
                
                db.Entry(tick).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void RemoveUserFromTicket(string userId, int ticketId)
        {
            if(IsUserOnTicket(userId, ticketId))
            {
                Ticket tick = db.Tickets.Find(ticketId);
                var delUser = db.Users.Find(userId);

                tick.Users.Remove(delUser);
                db.Entry(tick).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ICollection<Ticket> ListUserTickets()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var tickets = db.Tickets.Where(t => t.AssignedToUserId == userId).ToList();
           return (tickets);
        }

        public ICollection<Ticket> ListSubTickets()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var tickets = db.Tickets.Where(t => t.OwnerUserId == userId).ToList();
            return (tickets);
        }

        public ICollection<ApplicationUser> UsersOnTicket(int ticketId)
        {
            return db.Tickets.Find(ticketId).Users;
        }

        

        //public ICollection<ApplicationUser> UsersNotOnTicket()
        //{
        //    var userId = db.Users.ToString();
        //    return db.Tickets.Where(t => t.Users.All(u => u.Id != userId)).ToList();
        //    return db.Users.Where(u => u.Tickets.All(p => p.Id != ticketId)).ToList();

        //}

        public ICollection<string>GetTicketUserRoles(string roleName, int ticketId )
        {
            var users = UsersOnTicket(ticketId);
            var usersInRole = new List<string>();
            var roleHelper = new UserRolesHelper();

            foreach(var user in users)
            {
                if(roleHelper.IsUserInRole(user.Id,roleName))
                {
                    usersInRole.Add(user.FirstName + " " + user.LastName);
                }

            }
            return usersInRole;
        }
    }
}