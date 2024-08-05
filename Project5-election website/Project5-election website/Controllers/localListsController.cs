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
    public class localListsController : Controller
    {
        private ElectionEntities db = new ElectionEntities();

        // GET: localLists
        public ActionResult Index()
        {
            return View(db.localLists.ToList());
        }

        // GET: localLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            localList localList = db.localLists.Find(id);
            if (localList == null)
            {
                return HttpNotFound();
            }
            return View(localList);
        }

        // GET: localLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: localLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Circle_ID,list_Name,list_Candidates,Delegate_ID,Delegate_Name,Delegate_Phone,Delegate_Email,Counter,Status,reason,List_Logo")] localList localList)
        {
            if (ModelState.IsValid)
            {
                db.localLists.Add(localList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(localList);
        }

        // GET: localLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            localList localList = db.localLists.Find(id);
            if (localList == null)
            {
                return HttpNotFound();
            }
            return View(localList);
        }

        // POST: localLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Circle_ID,list_Name,list_Candidates,Delegate_ID,Delegate_Name,Delegate_Phone,Delegate_Email,Counter,Status,reason,List_Logo")] localList localList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(localList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(localList);
        }

        // GET: localLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            localList localList = db.localLists.Find(id);
            if (localList == null)
            {
                return HttpNotFound();
            }
            return View(localList);
        }

        // POST: localLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            localList localList = db.localLists.Find(id);
            db.localLists.Remove(localList);
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
