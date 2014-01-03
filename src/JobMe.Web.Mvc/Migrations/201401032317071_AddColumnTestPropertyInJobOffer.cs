namespace JobMe.Web.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnTestPropertyInJobOffer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobOffers", "TestProperty", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobOffers", "TestProperty");
        }
    }
}
