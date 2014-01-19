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
        public string PublishedByUserId { get; set; }
        public string CreatedByUserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public byte DaysAvailable { get; set; }
        public int JobType { get; set; } // Hire / Look for or (work as / need)
        public string EmailToApply { get; set; } // Email to apply
        public virtual ApplicationUser PublishedByUser { get; set; }
        public virtual ApplicationUser CreatedByUser { get; set; }
        public virtual ICollection<JobMessageHeader> JobMessageHeaders { get; set; }
        public string TestProperty { get; set; }
    }
    public class JobMessageHeader
    {
        public string Id { get; set; }
        public string JobOfferId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public DateTime? Sent { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public bool IsRead { get; set; }
        public virtual ICollection<JobMessageDetail> Details { get; set; }
        public virtual JobOffer JobOffer { get; set; }
        
    }
    public class JobMessageDetail
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public int Size { get; set; }
        public string ContentType { get; set; }
    }
}