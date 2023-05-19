using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBApp
{
    public class Subscriber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubsciberId { get; private set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual ICollection<SubscriberSubscription> SubscribersSubscriptions { get; set; }

        public Subscriber() 
        {
            this.SubscribersSubscriptions = new HashSet<SubscriberSubscription>();
        }
    }
}
