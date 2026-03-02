using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleTicketCategories.Commands.CreateTicketCategory;
using HospitioApi.Core.HandleTicketCategories.Commands.DeleteTicketCategory;
using HospitioApi.Core.HandleTicketCategories.Commands.UpdateTicketCategory;
using HospitioApi.Core.HandleTicketCategories.Queries.GetTicketCategories;
using HospitioApi.Core.HandleTicketCategories.Queries.GetTicketCategory;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class TicketCategoryEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-admin/ticketcategories",
        singular: "api/hospitio-admin/ticketcategory");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"{Route.Plural}",GetTicketCategoriesAsync)
        .AllowAnonymous(),
         app.MapGet($"/{Route.Singular}",GetTicketCategoryAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteTicketCategoryAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateTicketCategoryAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/update", UpdateTicketCategoryAsync)
        .AllowAnonymous()
    };
    #region Delegates
    private async Task<IResult> GetTicketCategoriesAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetTicketCategoriesRequest(), ct);
    private async Task<IResult> GetTicketCategoryAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetTicketCategoryIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetTicketCategoryRequest(@in), ct);
    }
    private async Task<IResult> DeleteTicketCategoryAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int TicketCategoryId, CT ct)
    {
        DeleteTicketCategoryIn @in = new() { Id = TicketCategoryId };
        return await mtrHlpr.ToResultAsync(new DeleteTicketCategoryRequest(@in), ct);
    }
    private async Task<IResult> CreateTicketCategoryAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateTicketCategoryIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateTicketCategoryRequest(@in), ct);
    private async Task<IResult> UpdateTicketCategoryAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateTicketCategoryIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateTicketCategoryRequest(@in), ct);
    #endregion
}
