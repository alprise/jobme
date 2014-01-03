using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace JobMe.Web.Mvc.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        /* added new email property not in standard mvc project*/
        public string Email { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
        
        /* new app specific dbsets */
        public IDbSet<JobOffer> JobOffers { get; set; }
    }
}