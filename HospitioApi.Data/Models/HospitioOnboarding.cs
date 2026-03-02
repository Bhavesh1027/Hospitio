using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HospitioApi.Data.Models;

public partial class HospitioOnboarding : Auditable
{
    [MaxLength(3)]
    public string? WhatsappCountry { get; set; }
    [MaxLength(20)]
    public string? WhatsappNumber { get; set; }
    [MaxLength(3)]
    public string? ViberCountry { get; set; }
    [MaxLength(20)]
    public string? ViberNumber { get; set; }
    [MaxLength(3)]
    public string? TelegramCounty { get; set; }
    [MaxLength(20)]
    public string? TelegramNumber { get; set; }
    [MaxLength(3)]
    public string? PhoneCountry { get; set; }
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    [MaxLength(200)]
    public string? SmsTitle { get; set; }
    [MaxLength(100)]
    public string? Messenger { get; set; }
    [MaxLength(100)]
    public string? Email { get; set; }
    [MaxLength(50)]
    public string? Cname { get; set; }
    //[MaxLength(50)]
    //public string? ClientDoamin { get; set; }
    [MaxLength(10)]
    public string? IncomingTranslationLangage { get; set; }
    public string? NoTranslateWords { get; set; }
    [DefaultValue(12)]
    public int TaxiTransCommission { get; set; }
}
