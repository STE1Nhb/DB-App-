using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DBApp
{
    [Table("subscribers_subscriptions")]
    public class SubscriberSubscription
    {
        [Required]
        [Key]
        [Column("subscriber_id",Order = 1)]
        [ForeignKey("Subscriber")]
        public int SubscriberId { get; set; }

        [Required]
        //[Key]
        [Column("type_id",Order = 2)]
        [ForeignKey("SubscriptionType")]
        public int SubscriptionId { get; set; }

        public SubscriberSubscription()
        {
            //this.PurchaseConfirmations = new HashSet<PurchaseConfirmation>();
        }
        public virtual SubscriptionType SubscriptionType { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        //public virtual ICollection<PurchaseConfirmation> PurchaseConfirmations { get; set; }

    }
}
