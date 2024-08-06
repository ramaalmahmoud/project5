using election_website.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Project5_election_website.Controllers
{

    public class CandidatesController : Controller
    {
        ElectionEntities db = new ElectionEntities();
        // GET: Candidates
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult localApplication()
        {
            return View();
        }
        [HttpPost]

        public ActionResult localApplication([Bind(Include = "ID,Circle_ID,list_Name,list_Candidates,Delegate_ID,Delegate_Name,Delegate_Phone,Delegate_Email")] localList localList, HttpPostedFileBase ListLogo, string CircleName)
        {



            if (ModelState.IsValid)
            {



                // UPLOAD A LOGO FOR THE LIST
                var uploadPath = Server.MapPath("~/Uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                if (ListLogo != null && ListLogo.ContentLength > 0)
                {
                    var listLogoFileName = Path.GetFileName(ListLogo.FileName);
                    var listLogoPath = Path.Combine(uploadPath, listLogoFileName);
                    ListLogo.SaveAs(listLogoPath);
                    localList.List_Logo = listLogoFileName;
                }
                db.localLists.Add(localList);
                db.SaveChanges();
                Session["list_name"] = localList.list_Name;
                Session["list_id"] = localList.ID;
                Session["count"] = localList.list_Candidates;
                Session["circlename"] = CircleName;
                return RedirectToAction("candidatesAPP");

            }

            return View(localList);




        }

        public ActionResult candidatesAPP()
        {
            var x = Session["count"];
            string listName = Session["list_name"] as string;
            int listID = Convert.ToInt32(Session["list_id"]);

            var list = db.localLists.FirstOrDefault(i => i.list_Name == listName);
            int number = list?.list_Candidates ?? 0;
            ViewBag.number = number;

            return View(new List<LocalCandidate>());
        }

        [HttpPost]
        public ActionResult candidatesAPP(List<LocalCandidate> candidated)
        {
            // Retrieve the session value first
            string listName = Session["list_name"] as string;
            int listID = Convert.ToInt32(Session["list_id"]);

            if (ModelState.IsValid)
            {
                //foreach (var candidate in candidated)
                //{
                //    // Assign the list name and ID to the candidate
                //    candidate.List_Name = listName;
                //    candidate.List_ID = listID;

                //    // Optional: Validate each candidate entity manually
                //    var validationResults = new List<ValidationResult>();
                //    var validationContext = new ValidationContext(candidate, null, null);

                //    if (!Validator.TryValidateObject(candidate, validationContext, validationResults, true))
                //    {
                //        foreach (var error in validationResults)
                //        {
                //            ModelState.AddModelError("", error.ErrorMessage);
                //        }
                //        return View(candidated); // Return the view with the errors
                //    }

                //    // Add the candidate to the database
                //    db.LocalCandidates.Add(candidate);
                //    continue;
                //}
                foreach (var candidate in candidated)
                {

                    var x = candidate;
                    LocalCandidate localCandidate = new LocalCandidate()
                    {
                        CandidateId = x.CandidateId,
                        National_ID = x.National_ID,
                        Candidate_Name = x.Candidate_Name,
                        Type_Of_Chair = x.Type_Of_Chair,
                        List_Name = x.List_Name,
                        Counter = null,
                        Picture = "ssss",
                        List_ID = Convert.ToInt32(Session["list_id"])
                    };
                    db.LocalCandidates.Add(localCandidate);
                }
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            // Log or handle each error
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    return View(candidated); // Return the view with the errors
                }
            }

            Session["x"] = candidated;
            return View(candidated);
        }

        [HttpGet]
        public ActionResult partyApplication()
        {
            return View();
        }
        [HttpPost]
        public ActionResult partyApplication([Bind(Include = "ID,Party_Name,List_Name,Delegate_ID,Delegate_Name,Delegate_Phone,Delegate_Email")] PartyList partyList, HttpPostedFileBase ListLogo)
        {
            if (ModelState.IsValid)
            {



                // UPLOAD A LOGO FOR THE LIST
                var uploadPath = Server.MapPath("~/Uploads");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                if (ListLogo != null && ListLogo.ContentLength > 0)
                {
                    var listLogoFileName = Path.GetFileName(ListLogo.FileName);
                    var listLogoPath = Path.Combine(uploadPath, listLogoFileName);
                    ListLogo.SaveAs(listLogoPath);
                    partyList.List_Logo = listLogoFileName;
                }
                db.PartyLists.Add(partyList);
                db.SaveChanges();

                Session["plist_id"] = partyList.ID;

                return RedirectToAction("PartycandidatesAPP");

            }

            return View();
        }
        public ActionResult PartycandidatesAPP()
        {


            return View();
        }
        [HttpPost]
        public ActionResult PartycandidatesAPP([Bind(Include = "ID,National_ID,Name,Gender,Religion,PartyLIST_ID,picture")] PartyCandidate partyCandidate, HttpPostedFileBase CandidatesExcelFile)
        {
            if (CandidatesExcelFile != null && CandidatesExcelFile.ContentLength > 0)
            {
                // Validate that the file is an Excel file
                var extension = Path.GetExtension(CandidatesExcelFile.FileName).ToLower();
                if (extension == ".xlsx" || extension == ".xls")
                {
                    string excelPath = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(CandidatesExcelFile.FileName));
                    CandidatesExcelFile.SaveAs(excelPath);

                    try
                    {
                        List<PartyCandidate> candidates = new List<PartyCandidate>();
                        using (var stream = System.IO.File.Open(excelPath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                DataTable result = reader.AsDataSet().Tables[0];

                                for (int i = 1; i < result.Rows.Count; i++)
                                {
                                    var row = result.Rows[i];
                                    candidates.Add(new PartyCandidate
                                    {
                                        National_ID = Convert.ToInt32(row[0]), // Correct index to access National_ID
                                        Name = row[1].ToString(),
                                        Gender = row[2].ToString(),
                                        Religion = row[3].ToString(),
                                        PartyLIST_ID = Convert.ToInt32(Session["plist_id"]),
                                        picture = "" // Handle the picture field if needed
                                    });
                                }
                            }
                            db.PartyCandidates.AddRange(candidates);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions, including the HeaderException
                        ModelState.AddModelError("", "An error occurred while processing the file: " + ex.Message);
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid file type. Please upload a valid Excel file.");
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Please upload a file.");
            }

            return View();
        }


    }
}