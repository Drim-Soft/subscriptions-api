using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubscriptionsApi.Models
{
    [Table("paymentmethod")]
    public class PaymentMethod
    {
        [Key]
        [Column("idpaymentmethod")]
        public int IdPaymentMethod { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;
    }
}
