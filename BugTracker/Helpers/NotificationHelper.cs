using BugTracker.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace BugTracker.Helpers
{
    public class NotificationHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();


        public async Task Notify (Ticket oldTicket, Ticket newTicket)
        {
            // The purpose of this method is to determine whtehr or not the system need to genereate a ticket notifications record

            var oldUserId = oldTicket.AssignedToUserTwoId;
            var newUserId = newTicket.AssignedToUserTwoId;
           


            //If the oldUserId and the newUserId are the same then do nothing
            if (oldUserId == newUserId)
                return;


            //If I am here, this means I am generating a notification
            var newNotification = new TicketNotification
            {
                Created = DateTime.Now,
                TicketId = newTicket.Id,
               
            };

            //Condition 1: The ticket is newly assigned. OldTicket (not assigned), newTicket(assigned)
            if (oldUserId == null && newUserId != null)
            {
                //This condition triggers an assignment notification
                newNotification.RecipientId = newUserId;
                newNotification.Description = $"You've have been assigned to ticket {newTicket.Id}";

                await SendEmailNotification(newTicket.Id, oldTicket.AssignedToUserTwoId, newTicket.AssignedToUserTwoId);
                db.TicketNotifications.Add(newNotification);
                db.SaveChanges();
            }

            //Condition 2 : The ticket has been newly assigned. OldTicket(assigned), newTicket(unassigned)

            else if (oldUserId != null && newUserId == null)
            {
                newNotification.RecipientId = oldUserId;
                newNotification.Description = $"You've been reassigned from ticket {newTicket.Id}";

                await SendEmailNotification(newTicket.Id, oldTicket.AssignedToUserTwoId, newTicket.AssignedToUserTwoId);
                db.TicketNotifications.Add(newNotification);
                db.SaveChanges();
            }

            //Condition 3 : Neither the oldTicket or the newTicket are null. Ticket is reassigned
            else 
            {
                newNotification.RecipientId = newUserId;
                newNotification.Description = $"You have been assigned to ticket {newTicket.Id}";
                
                db.TicketNotifications.Add(newNotification);

                var secondNotification = new TicketNotification
                {
                    Created = DateTime.Now,
                    TicketId = newTicket.Id,
                    RecipientId = oldUserId,
                    Description = $"You have been unassigned from ticket {newTicket.Id}"

                };

                await SendEmailNotification(newTicket.Id, oldTicket.AssignedToUserTwoId, newTicket.AssignedToUserTwoId);
                db.TicketNotifications.Add(secondNotification);
                db.SaveChanges();
            }

        }
    

        public async Task SendEmailNotification(int ticketId, string oldUserId, string newUserId)
        {
            string from = $"System <System@BugTrackerz.com>";
            string subject = "", body = "", emailTo = "", nameTo = "";

            if(oldUserId == null && newUserId != null)
            {
                subject = "Ticket Assignment";
                body = $"You have been Assigned to Ticket Id {ticketId}";

                var newUser = db.Users.Find(newUserId);
                emailTo = newUser.Email;
                nameTo = newUser.FirstName;

                var assignedMailModel = new EmailModel
                {
                    FromEmail = from,
                    ToEmail = emailTo,
                    ToName = nameTo,
                    Body = body,
                    Subject = subject
                };
                await SendEmail(assignedMailModel);
            }

            else if (oldUserId != null && newUserId != null)
            {
                subject = "Ticket Assignment";
                body = $"You have been assigned to Ticket Id {ticketId}";
                emailTo = db.Users.Find(newUserId).Email;

                var reAssignedNewUser = db.Users.Find(newUserId);
                emailTo = reAssignedNewUser.Email;
                nameTo = reAssignedNewUser.FirstName;

                var reAssignedNewUserMailModel = new EmailModel
                {
                    FromEmail = from,
                    ToEmail = emailTo,
                    ToName = nameTo,
                    Body = body,
                    Subject = subject
                };

                await SendEmail(reAssignedNewUserMailModel);

                subject = "Ticket Unassignment";
                body = $"You have been UnAssigned from Ticket Id {ticketId}";
                emailTo = db.Users.Find(oldUserId).Email;

                var reAssignedOldUser = db.Users.Find(oldUserId);
                emailTo = reAssignedOldUser.Email;
                nameTo = reAssignedOldUser.FirstName;

                var reAssignedOldUserMailModel = new EmailModel
                {
                    FromEmail = from,
                    ToEmail = emailTo,
                    ToName = nameTo,
                    Body = body,
                    Subject = subject
                };

                await SendEmail(reAssignedOldUserMailModel);
            }
        }

        public async Task SendEmail(EmailModel emailData)
        {
            var email = new MailMessage(emailData.FromEmail, emailData.ToEmail)
            {
                Subject = emailData.Subject,
                Body = emailData.Body,
                IsBodyHtml = true
            };

            var svc = new PersonalEmail();
            await svc.SendAsync(email);
        }

        public List<TicketNotification> UnreadNotifications()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            return db.TicketNotifications.Where(t => t.RecipientId == userId && !t.Read).ToList();

        }


    }



 }

      