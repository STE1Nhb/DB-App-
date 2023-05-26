using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBApp
{
    public class ExpirationDate
    {
        [Required]
        [Key]
        [ForeignKey("PurchaseConfirmation")]
        [Column(Order = 1)]
        public int PurchaseId { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public virtual PurchaseConfirmation PurchaseConfirmation { get; set; }
    }
}
