using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBApp
{
    [Table("subscription_types")]
    public class SubscriptionType
    {
        [Column("type_id")]
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //[Column(Order = 2)]
        public int SubscriptionId { get; set; }

        [Column("subscription_type")]
        [MaxLength(30)]
        [Required]
        public string Type { get; set; }
        public virtual ICollection<SubscriberSubscription> SubscribersSubscriptions { get; set; }

        [ForeignKey("SubscriptionId")]
        public virtual ICollection<PurchaseConfirmation> PurchaseConfirmations { get; set; }
        public virtual SubscriberSubscription SubscriptionPrice { get; set; }
        //public virtual ICollection<SubscriptionPrice> SubscriptionPrices { get; set; }

        public SubscriptionType()
        {
            this.SubscribersSubscriptions = new HashSet<SubscriberSubscription>();
            this.PurchaseConfirmations = new HashSet<PurchaseConfirmation>();
            //this.SubscriptionPrices = new HashSet<SubscriptionPrice>();
        }
    }
}
