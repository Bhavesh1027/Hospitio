using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerGuestAppEnhanceYourStayCategory : Auditable
{
    public CustomerGuestAppEnhanceYourStayCategory()
    {
        CustomerGuestAppEnhanceYourStayItems = new HashSet<CustomerGuestAppEnhanceYourStayItem>();
    }

    public int? CustomerGuestAppBuilderId { get; set; }
    public int? CustomerId { get; set; }
    [MaxLength(50)]
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
    public string? JsonData { get; set; }
    public bool? IsPublish { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual CustomerGuestAppBuilder? CustomerGuestAppBuilder { get; set; }
    public virtual ICollection<CustomerGuestAppEnhanceYourStayItem> CustomerGuestAppEnhanceYourStayItems { get; set; }
}
