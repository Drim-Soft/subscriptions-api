using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubscriptionsApi.Models
{
    [Table("invoice")]
    public class Invoice
    {
        [Key]
        [Column("idinvoice")]
        public int IdInvoice { get; set; }

        [Column("idsubscription")]
        public int IdSubscription { get; set; }

        [Column("idsubscriptionstatus")]
        public int IdSubscriptionStatus { get; set; }

        [Column("idpaymentmethod")]
        public int IdPaymentMethod { get; set; }

        [Column("idcurrency")]
        public int IdCurrency { get; set; }

        [Column("idorganization")]
        public int IdOrganization { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        [Column("startdate")]
        public DateTime StartDate { get; set; }

        [Column("enddate")]
        public DateTime EndDate { get; set; }

        // 🔗 Relaciones explícitas (ForeignKey)
        [ForeignKey("IdSubscription")]
        public Subscription Subscription { get; set; }

        [ForeignKey("IdSubscriptionStatus")]
        public SubscriptionStatus SubscriptionStatus { get; set; }

        [ForeignKey("IdPaymentMethod")]
        public PaymentMethod PaymentMethod { get; set; }

        [ForeignKey("IdCurrency")]
        public Currency Currency { get; set; }
    }
}

