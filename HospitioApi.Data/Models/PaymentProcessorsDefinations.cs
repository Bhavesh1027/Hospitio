namespace HospitioApi.Data.Models;

public partial class PaymentProcessorsDefinations : Auditable
{
    public int PaymentProcessorId { get; set; }
    /// <summary>
    ///  will remain null incase of digital wallet GRGroup etc..
    /// </summary>
    public string? GRFields { get; set; }
    public string? GRSupportedCountries { get;set; }
    public string? GRSupportedCurrencies { get;set; }
    public string? GRSupportedFeatures { get;set; }
    public virtual PaymentProcessor? PaymentProcessor { get; set; }
}
