using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapStoneCraftxProject.Models;
using Microsoft.AspNet.Identity;

namespace CapStoneCraftxProject.Controllers
    
{
    [Authorize]
    public class CellarsController : Controller
    {
        private CapStoneProjectEntities5 db = new CapStoneProjectEntities5();

        // GET: Cellars
        public ActionResult Index(int? beerId)
        {
            var cellars = db.Cellars.Include(c => c.Member);
            if (beerId.HasValue)
            {
                cellars = cellars.Where(c => c.Beers.Any(b => b.Id == beerId));
            }
            
            return View(cellars.ToList());
           
        }
        // DropDown
        public class Breweries
        {
            public List<SelectListItem> ListItems { get; set; }
                public int SelectItem { get; set; }
        }
        
        //https://localhost:44308/Cellars?beerId=2

        // GET: Cellars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cellar cellar = db.Cellars.Find(id);
            if (cellar == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsCurrentUser = cellar.MemberId == User.Identity.GetUserId();
            return View(cellar);
        }
        // My Cellar
        public ActionResult MyCellar()
        {
            var userId = User.Identity.GetUserId();
            var cellar = db.Members.Find(userId).Cellars.First();
            return View("Details", cellar);
        }
       

        // GET: Cellars/Create
        public ActionResult Create()
        {
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Email");
            return View();
        }

        // POST: Cellars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MemberId,CellarName")] Cellar cellar)
        {
            if (ModelState.IsValid)
            {
                db.Cellars.Add(cellar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MemberId = new SelectList(db.Members, "Id", "Email", cellar.MemberId);
            return View(cellar);

          
        }

        // GET: Cellars/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cellar cellar = db.Cellars.Find(id);
            if (cellar == null)
            {
                return HttpNotFound();
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Email", cellar.MemberId);
            return View(cellar);
        }

        // POST: Cellars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MemberId,CellarName")] Cellar cellar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cellar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MemberId = new SelectList(db.Members, "Id", "Email", cellar.MemberId);
            return View(cellar);
        }

        // GET: Cellars/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cellar cellar = db.Cellars.Find(id);
            if (cellar == null)
            {
                return HttpNotFound();
            }
            return View(cellar);
        }

        // POST: Cellars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cellar cellar = db.Cellars.Find(id);
            db.Cellars.Remove(cellar);
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
