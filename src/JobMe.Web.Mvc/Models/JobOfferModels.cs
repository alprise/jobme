using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace JobMe.Web.Mvc.Models
{
    /*
     * WorkAd/JobOffer
     * I look for 3 fulltime .net developers starting from now in Toronto (Canada)
     * I work as .net developer at hourly/rate basis starting in 3 months from home London(England)
     * Firma X looks for devs in Germany
     * Firma Y looks for work/projects
     */
    public class JobOffer
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public byte DaysAvailable { get; set; }
        public int JobType { get; set; } // Hire / Look for or (work as / need)
        public string EmailToApply { get; set; } // Email to apply
        public virtual ApplicationUser User { get; set; }
    }
}