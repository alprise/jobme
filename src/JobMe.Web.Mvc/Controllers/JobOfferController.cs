using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using JobMe.Web.Mvc.Models;
using Microsoft.AspNet.Identity;
using JobMe.Web.Mvc.Services;
using System.Net.Mail;

namespace JobMe.Web.Mvc.Controllers
{
    [Authorize]
    public class JobOfferController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /JobOffer/
    
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var offers = db.JobOffers.Include(s => s.PublishedByUser).Where(u => u.CreatedByUser.Id == userId).OrderByDescending(o=>o.PublishedOn).ToList();
            var jobOffers = offers.Select(x => new JobOfferIndexViewModel { Id=x.Id, Requester = x.PublishedByUser.UserName, Title = x.Title, PublishedOn = x.PublishedOn });
            return View(jobOffers);
        }

        // GET: /UserTest/Details/5
        public ActionResult Details(string id)
        {
            // todo: we can simplify actions i.e. see Details and Delete for instance
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobOffer offer = db.JobOffers.Include(o => o.PublishedByUser).Where(x => x.Id == id).SingleOrDefault();
            if (offer == null)
            {
                return HttpNotFound();
            }
            JobOfferDetailsViewModel viewModel = new JobOfferDetailsViewModel
            {
                Id = offer.Id,
                Requester = offer.PublishedByUser.UserName,
                Title = offer.Title,
                PublishedOn = offer.PublishedOn
            };
            viewModel.Responses = new ImapService().GetMessagesForEmail(offer.EmailToApply);

            return View(viewModel);
        }

        // GET: /UserTest/MessageDetails/5
        public ActionResult MessageDetails(uint id)
        {
            // todo: we can simplify actions i.e. see Details and Delete for instance
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobOfferMessageDetailViewModel viewModel = new ImapService().GetMessageDetail(id);
            if (viewModel == null)
            {
                return HttpNotFound();
            }
            
            return View(viewModel);
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobOfferCreateEditViewModel jobOffer)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;
                var loggedUser = db.Users.SingleOrDefault(x => x.UserName == userName);
            
                var user = new ApplicationUser { UserName = jobOffer.Requester, Email = jobOffer.EmailToApply };/*todo: email for new user?*/
                var existingUser = db.Users.Where(u => u.UserName == user.UserName).SingleOrDefault();
                if (existingUser != null)
                {
                    user = existingUser;
                }
                db.JobOffers.Add(new JobOffer{
                    Id = Guid.NewGuid().ToString(),
                    Title = jobOffer.Title, 
                    EmailToApply = jobOffer.EmailToApply,
                    Description=jobOffer.Description, 
                    PublishedOn=jobOffer.PublishedOn,
                    PublishedByUser = user,
                    CreatedByUser = loggedUser
                });
                db.SaveChanges();/*todo: catch exceptions (2 same e-mails for 2 users) we should display it*/
                return RedirectToAction("Index");
            }

            return View(jobOffer);
        }

        // GET: /UserTest/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobOffer offer = db.JobOffers.Include(o => o.PublishedByUser).Where(x => x.Id == id).SingleOrDefault();
            if (offer == null)
            {
                return HttpNotFound();
            }
            JobOfferCreateEditViewModel viewModel = new JobOfferCreateEditViewModel
            {
                Id = offer.Id,
                Requester = offer.PublishedByUser.UserName,
                EmailToApply = offer.EmailToApply,
                Description = offer.Description,
                Title = offer.Title,
                PublishedOn = offer.PublishedOn
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobOfferCreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.Users.Where(u => u.UserName == model.Requester).SingleOrDefault();
                existingUser.UserName = model.Requester;
                var existingJobOffer = db.JobOffers.Find(model.Id);
                existingJobOffer.EmailToApply = model.EmailToApply;
                existingJobOffer.Title = model.Title;
                existingJobOffer.Description = model.Description;
                existingJobOffer.PublishedOn = model.PublishedOn;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: /JobOffer/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobOffer offer = db.JobOffers.Include(o=>o.PublishedByUser).Where(x=>x.Id == id).SingleOrDefault();
            
            if (offer == null)
            {
                return HttpNotFound();
            }
            JobOfferDeleteViewModel viewModel = new JobOfferDeleteViewModel
            {
                Id = offer.Id,
                Requester = offer.PublishedByUser.UserName,
                Title = offer.Title,
                PublishedOn = offer.PublishedOn
            };
            return View(viewModel);
        }

        // POST: /UserTest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            JobOffer jobOffer = db.JobOffers.Find(id);
            db.JobOffers.Remove(jobOffer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}

}