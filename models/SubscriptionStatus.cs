using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubscriptionsApi.Models
{
    [Table("subscriptionstatus")]
    public class SubscriptionStatus
    {
        [Key]
        [Column("idsubscriptionstatus")]
        public int IdSubscriptionStatus { get; set; }

        [Column("name")]
        public required string Name { get; set; }
    }
}
