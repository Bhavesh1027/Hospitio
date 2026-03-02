using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.CreateGuestJourneyMessagesTemplates;
using HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.DeleteGuestJourneyMessagesTemplates;
using HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.UpdateGuestJourneyMessagesTemplates;
using HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplateById;
using HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplates;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class GuestJourneyMessagesTemplatesEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-admin/guestjourneymessagestemplates",
        singular: "api/hospitio-admin/guestjourneymessagestemplate");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetGuestJourneyMessagesTemplatesAsync)
        .RequireAuthorization(),
        app.MapGet($"/{Route.Singular}",GetGuestJourneyMessagesTemplateAsync)
        .RequireAuthorization(),
        app.MapDelete($"/{Route.Singular}", DeleteGuestJourneyMessagesTemplateAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/create", CreateGuestJourneyMessagesTemplateAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}/update", UpdateGuestJourneyMessagesTemplateAsync)
        .RequireAuthorization()
    };
    #region Delegates
    private async Task<IResult> GetGuestJourneyMessagesTemplatesAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetGuestJourneyMessagesTemplatesRequest(), ct);
    private async Task<IResult> GetGuestJourneyMessagesTemplateAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetGuestJourneyMessagesTemplateByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetGuestJourneyMessagesTemplateByIdReuest(@in), ct);
    }
    private async Task<IResult> DeleteGuestJourneyMessagesTemplateAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
    {
        DeleteGuestJourneyMessagesTemplatesIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new DeleteGuestJourneyMessagesTemplatesRequest(@in), ct);
    }
    private async Task<IResult> CreateGuestJourneyMessagesTemplateAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateGuestJourneyMessagesTemplatesIn @in, CT ct)
    {
        @in.UserId = Convert.ToInt32(cp.UserId());
        return await mtrHlpr.ToResultAsync(new CreateGuestJourneyMessagesTemplatesRequest(@in), ct);
    }
    private async Task<IResult> UpdateGuestJourneyMessagesTemplateAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateGuestJourneyMessagesTemplatesIn @in, CT ct)
    {
        @in.UserId = Convert.ToInt32(cp.UserId());
        return await mtrHlpr.ToResultAsync(new UpdateGuestJourneyMessagesTemplatesRequest(@in), ct);
    }
    #endregion
}
