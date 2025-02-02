﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace CapStoneCraftxProject
{
    public static class MessagSender

    {
        private static readonly string SmtpHost =
            ConfigurationManager.AppSettings["SmtpHost"];
        private static readonly int SmtpPort=
            int.Parse (ConfigurationManager.AppSettings["SmtpPort"]);
        private static readonly string SmtpUsername =
           ConfigurationManager.AppSettings["SmtpUsername"];
        private static readonly string SmtpPassword =
           ConfigurationManager.AppSettings["SmtpPassword"];
        private static readonly string FromAddress =
             ConfigurationManager.AppSettings["FromAddress"];

        public static void SendEmail(string to, string subject, string body)
        {
            using (var client = new SmtpClient (SmtpHost, SmtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
                client.EnableSsl = true;

                using(var msg = new MailMessage())
                {
                    msg.From = new MailAddress(FromAddress);
                    msg.To.Add(to);
                    msg.Subject = subject;
                    msg.Body = body;
                    msg.IsBodyHtml = true;

                    client.Send(msg);

                }
            }
        }
    }
}