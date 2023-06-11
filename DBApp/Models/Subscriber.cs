using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBApp
{
    [Table("subscribers")]
    public class Subscriber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("subscriber_id",Order = 1)]
        public int SubscriberId { get; set; }

        [Column("subscriber_email")]
        [MaxLength(50)]
        [Required]
        public string Email { get; set; }

        [Column("subscriber_birth_date")]
        [Required]
        public DateTime BirthDate { get; set; }

        public virtual SubscriberSubscription SubscriberSubscriber { get; set; }

        public virtual ICollection<SubscriberSubscription> SubscribersSubscriptions { get; set; }

        [ForeignKey("SubscriberId")]
        public virtual ICollection<PurchaseConfirmation> PurchaseConfirmations { get; set; }


        public Subscriber()
        {
            this.SubscribersSubscriptions = new HashSet<SubscriberSubscription>();
            this.PurchaseConfirmations = new HashSet<PurchaseConfirmation>();
        }
    }
}
