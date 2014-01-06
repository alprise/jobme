using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace JobMe.Web.Mvc.Hubs
{
    public class ChatHub : Hub
    {
        
        public void Send(string name, string message)
        {
            var datetime = DateTime.UtcNow;
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message, datetime);
        }
    }
}