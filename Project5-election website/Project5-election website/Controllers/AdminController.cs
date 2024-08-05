using Microsoft.Extensions.Configuration;
using Project5_election_website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web.Mvc;

namespace Project5_election_website.Controllers
{
    public class AdminController : Controller
    {
        public class EmailService
        {
            private readonly ElectionEntities _context; // Your database context
            private readonly IConfiguration _configuration;

            public EmailService(ElectionEntities context, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            public void SendEmailToCandidate(int delegateid, string subject, string body)
            {
                // Fetch the candidate's email address from the database
                var candidate = _context.localLists
                    .Where(c => c.Delegate_ID == delegateid)
                    .FirstOrDefault();

                if (candidate == null)
                {
                    throw new Exception("Candidate not found");
                }

                var toEmail = candidate.Delegate_Email;

                // SMTP settings
                var smtpHost = _configuration["SMTP:Host"];
                var smtpPort = Convert.ToInt32(_configuration["SMTP:Port"]);
                var smtpUsername = _configuration["SMTP:Username"];
                var smtpPassword = _configuration["SMTP:Password"];

                // Send email
                var smtpClient = new SmtpClient(smtpHost)
                {
                    Port = smtpPort,
                    Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpUsername),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);

                smtpClient.Send(mailMessage);
            }
        }


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
    }
}
