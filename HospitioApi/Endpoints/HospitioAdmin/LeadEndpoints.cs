using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleLeads.Commands.CreateLead;
using HospitioApi.Core.HandleLeads.Commands.EditLead;
using HospitioApi.Core.HandleLeads.Queries.DownloadLead;
using HospitioApi.Core.HandleLeads.Queries.GetLeadById;
using HospitioApi.Core.HandleLeads.Queries.GetLeads;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class LeadEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-admin/leads",
        singular: "api/hospitio-admin/lead");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {

        app.MapGet($"/{Route.Plural}", GetLeadsAsync)
            .AllowAnonymous(),

        app.MapGet($"/{Route.Singular}", GetLeadByIdAsync)
            .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/edit", EditLeadAsync)
            .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateEditLeadAsync)
            .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}/downloadLead", DownloadLeadAsync)
            .AllowAnonymous()
    };

    #region Delegates
    private async Task<IResult> GetLeadsAsync([FromServices] IMediatorHelper mtrHlpr, string? SearchValue, int PageNo, int PageSize, string? SortColumn, string? SortOrder, string? AlphabetsStartsWith, CT ct)
    {
        GetLeadsIn @in = new()
        {
            SearchValue = SearchValue ?? string.Empty,
            PageNo = PageNo,
            PageSize = PageSize,
            SortColumn = SortColumn ?? string.Empty,
            SortOrder = SortOrder ?? string.Empty,
            AlphabetsStartsWith = AlphabetsStartsWith ?? string.Empty,
        };

        return await mtrHlpr.ToResultAsync(new GetLeadsRequest(@in), ct);
    }
    private async Task<IResult> GetLeadByIdAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetLeadByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetLeadByIdRequest(@in), ct);
    }
    private async Task<IResult> CreateEditLeadAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateLeadIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateLeadRequest(@in), ct);
    private async Task<IResult> EditLeadAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] EditLeadIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new EditLeadRequest(@in), ct);
    private async Task<IResult> DownloadLeadAsync([FromServices] IMediatorHelper mtrHlpr, CT ct)
    => await mtrHlpr.ToResultAsync(new DownloadLeadRequest(), ct);

    #endregion
}