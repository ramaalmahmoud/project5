using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project5_election_website.Models;

namespace Project5_election_website.Controllers
{
    public class LocalCandidatesController : Controller
    {
        private ElectionEntities db = new ElectionEntities();

        // GET: LocalCandidates
        public ActionResult Index()
        {
            return View(db.LocalCandidates.ToList());
        }

        // GET: LocalCandidates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocalCandidate localCandidate = db.LocalCandidates.Find(id);
            if (localCandidate == null)
            {
                return HttpNotFound();
            }
            return View(localCandidate);
        }

        // GET: LocalCandidates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LocalCandidates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CandidateId,National_ID,Candidate_Name,Type_Of_Chair,List_Name,Counter,Picture,List_ID")] LocalCandidate localCandidate)
        {
            if (ModelState.IsValid)
            {
                db.LocalCandidates.Add(localCandidate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(localCandidate);
        }

        // GET: LocalCandidates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocalCandidate localCandidate = db.LocalCandidates.Find(id);
            if (localCandidate == null)
            {
                return HttpNotFound();
            }
            return View(localCandidate);
        }

        // POST: LocalCandidates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CandidateId,National_ID,Candidate_Name,Type_Of_Chair,List_Name,Counter,Picture,List_ID")] LocalCandidate localCandidate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(localCandidate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(localCandidate);
        }

        // GET: LocalCandidates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LocalCandidate localCandidate = db.LocalCandidates.Find(id);
            if (localCandidate == null)
            {
                return HttpNotFound();
            }
            return View(localCandidate);
        }

        // POST: LocalCandidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LocalCandidate localCandidate = db.LocalCandidates.Find(id);
            db.LocalCandidates.Remove(localCandidate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
