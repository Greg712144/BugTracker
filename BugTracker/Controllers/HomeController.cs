using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using PagedList;
using PagedList.Mvc;

namespace BugTracker.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        [Authorize]
        public ActionResult Index()
        {

            var data = new DataViewModel();
            data.myProjects = db.Projects.ToList();
            data.myTickets = db.Tickets.ToList();
            data.myUsers = db.Users.ToList();

            return View(data);
        }

        public ActionResult Login()
        {
     
            return View();
        }

        public ActionResult Register()
        {
           
            return View();
        }

        public ActionResult ReadNotification(int Id)
        {
            var notify = db.TicketNotifications.Find(Id);
            notify.Read = true;
            db.Entry(notify).Property(n => n.Read).IsModified = true;
            db.SaveChanges();
            return Redirect(Request.ServerVariables["http_referer"]);
        }
        //HTTP : GET
        public ActionResult Contact()
        {
            EmailModel model = new EmailModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Contact(EmailModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var body = "<p>Email From:<bold>{0}</bold><p>({1})</p><p>Mesaage:</p><p>{2}</p>";
                    var emailto = ConfigurationManager.AppSettings["emailto"];
                    var from = string.Format("Zblog<{0}>", emailto);

                    model.Body = "This is a message from your blog site. The name and the email of the contacting person is above." + model.Body;

                    var email = new MailMessage(from, emailto)
                    {
                        Subject = model.Subject,
                        Body = string.Format(body, model.FromName, model.FromEmail, model.Body),
                        IsBodyHtml = true
                    };

                    var svc = new PersonalEmail();
                    await svc.SendAsync(email);

                    return View(new EmailModel());
                }

                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                    await Task.FromResult(0);
                }

            }

            return View(model);

        }

    }
}
