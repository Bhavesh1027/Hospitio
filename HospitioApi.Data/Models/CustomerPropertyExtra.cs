using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerPropertyExtra : Auditable
{

    public CustomerPropertyExtra()
    {
        CustomerPropertyExtraDetails = new  HashSet<CustomerPropertyExtraDetails>();
    }
    public int? CustomerPropertyInformationId { get; set; }
    /// <summary>
    /// 1: Recommendation,  2: Around You
    /// </summary>
    public byte? ExtraType { get; set; }
    [MaxLength(100)]
    public string? Name { get; set; }
    //[MaxLength(200)]
    //public string? Description { get; set; }
    //[MaxLength(200)]
    //public string? Link { get; set; }
    public bool? IsPublish { get; set; }
    public string? JsonData { get; set; }

    public int? DisplayOrder { get; set; }
    public virtual CustomerPropertyInformation? CustomerPropertyInformation { get; set; }

    public virtual ICollection<CustomerPropertyExtraDetails> CustomerPropertyExtraDetails { get; set; }
}
