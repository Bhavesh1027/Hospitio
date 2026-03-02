namespace HospitioApi.Data.Models;

public partial class CustomerPropertyGallery : Auditable
{
    public int? CustomerPropertyInformationId { get; set; }
    public string? PropertyImage { get; set; }
    public bool? IsPublish { get; set; }
    public string? JsonData { get; set; }
    public virtual CustomerPropertyInformation? CustomerPropertyInformation { get; set; }
}
