using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.CreateCustomerAppBuilder;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.DeleteCustomerAppBuilder;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.UpdateCustomerAppBuilder;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerAppBuilderById;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerAppBuilders;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestAppBuilderByCustomerRoomId;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerGuestAppBuilderEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/guestappbuilders",
        singular: "api/hospitio-customer/guestappbuilder");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetCustomerGuestAppBuildersAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}",GetCustomerGuestAppBuilderAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}/Get",GetCustomerGuestAppBuilderByRoomIdAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerGuestAppBuilderAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerGuestAppBuilderAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/update", UpdateCustomerGuestAppBuilderAsync)
        .RequireAuthorization()
    };
    #region Delegates
    private async Task<IResult> GetCustomerGuestAppBuildersAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "CustomerId")] int CustomerId, string? SearchColumn, string? SearchValue, int? PageNo, int? PageSize, string? SortColumn, string? SortOrder, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerGuestAppBuildersIn @in = new()
        {
            CustomerId = CustomerId,
            SortOrder = SortOrder ?? "ASC",
            SortColumn = SortColumn ?? "CustomerRoomNameId",
            SearchColumn = SearchColumn ?? string.Empty,
            SearchValue = SearchValue ?? string.Empty,
            PageNo = PageNo ?? 1,
            PageSize = PageSize ?? 10,
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestAppBuildersRequest(@in), ct);
    }
    private async Task<IResult> GetCustomerGuestAppBuilderAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetCustomerGuestAppBuilderByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestAppBuilderByIdRequest(@in), ct);
    }
    private async Task<IResult> GetCustomerGuestAppBuilderByRoomIdAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "RoomId")] int RoomId, CT ct)
    {
        GetCustomerGuestAppBuilderByCustomerRoomIdIn @in = new() 
        { 
            RoomId = RoomId, 
            CustomerId = Convert.ToInt32(cp.CustomerId()),
            UserType= (UserTypeEnum.Customer).ToString()
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestAppBuilderByCustomerRoomIdRequest(@in), ct);
    }
    private async Task<IResult> DeleteCustomerGuestAppBuilderAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerGuestAppBuilderIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerGuestAppBuilderRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerGuestAppBuilderAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerGuestAppBuilderIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomerGuestAppBuilderRequest(@in), ct);
    private async Task<IResult> UpdateCustomerGuestAppBuilderAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerGuestAppBuilderIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerGuestAppBuilderRequest(@in,cp.CustomerId()!), ct);
    #endregion
}
