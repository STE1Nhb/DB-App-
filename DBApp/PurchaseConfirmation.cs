using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBApp
{
    public class PurchaseConfirmation
    {
        [Required]  
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 1)]
        public int PurchaseId { get; set; }
        [Required]
        //[Key]
        [ForeignKey("SubscriberSubscription")]
        [Column(Order = 2)]
        public int SubscriberId { get; set; }
        [Required]
        [Column(Order = 3)]
        //[Key]
        [ForeignKey("SubscriberSubscription")]
        public int SubscriptionId { get; set; }
        [Required]
        [ForeignKey("SubscriptionPrice")]
        public int Price { get; set; }
        //public PurchaseConfirmation()
        //{
        //    this.ExpirationDates = new HashSet<ExpirationDate>();
        //}
        [Required]
        public DateTime PurchaseDate { get; set; }

        public virtual SubscriptionPrice SubscriptionPrice { get; set; }
        public virtual SubscriberSubscription SubscriberSubscription { get; set; }
        public virtual ExpirationDate ExpirationDate { get; set; }

        //public virtual ICollection<ExpirationDate> ExpirationDates { get; set; }
    }
}
