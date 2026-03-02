using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class BusinessType : Auditable
{
    public BusinessType()
    {
        Customers = new HashSet<Customer>();
        Notifications = new HashSet<Notification>();

    }
    [MaxLength(100)]
    public string? BizType { get; set; }
    public virtual ICollection<Customer> Customers { get; set; }
    public virtual ICollection<Notification> Notifications { get; set; }

}
