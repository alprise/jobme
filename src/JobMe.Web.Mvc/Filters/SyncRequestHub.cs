using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobMe.Web.Mvc.Filters
{
    public class SyncRequestHub:Hub
    {
        private readonly SyncAttribute _redirecter;
        public SyncRequestHub()
            : this(SyncAttribute.Instance)
        {

        }
        public SyncRequestHub(SyncAttribute redirecter)
        {
            _redirecter = redirecter;
        }
    }
}