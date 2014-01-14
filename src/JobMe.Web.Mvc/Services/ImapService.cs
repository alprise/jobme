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
                Client.DefaultMailbox = "[Gmail]/Toate mesajele"; 
                
                uint[] uids = Client.Search(SearchCondition.From(domain).Or(SearchCondition.To(domain)));
                List<JobOfferResponseViewModel> allMessages = new List<JobOfferResponseViewModel>();
                uids.ToList().ForEach(id =>
                {
                    var message = Client.GetMessage(id);
                    allMessages.Add(new JobOfferResponseViewModel
                {
                    Uid = id,
                    MessageId = message.Headers["Message-ID"],
                    From = message.From.Address,
                    To = string.Join(",", message.To.Select(t => t.Address).ToArray()),
                    Subject = message.Subject,
                    Date = message.Headers["Date"]
                });
                });
                //TODO: measure performance of loading one message by uid or all by uids
                //


                //MailMessage[] messagesFrom = Client.GetMessages(uids);
                //var messages = messagesFrom.Select(x => new JobOfferResponseViewModel
                //{
                //    MessageId = x.Headers["Message-ID"],
                //    From = x.From.Address,
                //    To = string.Join(",", x.To.Select(t => t.Address).ToArray()),
                //    Subject = x.Subject,
                //    Date = x.Headers["Date"]
                //}).ToList();

                

                return allMessages;
            }
        }

        public JobOfferMessageDetailViewModel GetMessageDetail(uint id)
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
                Client.DefaultMailbox = "[Gmail]/Toate mesajele";
                MailMessage messagesFrom = Client.GetMessage(id);
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