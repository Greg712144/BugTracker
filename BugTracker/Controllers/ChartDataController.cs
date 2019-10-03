using BugTracker.Helpers;
using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;


namespace BugTracker.Controllers
{
    public class ChartDataController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketHelper tick = new TicketHelper();
  

        // GET: ChartData
        public JsonResult GetBarChartData()
        {
            var userId = User.Identity.GetUserId();
            var tickets = db.Tickets.ToList();

            var lCount = 0;
            var mCount = 0;
            var hCount = 0;
            var uCount = 0;

            if(User.IsInRole("Admin") || User.IsInRole("Project Manager"))
            {
                

                foreach (var tick in tickets)
                {
                    if (tick.TicketPriority.Name == "Low")
                    {
                        lCount++;
                    }
                    if (tick.TicketPriority.Name == "Medium")
                    {
                        mCount++;
                    }
                    if (tick.TicketPriority.Name == "High")
                    {
                        hCount++;
                    }
                    if (tick.TicketPriority.Name == "Urgent")
                    {
                        uCount++;
                    }
                }
            }
            else if(User.IsInRole("Developer"))
            {

                foreach (var tick in tickets.Where(t => t.AssignedToUserId == userId))
                {
                    if (tick.TicketPriority.Name == "Low")
                    {
                        lCount++;
                    }
                    if (tick.TicketPriority.Name == "Medium")
                    {
                        mCount++;
                    }
                    if (tick.TicketPriority.Name == "High")
                    {
                        hCount++;
                    }
                    if (tick.TicketPriority.Name == "Urgent")
                    {
                        uCount++;
                    }
                }
            }
            else if (User.IsInRole("Submitter"))
            {

                foreach (var tick in tickets.Where(t => t.OwnerUserId == userId))
                {
                    if (tick.TicketPriority.Name == "Low")
                    {
                        lCount++;
                    }
                    if (tick.TicketPriority.Name == "Medium")
                    {
                        mCount++;
                    }
                    if (tick.TicketPriority.Name == "High")
                    {
                        hCount++;
                    }
                    if (tick.TicketPriority.Name == "Urgent")
                    {
                        uCount++;
                    }
                }
            }


            var data = new BarChartData
            {

                Values = { lCount, mCount, hCount, uCount },
                Labels = {"Low", "Medium", "High", "Urgent" }
            };
            
            return Json(data);
            
        }

        //Get: DoughnutData
        public JsonResult GetDoughnutChartData()
        {

            //Collect necessary data
            var userId = User.Identity.GetUserId();
            var tickets = db.Tickets.ToList();
            //create variable (x3) to convert to int
            var bCount = 0;
            var fCount = 0;
            var dCount = 0;

            //loop over tickets , and add 
            if(User.IsInRole("Admin") || User.IsInRole("Project Manager"))
            {

                foreach (var tick in tickets)
                {
                    if (tick.TicketType.Name == "Bug")
                    {
                        bCount++;
                    }
                    if (tick.TicketType.Name == "Feature Update")
                    {
                        fCount++;
                    }
                    if (tick.TicketType.Name == "Documentation Request")
                    {
                        dCount++;
                    }
                }
            }
            else if (User.IsInRole("Developer"))
            {

                foreach (var tick in tickets.Where(t => t.AssignedToUserId == userId))
                {
                    if (tick.TicketType.Name == "Bug")
                    {
                        bCount++;
                    }
                    if (tick.TicketType.Name == "Feature Update")
                    {
                        fCount++;
                    }
                    if (tick.TicketType.Name == "Documentation Request")
                    {
                        dCount++;
                    }
                }
            }
            else if (User.IsInRole("Submitter"))
            {

                foreach (var tick in tickets.Where(t => t.OwnerUserId == userId))
                {
                    if (tick.TicketType.Name == "Bug")
                    {
                        bCount++;
                    }
                    if (tick.TicketType.Name == "Feature Update")
                    {
                        fCount++;
                    }
                    if (tick.TicketType.Name == "Documentation Request")
                    {
                        dCount++;
                    }
                }
            }

            var data = new DoughnutChartData
            {
                Values = { bCount,fCount,dCount },        
                Labels = {"Bug","Feature Update", "Documentation Request" }

            };

            return Json(data);
        }
    }
}