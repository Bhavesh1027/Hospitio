using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleTicket.Commands.CloseTicket;
using HospitioApi.Core.HandleTicket.Commands.CreateTicket;
using HospitioApi.Core.HandleTicket.Commands.CreateTicketReply;
using HospitioApi.Core.HandleTicket.Commands.ForwardTicket;
using HospitioApi.Core.HandleTicket.Commands.UpdateTicketPriority;
using HospitioApi.Core.HandleTicket.Queries.GetRecentTickets;
using HospitioApi.Core.HandleTicket.Queries.GetTicketById;
using HospitioApi.Core.HandleTicket.Queries.GetTickets;
using HospitioApi.Core.HandleTicket.Queries.GetTicketsWithFilters;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class TicketEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-admin/tickets",
        singular: "api/hospitio-admin/ticket");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) =>
        new[]
        {
            app.MapPost($"/{Route.Plural}/create", CreateAsync)
            .RequireAuthorization(),

            app.MapPost($"/{Route.Singular}/replies/create", CreateTicketReplyAsync)
            .RequireAuthorization(),

            app.MapGet($"/{Route.Plural}/" +"{Id}", GetByIdAsync)
            .RequireAuthorization(),

            app.MapGet($"/{Route.Plural}" , GetAllAsync)
            .RequireAuthorization(),

            app.MapGet($"/{Route.Plural}/recent" , GetAllRecentAsync)
            .RequireAuthorization(),

            app.MapPost($"/{Route.Plural}/filter" , GetTicketsWithFiltersAsync)
            .RequireAuthorization(),
            app.MapPost($"/{Route.Plural}/close" , UpdateTicketClosesAsync)
            .AllowAnonymous(),
            app.MapPost($"/{Route.Plural}/updateticketpriority" , UpdateTicketPriorityAsync)
            .AllowAnonymous(),
             app.MapPost($"/{Route.Plural}/forward" , ForwardTicketAsync)
            .AllowAnonymous()

        };

    #region Delegates

    private async Task<IResult> CreateAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateTicketIn @in, ClaimsPrincipal cp, CT ct)
    => await mtrHlpr.ToResultAsync(new CreateTicketRequest(@in, UserTypeEnum.Hospitio, cp.UserId()), ct);

    private async Task<IResult> CreateTicketReplyAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateTicketReplyIn @in, ClaimsPrincipal cp, CT ct)
    => await mtrHlpr.ToResultAsync(new CreateTicketReplyRequest(@in, UserTypeEnum.Hospitio, cp.UserId()), ct);

    private async Task<IResult> GetByIdAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, CT ct)
        => await mtrHlpr.ToResultAsync(new GetTicketByIdRequest(Id), ct);
      
    private async Task<IResult> GetAllAsync([FromServices] IMediatorHelper mtrHlpr,ClaimsPrincipal CP, CT ct)
        => await mtrHlpr.ToResultAsync(new GetTicketsRequest(CP.UserId()), ct);

    private async Task<IResult> GetAllRecentAsync([FromServices] IMediatorHelper mtrHlpr, int CustomerId, CT ct)
    {
        GetRecentTicketIn @in = new() { CustomerId = CustomerId };
        return await mtrHlpr.ToResultAsync(new GetRecentTicketsRequest(@in), ct);
    }

    private async Task<IResult> GetTicketsWithFiltersAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] GetTicketsWithFiltersIn @in, ClaimsPrincipal CP,CT ct)
       => await mtrHlpr.ToResultAsync(new GetTicketsWithFiltersRequest(@in, CP.UserId(),UserTypeEnum.Hospitio), ct);

    private async Task<IResult> UpdateTicketClosesAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CloseTicketIn @in, CT ct)
       => await mtrHlpr.ToResultAsync(new CloseTicketRequest(@in), ct);
    private async Task<IResult> UpdateTicketPriorityAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateTicketPriorityIn @in, CT ct)
       => await mtrHlpr.ToResultAsync(new UpdateTicketPriorityRequest(@in), ct);
    private async Task<IResult> ForwardTicketAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] ForwardTicketIn @in, CT ct)
      => await mtrHlpr.ToResultAsync(new ForwardTicketRequest(@in), ct);
    #endregion

}
