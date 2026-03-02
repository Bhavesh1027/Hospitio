using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomersGuestJourneys.Commands.CreateCustomersGuestJourneys;
using HospitioApi.Core.HandleCustomersGuestJourneys.Commands.DeleteCustomersGuestJourneys;
using HospitioApi.Core.HandleCustomersGuestJourneys.Commands.UpdateCustomersGuestJourneys;
using HospitioApi.Core.HandleCustomersGuestJourneys.Commands.UpdateIsActiveCustomersGuestJourneys;
using HospitioApi.Core.HandleCustomersGuestJourneys.Queries.GetCustomersGuestJourneys;
using HospitioApi.Core.HandleCustomersGuestJourneys.Queries.GetCustomersGuestJourneysById;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerGuestJourneyEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/guestjourneys",
       singular: "api/hospitio-customer/guestjourney");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetCustomerGuestJourneysAsync)
        .RequireAuthorization(),
        app.MapGet($"/{Route.Singular}",GetCustomerGuestJourneyAsync)
        .RequireAuthorization(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerGuestJourneyAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerGuestJourneyAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/updateIsActive", UpdateIsActiveCustomerGuestJourneyAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/update", UpdateCustomerGuestJourneyAsync)
        .RequireAuthorization()
        };

    #region Delegates
    private async Task<IResult> GetCustomerGuestJourneysAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp,CT ct)
    {       
        return await mtrHlpr.ToResultAsync(new GetCustomersGuestJourneysRequest(cp.CustomerId()!), ct);
    }
    private async Task<IResult> GetCustomerGuestJourneyAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetCustomersGuestJourneysByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetCustomersGuestJourneysByIdRequest(@in), ct);
    }
    private async Task<IResult> DeleteCustomerGuestJourneyAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteCustomersGuestJourneysIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomersGuestJourneysRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerGuestJourneyAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateCustomersGuestJourneysIn @in, ClaimsPrincipal cp, CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        return await mtrHlpr.ToResultAsync(new CreateCustomersGuestJourneysRequest(@in), ct);
    }

    private async Task<IResult> UpdateCustomerGuestJourneyAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateCustomersGuestJourneysIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomersGuestJourneysRequest(@in), ct);
    private async Task<IResult> UpdateIsActiveCustomerGuestJourneyAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateIsActiveCustomersGuestJourneyIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateIsActiveCustomersGuestJourneysRequest(@in), ct);
    #endregion
}
