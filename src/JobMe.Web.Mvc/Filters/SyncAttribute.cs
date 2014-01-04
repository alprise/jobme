using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobMe.Web.Mvc.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class SyncAttribute : ActionFilterAttribute
    {
        private SyncAttribute(IHubConnectionContext clients)
        {
            Clients = clients;
        }
        // Singleton instance
        private readonly static Lazy<SyncAttribute> _instance = new Lazy<SyncAttribute>(() => new SyncAttribute(GlobalHost.ConnectionManager.GetHubContext<SyncRequestHub>().Clients));

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var url = filterContext.HttpContext.Request.Url;
            var httpMethod = filterContext.RequestContext.HttpContext.Request.HttpMethod;
            if (httpMethod == "GET")
            {
                Clients.All.redirect(url);
            }
        }

        public static SyncAttribute Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext Clients
        {
            get;
            set;
        }
    }
}