using System;
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
    public class TradesController : Controller
    {
        private CapStoneProjectEntities5 db = new CapStoneProjectEntities5();

       
        // GET: Trades
        public ActionResult Index(TradeListTypeViewModel model)
          {
            IEnumerable<Trade> trades;
            TradeListType tradelisttype;
            if (model == null)
            {

                tradelisttype = TradeListType.AllOffersRecieved;

            }
            else
            {
                tradelisttype = model.TradeListType;
            }
            var userid = User.Identity.GetUserId();
            var member = db.Members.Find(userid);
            var cellar = member.Cellars.First();
            var senttrades = cellar.Trades1;
            var recievedtrades = cellar.Trades;
            var alltrades = senttrades.Union(recievedtrades);

           
            switch (tradelisttype)
            {
                case TradeListType.AcceptedOffers:
                    trades = alltrades.Where(t => t.IsApproved.HasValue && t.IsApproved.Value);
                    break;
                case TradeListType.AllOffersMade:
                    trades = senttrades;
                    break;
                case TradeListType.AllOffersRecieved:
                    trades = recievedtrades;
                    break;
                case TradeListType.DeniedOffers:
                    trades = alltrades.Where(t => t.IsApproved.HasValue && !t.IsApproved.Value);
                    break;
                case TradeListType.PendingOffersMade:
                    trades = senttrades.Where(t => !t.IsApproved.HasValue);
                    break;
                case TradeListType.PendingOffersRecieved:
                    trades = recievedtrades.Where(t => !t.IsApproved.HasValue);
                    break;
                default:trades = null;
                    break;

            }
            ViewBag.listtype = new TradeListTypeViewModel { TradeListType = tradelisttype };
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
        public ActionResult Create(int id,int beerid)
        {
            var userid = User.Identity.GetUserId();
            var sendingmember = db.Members.Find(userid);
            var sendingcellar = sendingmember.Cellars.First();

            ViewBag.SendingBeerId = new SelectList(sendingcellar.Beers, "Id", "BeerName");
            var trade = new Trade
            {
                SendingMemberId = sendingcellar.Id,
                ReceivingMemberId = id,
                ReceiverBeerId =beerid,
            };
            return View(trade);
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

                var recievingmember = db.Cellars.Find(trade.ReceivingMemberId).Member;
                var bodyTemplate = "<p>Hi {0}</p><p>You have a trade offer made to you,please log into your account to see offer.</p>";
                var body = string.Format(bodyTemplate,recievingmember.FirstName);
                MessagSender.SendEmail(recievingmember.Email, "Pending Trade Offer", body);

                var tradelisttypeviewmodel = new TradeListTypeViewModel { TradeListType = TradeListType.PendingOffersMade };
                return RedirectToAction("Index",new {model = tradelisttypeviewmodel });
            }

            var userid = User.Identity.GetUserId();
            var sendingmember = db.Members.Find(userid);
            var sendingcellar = sendingmember.Cellars.First();

            ViewBag.SendingBeerId = new SelectList(sendingcellar.Beers, "Id", "BeerName");
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

            var bools = new[]
            {
                new{val =true,name="Accept"},
                new{val= false,name="Denied"}
            };
            ViewBag.ReceiverBeerId = new SelectList(trade.Cellar.Beers, "Id", "BeerName", trade.ReceiverBeerId);
            ViewBag.SendingBeerId = new SelectList(trade.Cellar1.Beers, "Id", "BeerName", trade.SendingBeerId);
            ViewBag.BoolValues = new SelectList(bools, "val", "name", trade.IsApproved);
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
            {//var trade = new trade
                db.Entry(trade).State = EntityState.Modified;
                 if(trade.IsApproved.HasValue && trade.IsApproved.Value )
                 {

                    Cellar offertradecellar = db.Cellars.Find(trade.SendingMemberId);
                    Cellar recievingtradecellar = db.Cellars.Find(trade.ReceivingMemberId);
                    Beer offeretradebeer = db.Beers.Find(trade.SendingBeerId);
                    Beer recievingtradebeer = db.Beers.Find(trade.ReceiverBeerId);

                    offertradecellar.Beers.Remove(offeretradebeer);
                    offertradecellar.Beers.Add(recievingtradebeer);
                    recievingtradecellar.Beers.Add(offeretradebeer);
                    recievingtradecellar.Beers.Remove(recievingtradebeer);

                    var recievingmember = recievingtradecellar.Member;
                    var sendingMember = offertradecellar.Member;
                    var bodyTemplate = "<p>Hi {0}</p><p>Your trade has been accepted,Enjoy!</p>";
                    var recievingbody = string.Format(bodyTemplate, recievingmember.FirstName);
                    var sendingbody = string.Format(bodyTemplate, sendingMember.FirstName);
                    MessagSender.SendEmail(recievingmember.Email, "Acepted Trade Offer",recievingbody);
                    MessagSender.SendEmail(sendingMember.Email, "Acepted Trade Offer",sendingbody);
                }


                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                var error = string.Join("\n", allErrors.Select(e => e.ErrorMessage));
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
