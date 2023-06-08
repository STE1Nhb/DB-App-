using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBApp
{
    [Table("subscription_prices")]
    public class SubscriptionPrice
    {
        [Column("type_id")]
        [Required]
        [Key]
        [ForeignKey("SubscriptionType")]
        //[Column(Order = 2)]
        public int SubscriptionId { get; set; }

        [Column("subscription_price")]
        [Required]
        public float Price { get; set; }

        public SubscriptionPrice()
        {
            this.PurchaseConfirmations = new HashSet<PurchaseConfirmation>();
        }
        public virtual SubscriptionType SubscriptionType { get; set; }

        public virtual ICollection<PurchaseConfirmation> PurchaseConfirmations { get; set; }
    }
}
