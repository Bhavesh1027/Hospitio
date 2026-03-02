using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplates;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class GuestJourneyMessagesTemplatesForCustomerEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/guestjourneymessagestemplates");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetAsync)
        .AllowAnonymous()
    };
    #region Delegates
    private async Task<IResult> GetAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetGuestJourneyMessagesTemplatesForCustomerRequest(), ct);
    #endregion
}
