using BugTracker.Models;
using BugTracker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Routing;

namespace BugTracker.ActionFilters
{
    public class TicketAuthorization : ActionFilterAttribute
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private TicketHelper tickHelper = new TicketHelper();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var ticketId = Convert.ToInt32(filterContext.ActionParameters.SingleOrDefault(p => p.Key == "id").Value);
            var ticket = db.Tickets.Find(ticketId);
            string userId = HttpContext.Current.User.Identity.GetUserId();
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();

            if(userId == null)
            {
                
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            }
            else if(ticket == null)
            {
               
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            }
            else if(myRole == "Developer" && ticket.AssignedToUserId != userId || myRole == "Submitter" && ticket.OwnerUserId != userId)
            {
                //var userName = db.Users.Find(userId).FirstName;
                //var phrase = myRole == "Developer" ? "are not assigned to" : "do not own";
                //var msg = string.Format("Heloo {0}, it looks like you are a {1} trying to edit a Ticket that you {2}. Please contact the Project Manager to resolve this issue.", userName, myRole, phrase);
                //filterContext.Controller.TempData.Add("oopsMsg", msg);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            }
            //else if (myRole == "ProjectManager")
            //{
            //    if (!ticketHelper.IsTicketOnMyProjects(userId, ticket.Id))
            //    {
            //        var userName = db.Users.Find(userId).DisplayName;
            //        var msg = string.Format("Hello {0}, it looks like you are trying to Edit a Ticket on a Project you are not assigned to. Please contact an Admin to help resolve this issue.", userName);
            //        filterContext.Controller.TempData.Add("oopsMsg", msg);
            //        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "Oops" } });
            //    }
            //}

            base.OnActionExecuting(filterContext);
        }

    }
}