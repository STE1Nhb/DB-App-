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
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        //[Column(Order = 2)]
        public int SubscriptionId { get; set; }
        [Required]
        public string Type { get; set; }
        public virtual ICollection<SubscriberSubscription> SubscribersSubscriptions { get; set; }

        public virtual SubscriberSubscription SubscriptionPrice { get; set; }
        //public virtual ICollection<SubscriptionPrice> SubscriptionPrices { get; set; }

        public SubscriptionType()
        {
            this.SubscribersSubscriptions = new HashSet<SubscriberSubscription>();
            //this.SubscriptionPrices = new HashSet<SubscriptionPrice>();
        }
    }
}
