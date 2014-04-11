using Mvc.Mailer;

namespace JobMe.Web.Mvc.Mailers
{ 
    public class JobOfferMailer : MailerBase, IJobOfferMailer 	
	{
		public JobOfferMailer()
		{
			MasterName="_Layout";
		}
		
		public virtual MvcMailMessage ApplyToJob()
		{
            var model = this.GetModel();
            ViewData.Model = model;
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "ApplyToJob";
				x.ViewName = "ApplyToJob";
				x.To.Add("some-email@example.com");
                
			});
		}

        private object GetModel()
        {
            throw new System.NotImplementedException();
        }
 	}
}