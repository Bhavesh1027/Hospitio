using HospitioApi.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace HospitioApi.Data;
public abstract class TenantIdentifier
{
    public string CustomerId { get; set; } = null!;

    [ForeignKey("TenantId")]
    public virtual Customer Tenant { get; set; } = null!;
}
