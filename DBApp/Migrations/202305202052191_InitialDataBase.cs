namespace DBApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDataBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExpirationDates",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseId)
                .ForeignKey("dbo.PurchaseConfirmations", t => t.PurchaseId)
                .Index(t => t.PurchaseId);
            
            CreateTable(
                "dbo.PurchaseConfirmations",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false, identity: true),
                        SubscriberId = c.Int(nullable: false),
                        SubscriptionId = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseId)
                .ForeignKey("dbo.SubscriberSubscriptions", t => new { t.SubscriberId, t.SubscriptionId }, cascadeDelete: true)
                .ForeignKey("dbo.SubscriptionPrices", t => t.Price, cascadeDelete: true)
                .Index(t => new { t.SubscriberId, t.SubscriptionId })
                .Index(t => t.Price);
            
            CreateTable(
                "dbo.SubscriberSubscriptions",
                c => new
                    {
                        SubscriberId = c.Int(nullable: false),
                        SubscriptionId = c.Int(nullable: false),
                        Subscriber_SubscriberId = c.Int(),
                        SubscriptionType_SubscriptionId = c.Int(),
                    })
                .PrimaryKey(t => new { t.SubscriberId, t.SubscriptionId })
                .ForeignKey("dbo.Subscribers", t => t.Subscriber_SubscriberId)
                .ForeignKey("dbo.Subscribers", t => t.SubscriberId, cascadeDelete: true)
                .ForeignKey("dbo.SubscriptionTypes", t => t.SubscriptionType_SubscriptionId)
                .ForeignKey("dbo.SubscriptionTypes", t => t.SubscriptionId, cascadeDelete: true)
                .Index(t => t.SubscriberId)
                .Index(t => t.SubscriptionId)
                .Index(t => t.Subscriber_SubscriberId)
                .Index(t => t.SubscriptionType_SubscriptionId);
            
            CreateTable(
                "dbo.Subscribers",
                c => new
                    {
                        SubscriberId = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        BirthDate = c.DateTime(nullable: false),
                        SubscriberSubscriber_SubscriberId = c.Int(),
                        SubscriberSubscriber_SubscriptionId = c.Int(),
                    })
                .PrimaryKey(t => t.SubscriberId)
                .ForeignKey("dbo.SubscriberSubscriptions", t => new { t.SubscriberSubscriber_SubscriberId, t.SubscriberSubscriber_SubscriptionId })
                .Index(t => new { t.SubscriberSubscriber_SubscriberId, t.SubscriberSubscriber_SubscriptionId });
            
            CreateTable(
                "dbo.SubscriptionTypes",
                c => new
                    {
                        SubscriptionId = c.Int(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        SubscriptionPrice_SubscriberId = c.Int(),
                        SubscriptionPrice_SubscriptionId = c.Int(),
                    })
                .PrimaryKey(t => t.SubscriptionId)
                .ForeignKey("dbo.SubscriberSubscriptions", t => new { t.SubscriptionPrice_SubscriberId, t.SubscriptionPrice_SubscriptionId })
                .Index(t => new { t.SubscriptionPrice_SubscriberId, t.SubscriptionPrice_SubscriptionId });
            
            CreateTable(
                "dbo.SubscriptionPrices",
                c => new
                    {
                        Price = c.Int(nullable: false, identity: true),
                        SubscriptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Price)
                .ForeignKey("dbo.SubscriptionTypes", t => t.SubscriptionId, cascadeDelete: false)
                .Index(t => t.SubscriptionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExpirationDates", "PurchaseId", "dbo.PurchaseConfirmations");
            DropForeignKey("dbo.PurchaseConfirmations", "Price", "dbo.SubscriptionPrices");
            DropForeignKey("dbo.SubscriptionPrices", "SubscriptionId", "dbo.SubscriptionTypes");
            DropForeignKey("dbo.PurchaseConfirmations", new[] { "SubscriberId", "SubscriptionId" }, "dbo.SubscriberSubscriptions");
            DropForeignKey("dbo.SubscriberSubscriptions", "SubscriptionId", "dbo.SubscriptionTypes");
            DropForeignKey("dbo.SubscriptionTypes", new[] { "SubscriptionPrice_SubscriberId", "SubscriptionPrice_SubscriptionId" }, "dbo.SubscriberSubscriptions");
            DropForeignKey("dbo.SubscriberSubscriptions", "SubscriptionType_SubscriptionId", "dbo.SubscriptionTypes");
            DropForeignKey("dbo.SubscriberSubscriptions", "SubscriberId", "dbo.Subscribers");
            DropForeignKey("dbo.Subscribers", new[] { "SubscriberSubscriber_SubscriberId", "SubscriberSubscriber_SubscriptionId" }, "dbo.SubscriberSubscriptions");
            DropForeignKey("dbo.SubscriberSubscriptions", "Subscriber_SubscriberId", "dbo.Subscribers");
            DropIndex("dbo.SubscriptionPrices", new[] { "SubscriptionId" });
            DropIndex("dbo.SubscriptionTypes", new[] { "SubscriptionPrice_SubscriberId", "SubscriptionPrice_SubscriptionId" });
            DropIndex("dbo.Subscribers", new[] { "SubscriberSubscriber_SubscriberId", "SubscriberSubscriber_SubscriptionId" });
            DropIndex("dbo.SubscriberSubscriptions", new[] { "SubscriptionType_SubscriptionId" });
            DropIndex("dbo.SubscriberSubscriptions", new[] { "Subscriber_SubscriberId" });
            DropIndex("dbo.SubscriberSubscriptions", new[] { "SubscriptionId" });
            DropIndex("dbo.SubscriberSubscriptions", new[] { "SubscriberId" });
            DropIndex("dbo.PurchaseConfirmations", new[] { "Price" });
            DropIndex("dbo.PurchaseConfirmations", new[] { "SubscriberId", "SubscriptionId" });
            DropIndex("dbo.ExpirationDates", new[] { "PurchaseId" });
            DropTable("dbo.SubscriptionPrices");
            DropTable("dbo.SubscriptionTypes");
            DropTable("dbo.Subscribers");
            DropTable("dbo.SubscriberSubscriptions");
            DropTable("dbo.PurchaseConfirmations");
            DropTable("dbo.ExpirationDates");
        }
    }
}
