using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubscriptionsApi.Models
{
    [Table("currency")]
    public class Currency
    {
        [Key]
        [Column("idcurrency")]
        public int IdCurrency { get; set; }

        [Column("name")]
        public required string Name { get; set; }
    }
}
