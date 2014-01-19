namespace JobMe.Web.Mvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewEntities_JobMessageHeader_And_Detail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JobMessageHeaders",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        JobOfferId = c.String(maxLength: 128),
                        From = c.String(),
                        To = c.String(),
                        Subject = c.String(),
                        Sent = c.DateTime(),
                        Body = c.String(),
                        IsHtml = c.Boolean(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobOffers", t => t.JobOfferId)
                .Index(t => t.JobOfferId);
            
            CreateTable(
                "dbo.JobMessageDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FileName = c.String(),
                        Size = c.Int(nullable: false),
                        ContentType = c.String(),
                        JobMessageHeader_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.JobMessageHeaders", t => t.JobMessageHeader_Id)
                .Index(t => t.JobMessageHeader_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JobMessageHeaders", "JobOfferId", "dbo.JobOffers");
            DropForeignKey("dbo.JobMessageDetails", "JobMessageHeader_Id", "dbo.JobMessageHeaders");
            DropIndex("dbo.JobMessageHeaders", new[] { "JobOfferId" });
            DropIndex("dbo.JobMessageDetails", new[] { "JobMessageHeader_Id" });
            DropTable("dbo.JobMessageDetails");
            DropTable("dbo.JobMessageHeaders");
        }
    }
}
