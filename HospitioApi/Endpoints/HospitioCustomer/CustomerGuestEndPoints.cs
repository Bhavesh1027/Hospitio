using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleCustomerGuest.Commands.CreateCustomerChatWidgetUser;
using HospitioApi.Core.HandleCustomerGuest.Commands.CreateCustomerGuest;
using HospitioApi.Core.HandleCustomerGuest.Commands.DeleteCustomerGuest;
using HospitioApi.Core.HandleCustomerGuest.Commands.DownloadCustomerGuest;
using HospitioApi.Core.HandleCustomerGuest.Commands.EditCustomerGuest;
using HospitioApi.Core.HandleCustomerGuest.Commands.SendWelcomeMessage;
using HospitioApi.Core.HandleCustomerGuest.Commands.UpdateCustomerGuest;
using HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuestById;
using HospitioApi.Core.HandleCustomerGuest.Queries.GetCustomerGuests;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerGuestEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/guests",
        singular: "api/hospitio-customer/guest");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetCustomerGuestsAsync)
        .RequireAuthorization(),
        app.MapGet($"/{Route.Singular}",GetCustomerGuestAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteCustomerGuestAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateCustomerGuestAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/update", UpdateCustomerGuestAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/edit", EditCustomerGuestAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}/download", DownloadCustomerGuestAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/sendwelcomemessage", SendWelcomeMessageAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/createchatwidgetuser", CreateCustomerChatWidgetUserAsync)
        .AllowAnonymous(),
    };
    #region Delegates
    private async Task<IResult> GetCustomerGuestsAsync([FromServices] IMediatorHelper mtrHlpr, string? SearchValue, int? PageNo, int? PageSize, string? SortColumn, string? SortOrder, ClaimsPrincipal cp, CT ct)
    {
        GetCustomerGuestsIn @in = new()
        {
            SortOrder = SortOrder ?? "ASC",
            SortColumn = SortColumn ?? "Firstname",
            SearchValue = SearchValue ?? string.Empty,
            PageNo = PageNo ?? 1,
            PageSize = PageSize ?? 10,
        };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestsRequest(@in, Convert.ToInt32(cp.CustomerId()!)), ct);
    }
    private async Task<IResult> GetCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetCustomerGuestByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetCustomerGuestByIdRequest(@in), ct);
    }
    private async Task<IResult> DeleteCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
    {
        DeleteCustomerGuestIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteCustomerGuestRequest(@in), ct);
    }
    private async Task<IResult> CreateCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerGuestIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomerGuestRequest(@in, cp.CustomerId()), ct);
    private async Task<IResult> UpdateCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateCustomerGuestIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateCustomerGuestRequest(@in), ct);
    private async Task<IResult> EditCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] EditCustomerGuestIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new EditCustomerGuestRequest(@in), ct);
    private async Task<IResult> DownloadCustomerGuestAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new DownloadCustomerGuestsRequest(Convert.ToInt32(cp.CustomerId()!)), ct);
    private async Task<IResult> SendWelcomeMessageAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] SendWelcomeMessageIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new SendWelcomeMessageRequest(@in, cp.FindFirstValue("CustomerId")), ct);
    private async Task<IResult> CreateCustomerChatWidgetUserAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateCustomerChatWidgetUserIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateCustomerChatWidgetUserRequest(@in), ct);

    #endregion
}
