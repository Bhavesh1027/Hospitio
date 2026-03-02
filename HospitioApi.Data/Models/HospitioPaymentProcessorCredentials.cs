using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class HospitioPaymentProcessorCredentials :Auditable
{
    [MaxLength(200)]
    public string? MerchantId { get; set; }
    [MaxLength(200)]
    public string? ClientId { get; set; }
    [MaxLength(200)]
    public string? SecretKey { get; set; }
}
