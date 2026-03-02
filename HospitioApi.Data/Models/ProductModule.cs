using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Data.Models;

public partial class ProductModule : Auditable
{
    public ProductModule()
    {
        ProductModuleServices = new HashSet<ProductModuleService>();
    }

    public int ProductId { get; set; }
    public int ModuleId { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    [MaxLength(3)]
    public string? Currency { get; set; }

    //Module2Type mean it's have Dropdown list value selection e.g Pricing Period
    [MaxLength(20)]
    public string? Module2TypeValue { get; set; }
    public virtual Product? Product { get; set; }
    public virtual Module? Module { get; set; }

    public virtual ICollection<ProductModuleService>? ProductModuleServices { get; set; }





}
