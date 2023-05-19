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
        [Column(Order = 0)]
        public int PurchaseId { get; set; }
        [Key, Column(Order = 1)]
        public int SubscriberId { get; set; }
        [Key, Column(Order = 2)]
        public int SubscriptionId { get; set; }
        public int Price { get; set; }
        public PurchaseConfirmation()
        {
            this.ExpirationDate = new HashSet<ExpirationDate>();
        }
        public DateTime PurchaseDate { get; set; }
        public virtual SubscriptionPrice SubscriptionPrice { get; set; }
        public virtual SubscriberSubscription SubscriberSubscription { get; set; }

        public virtual ICollection<ExpirationDate> ExpirationDate { get; set; }
    }
}
