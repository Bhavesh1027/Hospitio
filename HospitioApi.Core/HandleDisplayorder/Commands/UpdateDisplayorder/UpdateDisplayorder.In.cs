using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Nodes;

namespace HospitioApi.Core.HandleDisplayorder.Commands.UpdateDisplayorder;

public class UpdateDisplayorderIn
{
    public int Id { get; set; }
    public int? ScreenName { get; set; }
    [Column(TypeName = "json")]
    public string? JsonData { get; set; }
    public int RefrenceId { get; set; }
}
