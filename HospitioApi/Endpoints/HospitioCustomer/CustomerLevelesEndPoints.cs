using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleUserLevels.Queries.GetCustomerLevels;
using HospitioApi.Helpers;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerLevelesEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
     plural__: "api/hospitio-customer/userlevels");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetCystomerLevelsAsync)
        .AllowAnonymous()
    };

    #region Delegate
    private async Task<IResult> GetCystomerLevelsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetCustomerLevelsRequest(UserTypeEnum.Customer), ct);
    #endregion
}
