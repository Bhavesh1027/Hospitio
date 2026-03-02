using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerPropertyInformation : Auditable
{
    public CustomerPropertyInformation()
    {
        CustomerPropertyEmergencyNumbers = new HashSet<CustomerPropertyEmergencyNumber>();
        CustomerPropertyExtras = new HashSet<CustomerPropertyExtra>();
        CustomerPropertyGalleries = new HashSet<CustomerPropertyGallery>();
        CustomerPropertyServices = new HashSet<CustomerPropertyService>();
    }


    public int? CustomerId { get; set; }
    public int? CustomerGuestAppBuilderId { get; set; }
    [MaxLength(30)]
    public string? WifiUsername { get; set; }
    [MaxLength(30)]
    public string? WifiPassword { get; set; }
    public string? Overview { get; set; }
    public string? CheckInPolicy { get; set; }
    public string? TermsAndConditions { get; set; }
    [MaxLength(100)]
    public string? Street { get; set; }
    [MaxLength(5)]
    public string? StreetNumber { get; set; }
    [MaxLength(20)]
    public string? City { get; set; }
    [MaxLength(10)]
    public string? Postalcode { get; set; }
    [MaxLength(3)]
    public string? Country { get; set; }
    public bool? IsPublish { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public string? JsonData { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual CustomerGuestAppBuilder? CustomerGuestAppBuilder { get; set; }
    public virtual ICollection<CustomerPropertyEmergencyNumber> CustomerPropertyEmergencyNumbers { get; set; }
    public virtual ICollection<CustomerPropertyExtra> CustomerPropertyExtras { get; set; }
    public virtual ICollection<CustomerPropertyGallery> CustomerPropertyGalleries { get; set; }
    public virtual ICollection<CustomerPropertyService> CustomerPropertyServices { get; set; }
}
