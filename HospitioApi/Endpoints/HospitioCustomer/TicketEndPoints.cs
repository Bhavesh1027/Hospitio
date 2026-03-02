using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleTicket.Commands.CreateTicket;
using HospitioApi.Core.HandleTicket.Commands.CreateTicketReply;
using HospitioApi.Core.HandleTicket.Queries.GetTicketById;
using HospitioApi.Core.HandleTicket.Queries.GetTicketsWithFilters;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class TicketEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-customer/tickets",
        singular: "api/hospitio-customer/ticket");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) =>
        new[]
        {
            app.MapPost($"/{Route.Plural}/create", CreateAsync)
            .RequireAuthorization(),

            app.MapPost($"/{Route.Singular}/replies/create", CreateTicketReplyAsync)
            .RequireAuthorization(),

            app.MapGet($"/{Route.Plural}/" +"{Id}", GetByIdAsync)
            .RequireAuthorization(),

            app.MapPost($"/{Route.Plural}" , GetAllAsync)
            .AllowAnonymous(),

        };

    #region Delegates

    private async Task<IResult> CreateAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateTicketIn @in, ClaimsPrincipal cp, CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        return await mtrHlpr.ToResultAsync(new CreateTicketRequest(@in, UserTypeEnum.Customer,cp.UserId()), ct);
    }

    private async Task<IResult> CreateTicketReplyAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateTicketReplyIn @in, ClaimsPrincipal cp, CT ct)
    => await mtrHlpr.ToResultAsync(new CreateTicketReplyRequest(@in, UserTypeEnum.Customer, cp.UserId()), ct);

    private async Task<IResult> GetByIdAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, CT ct)
      => await mtrHlpr.ToResultAsync(new GetTicketByIdRequest(Id), ct);

    private async Task<IResult> GetAllAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] GetTicketsWithFiltersIn @in, ClaimsPrincipal cp, CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.FindFirstValue("CustomerId"));
        return await mtrHlpr.ToResultAsync(new GetTicketsWithFiltersRequest(@in,cp.UserId(), UserTypeEnum.Customer), ct);
    }

    #endregion
}

