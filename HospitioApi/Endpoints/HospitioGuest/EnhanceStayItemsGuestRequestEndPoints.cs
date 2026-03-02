using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleGuestRequestEnhanceStayItem.Commands.CreateGuestRequestEnhanceStayItem;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioGuest;

public class EnhanceStayItemsGuestRequestEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-guest/enhancestayitemguestrequests",
       singular: "api/hospitio-guest/enhancestayitemguestrequest");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapPost($"/{Route.Singular}/create", CreateGuestRequestAsync)
        .AllowAnonymous()
    };

    private async Task<IResult> CreateGuestRequestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateGuestRequestEnhanceStayItemIn @in, CT ct)
       => await mtrHlpr.ToResultAsync(new CreateGuestRequestEnhanceStayItemRequest(@in), ct);

}
