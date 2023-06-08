namespace DBApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class InitialDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.expiry_dates",
                c => new
                    {
                        purchase_id = c.Int(nullable: false),
                        expiry_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.purchase_id)
                .ForeignKey("dbo.purchase_confirmations", t => t.purchase_id)
                .Index(t => t.purchase_id);
            
            CreateTable(
                "dbo.purchase_confirmations",
                c => new
                    {
                        purchase_id = c.Int(nullable: false, identity: true),
                        subscriber_id = c.Int(nullable: false),
                        type_id = c.Int(nullable: false),
                        subscription_price = c.Single(nullable: false),
                        purchase_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.purchase_id)
                .ForeignKey("dbo.subscribers_subscriptions", t => new { t.subscriber_id, t.type_id }, cascadeDelete: true)
                .ForeignKey("dbo.subscription_prices", t => t.type_id, cascadeDelete: true)
                .Index(t => new { t.subscriber_id, t.type_id });
            
            CreateTable(
                "dbo.subscribers_subscriptions",
                c => new
                    {
                        subscriber_id = c.Int(nullable: false),
                        type_id = c.Int(nullable: false),
                        Subscriber_SubscriberId = c.Int(),
                        SubscriptionType_SubscriptionId = c.Int(),
                    })
                .PrimaryKey(t => new { t.subscriber_id, t.type_id })
                .ForeignKey("dbo.subscribers", t => t.Subscriber_SubscriberId)
                .ForeignKey("dbo.subscribers", t => t.subscriber_id, cascadeDelete: true)
                .ForeignKey("dbo.subscription_types", t => t.SubscriptionType_SubscriptionId)
                .ForeignKey("dbo.subscription_types", t => t.type_id, cascadeDelete: true)
                .Index(t => t.subscriber_id)
                .Index(t => t.type_id)
                .Index(t => t.Subscriber_SubscriberId)
                .Index(t => t.SubscriptionType_SubscriptionId);
            
            CreateTable(
                "dbo.subscribers",
                c => new
                    {
                        subscriber_id = c.Int(nullable: false, identity: true),
                        subscriber_email = c.String(nullable: false, maxLength: 50),
                        subscriber_birth_date = c.DateTime(nullable: false),
                        SubscriberSubscriber_SubscriberId = c.Int(),
                        SubscriberSubscriber_SubscriptionId = c.Int(),
                    })
                .PrimaryKey(t => t.subscriber_id)
                .ForeignKey("dbo.subscribers_subscriptions", t => new { t.SubscriberSubscriber_SubscriberId, t.SubscriberSubscriber_SubscriptionId })
                .Index(t => new { t.SubscriberSubscriber_SubscriberId, t.SubscriberSubscriber_SubscriptionId });
            
            CreateTable(
                "dbo.subscription_types",
                c => new
                    {
                        type_id = c.Int(nullable: false, identity: true),
                        subscription_type = c.String(nullable: false, maxLength: 30),
                        SubscriptionPrice_SubscriberId = c.Int(),
                        SubscriptionPrice_SubscriptionId = c.Int(),
                    })
                .PrimaryKey(t => t.type_id)
                .ForeignKey("dbo.subscribers_subscriptions", t => new { t.SubscriptionPrice_SubscriberId, t.SubscriptionPrice_SubscriptionId })
                .Index(t => new { t.SubscriptionPrice_SubscriberId, t.SubscriptionPrice_SubscriptionId });
            
            CreateTable(
                "dbo.subscription_prices",
                c => new
                    {
                        type_id = c.Int(nullable: false),
                        subscription_price = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.type_id)
                .ForeignKey("dbo.subscription_types", t => t.type_id)
                .Index(t => t.type_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.expiry_dates", "purchase_id", "dbo.purchase_confirmations");
            DropForeignKey("dbo.subscription_prices", "type_id", "dbo.subscription_types");
            DropForeignKey("dbo.purchase_confirmations", "type_id", "dbo.subscription_prices");
            DropForeignKey("dbo.purchase_confirmations", new[] { "subscriber_id", "type_id" }, "dbo.subscribers_subscriptions");
            DropForeignKey("dbo.subscribers_subscriptions", "type_id", "dbo.subscription_types");
            DropForeignKey("dbo.subscription_types", new[] { "SubscriptionPrice_SubscriberId", "SubscriptionPrice_SubscriptionId" }, "dbo.subscribers_subscriptions");
            DropForeignKey("dbo.subscribers_subscriptions", "SubscriptionType_SubscriptionId", "dbo.subscription_types");
            DropForeignKey("dbo.subscribers_subscriptions", "subscriber_id", "dbo.subscribers");
            DropForeignKey("dbo.subscribers", new[] { "SubscriberSubscriber_SubscriberId", "SubscriberSubscriber_SubscriptionId" }, "dbo.subscribers_subscriptions");
            DropForeignKey("dbo.subscribers_subscriptions", "Subscriber_SubscriberId", "dbo.subscribers");
            DropIndex("dbo.subscription_prices", new[] { "type_id" });
            DropIndex("dbo.subscription_types", new[] { "SubscriptionPrice_SubscriberId", "SubscriptionPrice_SubscriptionId" });
            DropIndex("dbo.subscribers", new[] { "SubscriberSubscriber_SubscriberId", "SubscriberSubscriber_SubscriptionId" });
            DropIndex("dbo.subscribers_subscriptions", new[] { "SubscriptionType_SubscriptionId" });
            DropIndex("dbo.subscribers_subscriptions", new[] { "Subscriber_SubscriberId" });
            DropIndex("dbo.subscribers_subscriptions", new[] { "type_id" });
            DropIndex("dbo.subscribers_subscriptions", new[] { "subscriber_id" });
            DropIndex("dbo.purchase_confirmations", new[] { "subscriber_id", "type_id" });
            DropIndex("dbo.expiry_dates", new[] { "purchase_id" });
            DropTable("dbo.subscription_prices");
            DropTable("dbo.subscription_types");
            DropTable("dbo.subscribers");
            DropTable("dbo.subscribers_subscriptions");
            DropTable("dbo.purchase_confirmations");
            DropTable("dbo.expiry_dates");
        }
    }
}
