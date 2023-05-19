using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBApp
{
    public class ExpirationDate
    {
        public int PurchaseId { get; set; }
        public DateTime Date { get; set; }

        public virtual PurchaseConfirmation PurchaseConfirmation { get; set; }
    }
}
