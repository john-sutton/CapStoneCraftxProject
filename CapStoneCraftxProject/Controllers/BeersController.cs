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
    public class BeersController : Controller
    {
        private CapStoneProjectEntities5 db = new CapStoneProjectEntities5();

        // GET: Beers
        public ActionResult Index()
        {
            var beers = db.Beers.OrderBy(b => b.Style).ThenBy(br => br.Brewer).ThenBy(b => b.BeerName);
            return View(beers.ToList());

        }
        public ActionResult Forcellar(int id)
        {
            ViewBag.Cellarid = id;
            var Cellar = db.Cellars.Find(id);
            var beers = Cellar.Beers;
            if (Cellar.MemberId == User.Identity.GetUserId())
            {
                ViewBag.IsCurrentUser = true;
            }
            else
            {
                ViewBag.IsCurrentUser = false;
            }
            
            return PartialView(beers.ToList());



        }

        // GET: Beers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // GET: Beers/Create
        public ActionResult Create(int id)
        {
            ViewBag.BrewerList = new SelectList(GetBrewers());
            ViewBag.StyleList = new SelectList(GetStyle());
            ViewBag.Cellarid = id;
            return View();
        }

        // POST: Beers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Beer beer)
        {
            if (ModelState.IsValid)
            {
                var existingbeer = db.Beers.Where(b => b.BeerName == beer.BeerName).FirstOrDefault();
                if (existingbeer == null)
                {
                    beer = db.Beers.Add(beer);
                }
                else
                {
                    beer = existingbeer;
                }
                var cellar = db.Cellars.Find(id);
                cellar.Beers.Add(beer);
                db.SaveChanges();
                return RedirectToAction("Details", "Cellars", new { id = id });
            }

            return View(beer);
        }

        // GET: Beers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // POST: Beers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Style,Brewer,BeerName,Abv,Description")] Beer beer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(beer);
        }

        // GET: Beers/Delete/5
        public ActionResult Delete(int? id, int Cellarid)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            var cellar = beer.Cellars.FirstOrDefault(c => c.Id == Cellarid);
            if (cellar == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cellarid = Cellarid;
            return View(beer);
        }

        // POST: Beers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int Cellarid)
        {
            Beer beer = db.Beers.Find(id);
            var cellar = db.Cellars.Find(Cellarid);
            cellar.Beers.Remove(beer);
            db.SaveChanges();
            return RedirectToAction("Details", "Cellars", new { id = Cellarid });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private IEnumerable<string> GetBrewers()
        {
            var Breweries = new List<string> {
                "Goose Island",
                "Third Wheel",
                "Avery",
                "Three Floyds",
                "Four Hands",
                "New Glarus",
                "Founders",
                "Perennial",
                "Half Acre",
                "Firestone Walker",
                "Civil Life",
                "Earth Bound "

            };
            Breweries.Sort();
            return Breweries;
        }
        private IEnumerable<string> GetStyle()
        {
            var BeerStyles = new List<string> { 
                "Stout",
                "American Brown",
                "English Brown",
                "Porter",
                "American Pale Ale",
                "India Pale Ale(IPA)",
                "Wheat Beer",
                "Lager",
                "Pilsner",
                "Amber Ale",
                "English Pale Ale",
                "Hefewizen",
                "American Wheat",
                "Sour",
                "Barleywine",        
                        
            };
            BeerStyles.Sort();
            return BeerStyles;

        }

    }

}
