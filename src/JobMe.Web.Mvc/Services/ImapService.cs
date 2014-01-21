using JobMe.Web.Mvc.Models;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Data.Entity;
using System.Diagnostics.Contracts;

namespace JobMe.Web.Mvc.Services
{
    public class ImapService
    {

        internal int SaveUnseenMessages(string userId)
        {
            Contract.Requires(!string.IsNullOrEmpty(ImapSettings.Hostname));

            using (var db = new ApplicationDbContext())
            {
                var jobheadersQuery = db.JobOffers
                    .Include(s => s.PublishedByUser)
                    .Include(m => m.JobMessageHeaders)
                    .Where(u => u.CreatedByUser.Id == userId);

                var jobheaders = jobheadersQuery
                    .SelectMany(j => j.JobMessageHeaders).ToList();
                var allOffers = jobheadersQuery.ToList();

                uint maxUid = 0;
                foreach (var item in jobheaders)
                {
                    try
                    {
                        var messageId = item.Id;
                        uint uid = Convert.ToUInt32(messageId);
                        if (uid > maxUid) maxUid = uid;
                    }
                    catch {}
                }
                //var maxUid = 0; jobheaders.Count > 0 ? jobheaders.Select(x => x.Id).Cast<uint>().Max() : 0;

                using (ImapClient Client = new ImapClient(ImapSettings.Hostname, ImapSettings.Port,
             ImapSettings.Username, ImapSettings.Password, AuthMethod.Login, ImapSettings.UseSsl))
                {
                    Client.DefaultMailbox = "[Gmail]/Toate mesajele";
                    uint[] uids = Client.Search(SearchCondition.GreaterThan(maxUid));
                    uids.ToList().ForEach(id =>
                    {
                        try
                        {
                            var message = Client.GetMessage(id);
                            DateTime sent;
                            DateTime? parsedSent = null;
                            bool isConverted = DateTime.TryParse(message.Headers["Date"], out sent);
                            if (isConverted) parsedSent = sent;
                            var jobmessage = new JobMessageHeader
                            {
                                Id=id.ToString(),
                                From = message.From.Address,
                                To = string.Join(",", message.To.Select(t => t.Address).ToArray()),
                                Subject = message.Subject,
                                Sent = parsedSent,
                                Body = message.Body
                            };
                            FindJobOfferForMessage(userId, db, jobmessage, allOffers);
                        
                        }
                        catch {                         
                        }
                        

                    });
                }
                return db.SaveChanges();
            }
        }

        private static void FindJobOfferForMessage(string userId, ApplicationDbContext db, JobMessageHeader jobmessage, List<JobOffer> allOffers)
        {
            // find a coresponding job offer for this message and save it in db if found
            // loop through all job offers and take the domain of the email to apply and 
            //see if it is contained in from or to fields ( todo optimeze it with joins, dictionaries)
            
            foreach (var offer in allOffers)
            {
                System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(offer.EmailToApply);
                string domain = addr.Host;
                if (jobmessage.From.Contains(domain) || jobmessage.To.Contains(domain))
                {
                    jobmessage.JobOffer = offer;
                    db.MessageHeaders.Add(jobmessage);
                }
            }
        }
    }
    public static class ImapSettings
    {
        public static string Hostname { get { return ConfigurationManager.AppSettings["hostname"]; } }
        public static int Port { get { return int.Parse(ConfigurationManager.AppSettings["port"]); ; } }
        public static bool UseSsl { get { return bool.Parse(ConfigurationManager.AppSettings["useSsl"]); } }
        public static string Username { get { return ConfigurationManager.AppSettings["username"]; } }
        public static string Password { get { return ConfigurationManager.AppSettings["password"]; } }

    }
}