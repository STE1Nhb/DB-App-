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
        [Column(Order = 1)]
        public int SubscriberId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }

        public virtual SubscriberSubscription SubscriberSubscriber { get; set; }

        public virtual ICollection<SubscriberSubscription> SubscribersSubscriptions { get; set; }

        public Subscriber()
        {
            this.SubscribersSubscriptions = new HashSet<SubscriberSubscription>();
        }
    }
}
