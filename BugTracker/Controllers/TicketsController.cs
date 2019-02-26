using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private TicketHelper tickHelper = new TicketHelper();
        private ProjectHelper projHelper = new ProjectHelper();
        private UserRolesHelper roleHelper = new UserRolesHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();
        private HistoryHelper historyHelper = new HistoryHelper();

        // GET: Tickets
        
        public ActionResult Index()
        {

            var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType).Include(t => t.Users);
         
            return View(tickets);
        }

        public ActionResult myTickets()
        {
            var userId = User.Identity.GetUserId();
            var myTickets = db.Tickets.Where(t => t.AssignedToUserId == userId);

            return View(myTickets.ToList());
        }

        public ActionResult subTickets()
        {
            var userId = User.Identity.GetUserId();
            var myTickets = db.Tickets.Where(t => t.OwnerUserId == userId);

            return View(myTickets.ToList());
        }


        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = User.Identity.GetUserId();
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            if (User.IsInRole("Admin") || ticket.AssignedToUserId == userId || ticket.OwnerUserId == userId)
            {
                return View(ticket);
            }
            if(ticket.AssignedToUserId != userId || ticket.OwnerUserId != userId)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var myProjects = projHelper.ListUserProjects(userId);


            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            if (User.IsInRole("Submitter"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,OwnerUserId,AssignedToUserId,AssignedToUserTwoId,Title,Description")] ApplicationUser applicationUser, Ticket ticket, string ticketId, Project project, string projectId, string OwnerUserId)
        {
            if (ModelState.IsValid)
            {

                ticket.Created = DateTime.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var userId = User.Identity.GetUserId();
            var myProjects = projHelper.ListUserProjects(userId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);

            return View();
        }

        // GET: Tickets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
           
            if (id == null)
            { 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }


            var userId = User.Identity.GetUserId();
            var tAssigned = roleHelper.UsersInRole("Developer");
            var myProjects = projHelper.ListUserProjects(userId);
            var myTickets = tickHelper.ListUserTickets();

            ViewBag.Id = new SelectList(myTickets, "Id", "Name", ticket.Id);
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name", ticket.ProjectId);
            ViewBag.AssignedToUserId = new SelectList(tAssigned, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,Progress,OwnerUserId,AssignedToUserId,AssignedToUserTwoId,Title,Description,Created,Updated")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //Get a reference to the Old Ticket
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);
                //Compare it to the incoming Ticket(ticket)

                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.Created = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                await notificationHelper.Notify(oldTicket, ticket);
                historyHelper.AddHistory(oldTicket, ticket);

                return RedirectToAction("Index", "Home");
            }


            var userId = User.Identity.GetUserId();
            var tAssigned = roleHelper.UsersInRole("Developer");
            var myProjects = projHelper.ListUserProjects(userId);
            var myTickets = tickHelper.ListUserTickets();
            

            ViewBag.Id = new SelectList(myTickets, "Id", "FirstName", ticket.Id);
            ViewBag.AssignedToUserId = new SelectList(tAssigned, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatuses, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }
 
        // GET: Tickets/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userId = User.Identity.GetUserId();
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin") || ticket.AssignedToUserId == userId || ticket.OwnerUserId == userId)
            {
                return View(ticket);
            }
            if (ticket.AssignedToUserId != userId || ticket.OwnerUserId != userId)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: Tickets/Assigned
        [Authorize]
        public ActionResult AssignedT()
        {
            var userId = User.Identity.GetUserId();
            var myTickets = db.Tickets.Where(t => t.AssignedToUserId == userId);

            return View(myTickets.ToList());
        }

        // POST: Tickets/Assigned
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignedT([Bind(Include = "Id,Title,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(ticket);
        }
    }
}
