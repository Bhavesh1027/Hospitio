using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.CreateCustomerEnhanceYourStay;
using HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.DeleteCustomerEnhanceYourStay;
using HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.DisplayOrderCustomerEnhanceYourStay;
using HospitioApi.Core.HandleCustomerEnhanceYourStay.Queries.GetCustomerEnhanceYourStay;
using HospitioApi.Core.HandleCustomerHouseKeeping.Commands.DisplayOrderCustomerHouseKeeping;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerEnhanceYourStayEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/enhance-stay",
       singular: "api/hospitio-customer/enhance-stay");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}", GetCustomerEnhanceYourStayAsync)
        .RequireAuthorization(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerEnhanceYourStayAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerEnhanceYourStayAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/displayorderupdate", UpdateDisplayOrderCustomerEnhanceYourStayAsync)
        .RequireAuthorization()
        };

    #region Delegates
    private async Task<IResult> GetCustomerEnhanceYourStayAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "BuilderId")] int BuilderId, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerEnhanceYourStayIn @in = new()
        {
            BuilderId = BuilderId
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerEnhanceYourStayRequest(@in, cp.UserType()), ct);

    }
    private async Task<IResult> DeleteCustomerEnhanceYourStayAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int CategoryId, CT ct)
    {
        DeleteCustomerEnhanceYourStayIn @in = new() { CategoryId = CategoryId };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerEnhanceYourStayRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerEnhanceYourStayAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateCustomerEnhanceYourStayIn @in, ClaimsPrincipal cp,CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        return await mtrHlpr.ToResultAsync(new CreateCustomerEnhanceYourStayRequest(@in), ct);
    }

    private async Task<IResult> UpdateDisplayOrderCustomerEnhanceYourStayAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] DisplayOrderCustomerEnhanceYourStayIn @in, CT ct)
    => await mtrHlpr.ToResultAsync(new DisplayOrderCustomerEnhanceYourStayRequest(@in), ct);
    #endregion
}
