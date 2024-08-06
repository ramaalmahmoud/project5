﻿using election_website.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace election_website.Controllers
{
    public class AdminController : Controller
    {



        private readonly ElectionEntities db = new ElectionEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Lists()
        {
            // Get the local lists and their associated candidates
            var localLists = db.localLists.ToList();
            var localCandidatesByList = db.LocalCandidates
                                .Where(c => c.List_Name != null) // Filter out null List_Name values
                                .GroupBy(c => c.List_Name)
                                .ToDictionary(g => g.Key, g => g.ToList());


            // Get the party lists and their associated candidates
            var partyLists = db.PartyLists.ToList();
            var partyCandidatesByList = db.PartyCandidates
                                           .GroupBy(c => c.PartyLIST_ID)
                                           .ToDictionary(g => g.Key, g => g.ToList());

            // Create and populate the view model
            var viewModel = new ListViewModel
            {
                LocalLists = localLists,
                CandidatesByList = localLists.ToDictionary(
                    list => list.ID,
                    list => !string.IsNullOrEmpty(list.list_Name) && localCandidatesByList.ContainsKey(list.list_Name)
                        ? localCandidatesByList[list.list_Name]
                        : new List<LocalCandidate>()
                ),
                PartyLists = partyLists,
                PartyCandidatesByList = partyLists.ToDictionary(
                    list => list.ID,
                    list => partyCandidatesByList.ContainsKey(list.ID)
                        ? partyCandidatesByList[list.ID]
                        : new List<PartyCandidate>()
                )
            };
          
            return View(viewModel);
        }
        private void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("techlearnhub.contact@gmail.com", "lyrlogeztsxclank"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("ramaoudat151@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the error or handle it accordingly
                Console.WriteLine("Failed to send email: " + ex.Message);
                throw; // Re-throw the exception to handle it in the calling method
            }
        }


        [HttpPost]
        public ActionResult Approve(int listId, string message)
        {
            var partyList = db.PartyLists.FirstOrDefault(x => x.ID == listId);
            var localList = db.localLists.FirstOrDefault(x => x.ID == listId);

            if (partyList == null && localList == null)
            {
                return Json(new { success = false, message = "List not found" });
            }

            string delegateEmail = string.Empty;

            // Check if it's a party list or a local list and get the correct email
            if (partyList != null)
            {
                delegateEmail = partyList.Delegate_Email;
                partyList.Status = "Approved"; // Update the status
            }
            else if (localList != null)
            {
                delegateEmail = localList.Delegate_Email;
                localList.Status = "Approved"; // Update the status
            }

            // Send the email
            try
            {
                SendEmail(delegateEmail, "Approval Notification", message);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult Reject(int listId, string message)
        {
            var partyList = db.PartyLists.FirstOrDefault(x => x.ID == listId);
            var localList = db.localLists.FirstOrDefault(x => x.ID == listId);

            if (partyList == null && localList == null)
            {
                return Json(new { success = false, message = "List not found" });
            }

            string delegateEmail = string.Empty;

            // Check if it's a party list or a local list and get the correct email
            if (partyList != null)
            {
                delegateEmail = partyList.Delegate_Email;
                partyList.Status = "Rejected"; // Update the status
            }
            else if (localList != null)
            {
                delegateEmail = localList.Delegate_Email;
                localList.Status = "Rejected"; // Update the status
            }

            // Send the rejection email
            try
            {
                SendEmail(delegateEmail, "Rejection Notification", message);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}