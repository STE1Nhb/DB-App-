namespace DBApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
                .ForeignKey("dbo.purchase_confirmations", t => t.purchase_id, cascadeDelete: true)
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
                .ForeignKey("dbo.subscribers", t => t.subscriber_id, cascadeDelete: true)
                .ForeignKey("dbo.subscription_types", t => t.type_id, cascadeDelete: true)
                .Index(t => t.subscriber_id)
                .Index(t => t.type_id);
            
            CreateTable(
                "dbo.subscribers",
                c => new
                    {
                        subscriber_id = c.Int(nullable: false, identity: true),
                        subscriber_email = c.String(nullable: false, maxLength: 50),
                        subscriber_birth_date = c.DateTime(nullable: false),
                        SubscriberSubscriber_SubscriberId = c.Int(),
                    })
                .PrimaryKey(t => t.subscriber_id)
                .ForeignKey("dbo.subscribers_subscriptions", t => t.SubscriberSubscriber_SubscriberId)
                .Index(t => t.SubscriberSubscriber_SubscriberId);
            
            CreateTable(
                "dbo.subscribers_subscriptions",
                c => new
                    {
                        subscriber_id = c.Int(nullable: false),
                        type_id = c.Int(nullable: false),
                        SubscriptionType_SubscriptionId = c.Int(),
                        Subscriber_SubscriberId = c.Int(),
                    })
                .PrimaryKey(t => t.subscriber_id)
                .ForeignKey("dbo.subscribers", t => t.subscriber_id)
                .ForeignKey("dbo.subscription_types", t => t.SubscriptionType_SubscriptionId)
                .ForeignKey("dbo.subscription_types", t => t.type_id, cascadeDelete: true)
                .ForeignKey("dbo.subscribers", t => t.Subscriber_SubscriberId)
                .Index(t => t.subscriber_id)
                .Index(t => t.type_id)
                .Index(t => t.SubscriptionType_SubscriptionId)
                .Index(t => t.Subscriber_SubscriberId);
            
            CreateTable(
                "dbo.subscription_types",
                c => new
                    {
                        type_id = c.Int(nullable: false, identity: true),
                        subscription_type = c.String(nullable: false, maxLength: 30),
                        SubscriptionPrice_SubscriberId = c.Int(),
                    })
                .PrimaryKey(t => t.type_id)
                .ForeignKey("dbo.subscribers_subscriptions", t => t.SubscriptionPrice_SubscriberId)
                .Index(t => t.SubscriptionPrice_SubscriberId);
            
            CreateTable(
                "dbo.subscription_prices",
                c => new
                    {
                        type_id = c.Int(nullable: false),
                        subscription_price = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.type_id)
                .ForeignKey("dbo.subscription_types", t => t.type_id, cascadeDelete: true)
                .Index(t => t.type_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.subscription_prices", "type_id", "dbo.subscription_types");
            DropForeignKey("dbo.expiry_dates", "purchase_id", "dbo.purchase_confirmations");
            DropForeignKey("dbo.subscribers", "SubscriberSubscriber_SubscriberId", "dbo.subscribers_subscriptions");
            DropForeignKey("dbo.subscribers_subscriptions", "Subscriber_SubscriberId", "dbo.subscribers");
            DropForeignKey("dbo.subscribers_subscriptions", "type_id", "dbo.subscription_types");
            DropForeignKey("dbo.subscription_types", "SubscriptionPrice_SubscriberId", "dbo.subscribers_subscriptions");
            DropForeignKey("dbo.subscribers_subscriptions", "SubscriptionType_SubscriptionId", "dbo.subscription_types");
            DropForeignKey("dbo.purchase_confirmations", "type_id", "dbo.subscription_types");
            DropForeignKey("dbo.subscribers_subscriptions", "subscriber_id", "dbo.subscribers");
            DropForeignKey("dbo.purchase_confirmations", "subscriber_id", "dbo.subscribers");
            DropIndex("dbo.subscription_prices", new[] { "type_id" });
            DropIndex("dbo.subscription_types", new[] { "SubscriptionPrice_SubscriberId" });
            DropIndex("dbo.subscribers_subscriptions", new[] { "Subscriber_SubscriberId" });
            DropIndex("dbo.subscribers_subscriptions", new[] { "SubscriptionType_SubscriptionId" });
            DropIndex("dbo.subscribers_subscriptions", new[] { "type_id" });
            DropIndex("dbo.subscribers_subscriptions", new[] { "subscriber_id" });
            DropIndex("dbo.subscribers", new[] { "SubscriberSubscriber_SubscriberId" });
            DropIndex("dbo.purchase_confirmations", new[] { "type_id" });
            DropIndex("dbo.purchase_confirmations", new[] { "subscriber_id" });
            DropIndex("dbo.expiry_dates", new[] { "purchase_id" });
            DropTable("dbo.subscription_prices");
            DropTable("dbo.subscription_types");
            DropTable("dbo.subscribers_subscriptions");
            DropTable("dbo.subscribers");
            DropTable("dbo.purchase_confirmations");
            DropTable("dbo.expiry_dates");
        }
    }
}
