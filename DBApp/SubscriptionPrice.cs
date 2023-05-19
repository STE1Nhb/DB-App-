using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBApp
{
    public class SubscriptionPrice
    {
        [Key]
        public int SubscriptionId { get; set; }
        [Key]
        public int Price { get; set; }

        public SubscriptionPrice()
        {
            this.PurchasesConfirmations = new HashSet<PurchaseConfirmation>();
        }
        public virtual SubscriptionType SubscriptionType { get; set; }

        public virtual ICollection<PurchaseConfirmation> PurchasesConfirmations { get; set; }
    }
}
