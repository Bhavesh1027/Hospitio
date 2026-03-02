using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class Product : Auditable
{
    public Product()
    {
        Notifications = new HashSet<Notification>();
        ProductModuleServices = new HashSet<ProductModuleService>();
        ProductModules = new HashSet<ProductModule>();
    }

    [MaxLength(30)]
    public string? Name { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; }
    public virtual ICollection<ProductModuleService> ProductModuleServices { get; set; }
    public virtual ICollection<ProductModule> ProductModules { get; set; }

}
