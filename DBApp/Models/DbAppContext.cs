using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DBApp
{
    public class DbAppContext : DbContext
    {
        public DbAppContext() : base("DbConnection") 
        { }

        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public DbSet<SubscriberSubscription> SubscribersSubscriptions { get; set; }
        public DbSet<SubscriptionPrice> SubscriptionPrices { get; set; }
        public DbSet<PurchaseConfirmation> PurchaseConfirmations { get; set; }
        public DbSet<ExpirationDate> ExpirationDates { get; set; }
    }
}
