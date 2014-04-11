using ActionMailer.Net.Standalone;
using JobMe.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Web;

namespace JobMe.Web.Mvc.Services
{
    public class MailerService : RazorMailerBase
    {
        public override string ViewPath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates"); }
        }

        public RazorEmailResult Verification(JobOfferApplyViewModel model)
        {
            Contract.Requires(!string.IsNullOrEmpty(ImapSettings.Username));

            To.Add(model.EmailToApply);
            From = ImapSettings.Username;
            Subject = "Anzeige";
            
            return Email("Verification", model);
        }
    }
}