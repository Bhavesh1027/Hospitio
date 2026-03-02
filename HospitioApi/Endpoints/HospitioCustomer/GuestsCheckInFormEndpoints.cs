using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustGuestsCheckInFormBuilder.Queries.GetCustomerGuestsCheckInFormBuilder;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class GuestsCheckInFormEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
    plural__: "api/hospitio-customer/guest-checkIns",
    singular: "api/hospitio-customer/guest-checkIn");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
    app.MapGet($"/{Route.Singular}formfields",GetCustomerGuestsCheckInFormBuilderAsync)
    .RequireAuthorization(),
};
    #region Delegates
    private async Task<IResult> GetCustomerGuestsCheckInFormBuilderAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerGuestsCheckInFormBuilderIn @in = new() { CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId")) };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestsCheckInFormBuilderRequest(@in), ct);
    }

    #endregion
}
