using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Helpers
{
    

    public class LookupHelper
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static string GetNameFromId (string propertyName, string id)
        {
            var name = "";
            //switch(propertyName)
            //{
            //    case "TicketPriorityId":
            //        name = db.TicketPriorities.Find(Convert.ToInt32(id)).Name;
            //        break;
            //    case "TicketStatuses":
            //        name = db.TicketStatuses.Find(Convert.ToInt32(id)).Name;
            //        break;
            //    case "TicketTypeId":
            //        name = db.TicketTypes.Find(Convert.ToInt32(id)).Name;
            //        break;
            //    case "AssignedToUserId":
            //        name = db.Users.Find(id).FirstName;
            //        break;
            //}

            if(propertyName == "TicketPriorityId")
            {
                name = db.TicketPriorities.Find(Convert.ToInt32(id)).Name;
            }
            else if(propertyName == "TicketStatusId")
            {
                name = db.TicketStatuses.Find(Convert.ToInt32(id)).Name;
            }
            else if (propertyName == "TicketTypeId")
            {
                name = db.TicketTypes.Find(Convert.ToInt32(id)).Name;
            }
            else if(propertyName == "AssignedToUserId")
            {
                name = db.Users.Find(id).FirstName;
            }

            return name;
        }

    }
}