using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DBApp
{
    public class SubscriberSubscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubscriberId { get; set; }
        [Key]
        public int SubscriptionId { get; set; }

        public SubscriberSubscription()
        {
            this.PurchasesConfirmations = new HashSet<PurchaseConfirmation>();
        }
        public virtual SubscriptionType SubscriptionType { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        public virtual ICollection<PurchaseConfirmation> PurchasesConfirmations { get; set; }

        //public virtual PlayersScores PlayersScores { get; set; }
    }
}
