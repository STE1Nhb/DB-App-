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
        [Required]
        [ForeignKey("SubscriptionType")]
        //[Column(Order = 2)]
        public int SubscriptionId { get; set; }
        [Required]
        [Key]
        public int Price { get; set; }

        public SubscriptionPrice()
        {
            this.PurchaseConfirmations = new HashSet<PurchaseConfirmation>();
        }
        public virtual SubscriptionType SubscriptionType { get; set; }

        public virtual ICollection<PurchaseConfirmation> PurchaseConfirmations { get; set; }
    }
}
