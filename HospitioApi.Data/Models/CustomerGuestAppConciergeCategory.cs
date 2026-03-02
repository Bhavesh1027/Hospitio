using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerGuestAppConciergeCategory : Auditable
{
    public CustomerGuestAppConciergeCategory()
    {
        CustomerGuestAppConciergeItems = new HashSet<CustomerGuestAppConciergeItem>();
    }

    public int? CustomerGuestAppBuilderId { get; set; }
    public int? CustomerId { get; set; }
    [MaxLength(50)]
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsPublish { get; set; }
    public string? JsonData { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual CustomerGuestAppBuilder? CustomerGuestAppBuilder { get; set; }
    public virtual ICollection<CustomerGuestAppConciergeItem> CustomerGuestAppConciergeItems { get; set; }
}
