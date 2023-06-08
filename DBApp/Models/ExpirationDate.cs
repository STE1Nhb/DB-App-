using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBApp
{
    [Table("expiry_dates")]
    public class ExpirationDate
    {
        [Required]
        [Key]
        [ForeignKey("PurchaseConfirmation")]
        [Column("purchase_id",Order = 1)]
        public int PurchaseId { get; set; }

        [Column("expiry_date")]
        [Required]
        public DateTime ExpiryDate { get; set; }

        public virtual PurchaseConfirmation PurchaseConfirmation { get; set; }
    }
}
