namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.CreateCustomerPropertyExtras;

public class CreateCustomerPropertyExtrasIn
{
    public int? CustomerPropertyInformationId { get; set; }
    /// <summary>
    /// 1: Recommendation,  2: Around You
    /// </summary>
    public byte? ExtraType { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Link { get; set; }
    public bool? IsActive { get; set; }
}