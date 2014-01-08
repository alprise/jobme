using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;

namespace JobMe.Web.Mvc.Hubs
{
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> allConnectedUsers = new ConcurrentDictionary<string, string>();

        public override System.Threading.Tasks.Task OnConnected()
        {
            var currentConnectionId = this.Context.ConnectionId;
            var datetime = DateTime.UtcNow;
            if (this.Context.User != null)
            {
                var name = this.Context.User.Identity.Name;
                allConnectedUsers.TryAdd(currentConnectionId,name);
                var availableUsers = string.Join(",", allConnectedUsers.Where(x => x.Key != currentConnectionId).Select(y => y.Value).ToArray());

                if (!string.IsNullOrEmpty(availableUsers))
                {
                    Clients.Caller.showAvailableUsers(availableUsers, datetime);
                }                

                Clients.AllExcept(currentConnectionId).showUserConnected(name, datetime);
            }
            
            return base.OnConnected();
        }
        public override System.Threading.Tasks.Task OnDisconnected()
        {
            var currentConnectionId = this.Context.ConnectionId;
            var datetime = DateTime.UtcNow;
            if (this.Context.User != null)
            {
                var name = this.Context.User.Identity.Name;
                string value;
                allConnectedUsers.TryRemove(currentConnectionId, out value);
                Clients.AllExcept(currentConnectionId).showUserDisconnected(name, datetime);
            }
            
            return base.OnDisconnected();
        }
        public void Send(string name, string message)
        {
            var datetime = DateTime.UtcNow;
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message, datetime);
        }
    }
}