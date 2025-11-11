using System;

namespace SubscriptionsApi.Models
{
    public class InvoiceRequestDTO
    {
        // Datos del cliente
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        // Datos adicionales
        public int AccessCount { get; set; }

        // Mapeo para la factura real
        public int IdSubscription { get; set; }
        public int IdSubscriptionStatus { get; set; }
        public int IdPaymentMethod { get; set; }
        public int IdCurrency { get; set; }
        public int IdOrganization { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
