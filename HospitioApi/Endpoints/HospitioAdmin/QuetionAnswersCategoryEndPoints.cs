using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleQaCategories.Commands.CreateQaCategory;
using HospitioApi.Core.HandleQaCategories.Commands.DeleteQaCategory;
using HospitioApi.Core.HandleQaCategories.Commands.UpdateQaCategory;
using HospitioApi.Core.HandleQaCategories.Queries.GetQaCategories;
using HospitioApi.Core.HandleQaCategories.Queries.GetQaCategory;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class QuetionAnswersCategoryEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-admin/qacategories",
        singular: "api/hospitio-admin/qacategory");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Plural}",GetQaCategoriesAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}",GetQaCategoryAsync)
        .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteQaCategoryAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/create", CreateQaCategoryAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/update", UpdateQaCategoryAsync)
        .AllowAnonymous()
    };
    #region Delegates
    private async Task<IResult> GetQaCategoriesAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new GetQaCategoriesRequest(), ct);
    private async Task<IResult> GetQaCategoryAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] long Id, CT ct)
    {
        GetQaCategoryIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetQaCategoryRequest(@in), ct);
    }
    private async Task<IResult> DeleteQaCategoryAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] long qaCategoryId, CT ct)
    {
        DeleteQaCategoryIn @in = new() { QaCategoryId = qaCategoryId };
        return await mtrHlpr.ToResultAsync(new DeleteQaCategoryRequest(@in), ct);
    }
    private async Task<IResult> CreateQaCategoryAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateQaCategoryIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateQaCategoryRequest(@in), ct);
    private async Task<IResult> UpdateQaCategoryAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateQaCategoryIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new UpdateQaCategoryRequest(@in), ct);
    #endregion
}
