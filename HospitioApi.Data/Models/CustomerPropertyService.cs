using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerPropertyService : Auditable
{
    public CustomerPropertyService()
    {
        CustomerPropertyServiceImages = new HashSet<CustomerPropertyServiceImage>();
    }

    public int? CustomerPropertyInformationId { get; set; }
    [MaxLength(100)]
    public string? Name { get; set; }
    [MaxLength(100)]
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public bool? IsPublish { get; set; }
    public string? JsonData { get; set; }
    public virtual CustomerPropertyInformation? CustomerPropertyInformation { get; set; }
    public virtual ICollection<CustomerPropertyServiceImage> CustomerPropertyServiceImages { get; set; }
}
