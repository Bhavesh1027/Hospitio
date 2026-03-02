using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models
{
    public partial class CustomerPaymentProcessor : Auditable
    {
        public int? CustomerId { get; set; }
        public int? PaymentProcessorId { get; set; }
        [MaxLength(200)]
        public string? GRPaymentServiceId { get; set; }
        public string? GRAcceptedCountries { get; set; }
        public string? GRAcceptedCurrencies { get; set; }
        [MaxLength(200)]
        public string? GRWebhookURL { get; set; }
        public bool GR3DSecureEnabled { get; set; }
        public string? GRMerchantProfile { get; set; }
        public string? GRFields { get; set; }
        public bool GRIsDeleted { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual PaymentProcessor? PaymentProcessor { get; set; }

    }
}
