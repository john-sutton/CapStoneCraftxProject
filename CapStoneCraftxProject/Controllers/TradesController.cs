using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CapStoneCraftxProject.Models;

namespace CapStoneCraftxProject.Controllers
{
    public class TradesController : Controller
    {
        private CapStoneProjectEntities5 db = new CapStoneProjectEntities5();

        // GET: Trades
        public ActionResult Index()
        {
            var trades = db.Trades.Include(t => t.Beer).Include(t => t.Beer1).Include(t => t.Cellar).Include(t => t.Cellar1);
            return View(trades.ToList());
        }

        // GET: Trades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            return View(trade);
        }

        // GET: Trades/Create
        public ActionResult Create()
        {
            ViewBag.ReceiverBeerId = new SelectList(db.Beers, "Id", "Style");
            ViewBag.SendingBeerId = new SelectList(db.Beers, "Id", "Style");
            ViewBag.ReceivingMemberId = new SelectList(db.Cellars, "Id", "MemberId");
            ViewBag.SendingMemberId = new SelectList(db.Cellars, "Id", "MemberId");
            return View();
        }

        // POST: Trades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SendingMemberId,ReceivingMemberId,SendingBeerId,ReceiverBeerId,IsApproved,SendingComments,ReceivingComments")] Trade trade)
        {
            if (ModelState.IsValid)
            {
                db.Trades.Add(trade);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReceiverBeerId = new SelectList(db.Beers, "Id", "Style", trade.ReceiverBeerId);
            ViewBag.SendingBeerId = new SelectList(db.Beers, "Id", "Style", trade.SendingBeerId);
            ViewBag.ReceivingMemberId = new SelectList(db.Cellars, "Id", "MemberId", trade.ReceivingMemberId);
            ViewBag.SendingMemberId = new SelectList(db.Cellars, "Id", "MemberId", trade.SendingMemberId);
            return View(trade);
        }

        // GET: Trades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReceiverBeerId = new SelectList(db.Beers, "Id", "Style", trade.ReceiverBeerId);
            ViewBag.SendingBeerId = new SelectList(db.Beers, "Id", "Style", trade.SendingBeerId);
            ViewBag.ReceivingMemberId = new SelectList(db.Cellars, "Id", "MemberId", trade.ReceivingMemberId);
            ViewBag.SendingMemberId = new SelectList(db.Cellars, "Id", "MemberId", trade.SendingMemberId);
            return View(trade);
        }

        // POST: Trades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SendingMemberId,ReceivingMemberId,SendingBeerId,ReceiverBeerId,IsApproved,SendingComments,ReceivingComments")] Trade trade)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReceiverBeerId = new SelectList(db.Beers, "Id", "Style", trade.ReceiverBeerId);
            ViewBag.SendingBeerId = new SelectList(db.Beers, "Id", "Style", trade.SendingBeerId);
            ViewBag.ReceivingMemberId = new SelectList(db.Cellars, "Id", "MemberId", trade.ReceivingMemberId);
            ViewBag.SendingMemberId = new SelectList(db.Cellars, "Id", "MemberId", trade.SendingMemberId);
            return View(trade);
        }

        // GET: Trades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Trade trade = db.Trades.Find(id);
            if (trade == null)
            {
                return HttpNotFound();
            }
            return View(trade);
        }

        // POST: Trades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Trade trade = db.Trades.Find(id);
            db.Trades.Remove(trade);
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
