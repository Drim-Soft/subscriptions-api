using System;
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

        [Column("total", TypeName = "numeric")]
        public decimal Total { get; set; }

        [Column("startdate", TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column("enddate", TypeName = "date")]
        public DateTime EndDate { get; set; }

        // Relaciones
        public Subscription? Subscription { get; set; }
        public SubscriptionStatus? SubscriptionStatus { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public Currency? Currency { get; set; }
    }
}
