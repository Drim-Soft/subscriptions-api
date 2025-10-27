using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubscriptionsApi.Models
{
    [Table("subscription")]
    public class Subscription
    {
        [Key]
        [Column("idsubscription")]
        public int IdSubscription { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;
    }
}
