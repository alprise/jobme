using JobMe.Web.Mvc.Models;
using S22.Imap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace JobMe.Web.Mvc.Services
{
    public class ImapService
    {
        public IEnumerable<JobOfferResponseViewModel> GetMessagesForEmail(string email)
        {
            var hostname = ConfigurationManager.AppSettings["hostname"];
            var port = int.Parse(ConfigurationManager.AppSettings["port"]);
            var useSsl = bool.Parse(ConfigurationManager.AppSettings["useSsl"]);
            var username = ConfigurationManager.AppSettings["username"];
            var password = ConfigurationManager.AppSettings["password"];

            System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(email);
            string domain = addr.Host;

            using (ImapClient Client = new ImapClient(hostname, port,
             username, password, AuthMethod.Login, useSsl))
            {
                // TODO: get date as datetime
                // 
                
                uint[] uids = Client.Search(SearchCondition.From(domain));
                MailMessage[] messagesFrom = Client.GetMessages(uids);
                var messages = messagesFrom.Select(x => new JobOfferResponseViewModel
                {
                    MessageId = x.Headers["Message-ID"],
                    From = x.From.Address,
                    To = string.Join(",", x.To.Select(t => t.Address).ToArray()),
                    Subject = x.Subject,
                    Date = x.Headers["Date"]
                }).ToList();

                

                return messages;
            }
        }

        public JobOfferMessageDetailViewModel GetMessageDetail(string id)
        {
            JobOfferMessageDetailViewModel message = null;
            var hostname = ConfigurationManager.AppSettings["hostname"];
            var port = int.Parse(ConfigurationManager.AppSettings["port"]);
            var useSsl = bool.Parse(ConfigurationManager.AppSettings["useSsl"]);
            var username = ConfigurationManager.AppSettings["username"];
            var password = ConfigurationManager.AppSettings["password"];

            using (ImapClient Client = new ImapClient(hostname, port,
             username, password, AuthMethod.Login, useSsl))
            {
                MailMessage messagesFrom = Client.GetMessage(uint.Parse(id));
                message = new JobOfferMessageDetailViewModel
                {
                    MessageId = messagesFrom.Headers["Message-ID"],
                    From = messagesFrom.From.Address,
                    To = string.Join(",", messagesFrom.To.Select(t => t.Address).ToArray()),
                    Subject = messagesFrom.Subject,
                    Date = messagesFrom.Body,
                    Body = messagesFrom.Headers["Body"]
                };
                return message;
            }
        }
    }
}