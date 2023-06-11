using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace DBApp
{
    [Table("purchase_confirmations")]
    public class PurchaseConfirmation
    {
        [Required]  
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("purchase_id",Order = 1)]
        public int PurchaseId { get; set; }

        [Required]
        //[Key]
        [Column("subscriber_id",Order = 2)]
        //[ForeignKey("SubscriberSubscription")]
        public int SubscriberId { get; set; }

        [Required]
        [Column("type_id",Order = 3)]
        //[Key]
        //[ForeignKey("SubscriberSubscription")]
        public int SubscriptionId { get; set; }

        [Column("subscription_price")]
        [Required]
        public float Price { get; set; }

        [Column("purchase_date")]
        [Required]
        public DateTime PurchaseDate { get; set; }

        //public virtual SubscriptionPrice SubscriptionPrice { get; set; }

        public virtual Subscriber Subscriber { get; set; }
        public virtual SubscriptionType SubscriptionType { get; set; }

        //public virtual SubscriberSubscription SubscriberSubscription { get; set; }
        public virtual ExpirationDate ExpirationDate { get; set; }

        //public virtual ICollection<ExpirationDate> ExpirationDates { get; set; }
    }
}
