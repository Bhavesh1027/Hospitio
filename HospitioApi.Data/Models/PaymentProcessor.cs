using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class PaymentProcessor : Auditable
{
    public PaymentProcessor()
    {
        CustomerPaymentProcessors = new HashSet<CustomerPaymentProcessor>();
        HospitioPaymentProcessors = new HashSet<HospitioPaymentProcessor>();
        PaymentProcessorsDefinations = new HashSet<PaymentProcessorsDefinations>();
    }
    [MaxLength(200)]
    public string? GRID { get; set; }
    public string? GRIcon { get; set; }
    [MaxLength(100)]
    /// <summary>
    ///  payment-service, digital-wallet etc..
    /// </summary>
    public string? GRGroup { get; set; }
    [MaxLength(100)]
    /// <summary>
    ///  card, wallet etc..
    /// </summary>
    public string? GRCategory { get; set; }

    [MaxLength(50)]
    public string? GRName { get; set; }

    public virtual ICollection<CustomerPaymentProcessor> CustomerPaymentProcessors { get; set; }
    public virtual ICollection<HospitioPaymentProcessor> HospitioPaymentProcessors { get; set; }
    public virtual ICollection<PaymentProcessorsDefinations> PaymentProcessorsDefinations { get; set; }
}
