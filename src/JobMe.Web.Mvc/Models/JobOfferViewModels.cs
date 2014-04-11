using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobMe.Web.Mvc.Models
{
    public class JobOfferIndexViewModel 
    {
        public string Id { get; set; }
        public string Requester { get; set; }        
        public string Title { get; set; }
        public int Total { get; set; }
        public int TotalRead { get; set; }
        public DateTime PublishedOn { get; set; }
        public DateTime ApplyedOn { get; set; }
    }
    public class JobOfferDeleteViewModel
    {
        public string Id { get; set; }
        public string Requester { get; set; }
        public string Title { get; set; }
        public DateTime PublishedOn { get; set; }
    }

    public class JobOfferApplyViewModel
    {
        public string Id { get; set; }
        public string EmailToApply { get; set; }
        public string Requester { get; set; }
        public string Title { get; set; }
        public DateTime PublishedOn { get; set; }
    }
    public class JobOfferCreateEditViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Requester { get; set; }
        [Required]
        public string EmailToApply { get; set; }
        [Required]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }
        
    }

    public class JobOfferDetailsViewModel
    {
        public string Id { get; set; }
        public string Requester { get; set; }
        public string Title { get; set; }
        public DateTime PublishedOn { get; set; }
        public IEnumerable<JobOfferResponseViewModel> Responses { get; set; }

    }
    public class JobOfferResponseViewModel
    {
        public string Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public DateTime? DateAndTime { get; set; }
    }
    public class JobOfferMessageDetailViewModel
    {
        public string Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime? DateAndTime { get; set; }
    }

}