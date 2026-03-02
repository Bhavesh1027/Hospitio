using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Data.Models;

public partial class ScreenDisplayOrderAndStatus : Auditable
{
    public int? ScreenName { get; set; }
    public string? JsonData { get; set; }
    public int? RefrenceId { get; set; }
    public bool? IsPublish { get; set; }
    public string? ScreenJsonData { get; set; }
}
