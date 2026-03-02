using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Data.Models;

public partial class CustomerRoomName : Auditable
{
    public CustomerRoomName()
    {
        CustomerGuestAppBuilders = new HashSet<CustomerGuestAppBuilder>();
    }
    public Guid Guid { get; set; }
    public string? AvantioAccomodationRefrence { get;set; } = null;

    /// <summary>
    /// 1=Centurion, 2=GEA
    /// </summary>
    public byte? GuidType { get; set; }

    public int? CustomerId { get; set; }

    [MaxLength(50)]
    public string? Name { get; set; }

    /// <summary>
    ///  1: User,   2: Customer
    /// </summary>
    public byte? CreatedFrom { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<CustomerGuestAppBuilder> CustomerGuestAppBuilders { get; set; }
}
