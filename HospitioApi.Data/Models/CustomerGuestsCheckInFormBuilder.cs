using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class CustomerGuestsCheckInFormBuilder : Auditable
{
    public CustomerGuestsCheckInFormBuilder()
    {
        CustomerGuestsCheckInFormFields = new HashSet<CustomerGuestsCheckInFormField>();
    }

    public int? CustomerId { get; set; }
    [DefaultValue("#000000")]
    [MaxLength(6)]
    public string? Color { get; set; }
    [MaxLength(100)]
    public string? Name { get; set; }
    /// <summary>
    /// 1, 2, 3,  4, 5
    /// </summary>
    [DefaultValue(3)]
    public byte? Stars { get; set; }
    [MaxLength(500)]
    public string? Logo { get; set; }
    [MaxLength(500)]
    public string? AppImage { get; set; }
    [MaxLength(500)]
    public string? SplashScreen { get; set; }
    [DefaultValue(1)]
    public bool? IsOnlineCheckInFormEnable { get; set; }
    [DefaultValue(1)]
    public bool? IsRedirectToGuestAppEnable { get; set; }
    [MaxLength(100)]
    public string? SubmissionMail { get; set; }
    public string? TermsLink { get; set; }
    public string? GuestWelcomeMessage { get; set; }
    public string? JsonData { get; set; }
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<CustomerGuestsCheckInFormField> CustomerGuestsCheckInFormFields { get; set; }

}
