namespace JobMe.Web.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProperty_ApplyedOn_InJobOffer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobOffers", "ApplyedOn", c => c.DateTime(nullable: false));
            Sql("UPDATE dbo.JobOffers SET ApplyedOn = PublishedOn"); 
            DropColumn("dbo.JobOffers", "TestProperty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JobOffers", "TestProperty", c => c.String());
            DropColumn("dbo.JobOffers", "ApplyedOn");
        }
    }
}
