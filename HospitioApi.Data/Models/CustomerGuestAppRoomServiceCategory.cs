using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerGuestAppRoomServiceCategory : Auditable
{
    public CustomerGuestAppRoomServiceCategory()
    {
        CustomerGuestAppRoomServiceItems = new HashSet<CustomerGuestAppRoomServiceItem>();
    }

    public int? CustomerGuestAppBuilderId { get; set; }
    public int? CustomerId { get; set; }
    [MaxLength(25)]
    public string? CategoryName { get; set; }
    public int? DisplayOrder { get; set; }
    public bool? IsPublish { get; set; }
    public string? JsonData { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual CustomerGuestAppBuilder? CustomerGuestAppBuilder { get; set; }
    public virtual ICollection<CustomerGuestAppRoomServiceItem> CustomerGuestAppRoomServiceItems { get; set; }
}
