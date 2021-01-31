using CapStoneCraftxProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CapStoneCraftxProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var user = User.Identity;
            var Id = user.GetUserId();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(EmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bodyTemplate = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var body = string.Format(bodyTemplate, model.FromName, model.FromEmail, model.Message);
                MessagSender.SendEmail("jsutton5844@gmail.com", "A message from the Contact Form", body);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

    }
}