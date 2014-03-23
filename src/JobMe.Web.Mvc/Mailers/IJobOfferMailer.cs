using Mvc.Mailer;

namespace JobMe.Web.Mvc.Mailers
{ 
    public interface IJobOfferMailer
    {
			MvcMailMessage ApplyToJob();
	}
}