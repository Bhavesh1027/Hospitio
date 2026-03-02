using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerPaymentProcessorCredentials : Auditable
{
    public int CustomerId { get; set; }
    [MaxLength(200)]
    public string? MerchantId { get; set; }
    [MaxLength(200)]
    public string? ClientId { get; set; }
    [MaxLength(200)]
    public string? SecretKey { get; set; }
    public virtual Customer? Customer { get; set; }
}
