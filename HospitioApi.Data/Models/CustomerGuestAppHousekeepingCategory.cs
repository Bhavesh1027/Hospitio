using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerGuestAppHousekeepingCategory : Auditable
{
    public CustomerGuestAppHousekeepingCategory()
    {
        CustomerGuestAppHousekeepingItems = new HashSet<CustomerGuestAppHousekeepingItem>();
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
    public virtual ICollection<CustomerGuestAppHousekeepingItem> CustomerGuestAppHousekeepingItems { get; set; }
}
