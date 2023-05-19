using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBApp
{
    public class SubscriptionType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }
        public string Type { get; set; }
        public virtual ICollection<SubscriberSubscription> SubscribersSubscriptions { get; set; }
        public virtual ICollection<SubscriptionPrice> SubscriptionsPrices { get; set; }

        public SubscriptionType()
        {
            this.SubscribersSubscriptions = new HashSet<SubscriberSubscription>();
            this.SubscriptionsPrices = new HashSet<SubscriptionPrice>();
        }
    }
}
