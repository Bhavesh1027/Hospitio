using Microsoft.EntityFrameworkCore.Metadata.Internal;
using HospitioApi.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitioApi.Core.HandleDisplayorder.Queries.GetDisplayOrder;

public class GetDisplayOrderOut : BaseResponseOut
{
    public GetDisplayOrderOut(string message, DisplayOrderOut displayOrderOut) : base(message)
    {
        DisplayOrderOut = displayOrderOut;
    }
    public DisplayOrderOut DisplayOrderOut { get; set; }
}
public class DisplayOrderOut
{
    public int Id { get; set; }
    public int? ScreenName { get; set; }
    public string? JsonData { get; set; }
    public int RefrenceId { get; set; }
}
