using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleQuestionAnswer.Commands.CreateQuestionAnswer;
using HospitioApi.Core.HandleQuestionAnswer.Commands.DeleteQuestionAnswer;
using HospitioApi.Core.HandleQuestionAnswer.Commands.EditQuestionAnswer;
using HospitioApi.Core.HandleQuestionAnswer.Commands.UpdateQuestionAnswer;
using HospitioApi.Core.HandleQuestionAnswer.Commands.UpdateQuestionAnswerStatus;
using HospitioApi.Core.HandleQuestionAnswer.Queries.GetQuestionAnswerInfoById;
using HospitioApi.Core.HandleQuestionAnswer.Queries.GetQuestionAnswers;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;
public class QuestionAnswerEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-admin/questionanswers",
       singular: "api/hospitio-admin/questionanswer");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {

        app.MapPost($"/{Route.Singular}", CreateAsync)
            .AllowAnonymous(),
        app.MapDelete($"/{Route.Singular}", DeleteAsync)
        .RequireAuthorization(),
        app.MapPost($"/{Route.Singular}" + "/{Id}", EditAsync)
            .RequireAuthorization(),
         app.MapPost($"/{Route.Singular}" + "/update/{Id}", UpdateAsync)
            .RequireAuthorization(),
          app.MapPost($"/{Route.Singular}" + "/update-status/{Id}", UpdateStatusAsync)
            .RequireAuthorization(),
        app.MapGet($"/{Route.Singular}", GetByIdAsync)
            .RequireAuthorization(),

          app.MapGet($"/{Route.Plural}" , GetAllAsync)
            .RequireAuthorization(),
    };

    #region Delegates
    private async Task<IResult> CreateAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateQuestionAnswerIn @in, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateQuestionAnswerRequest(@in), ct);
    private async Task<IResult> DeleteAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery] int Id, CT ct)
    {
        DeleteQuestionAnswerIn @in = new() { QuestionAnswerId = Id };
        return await mtrHlpr.ToResultAsync(new DeleteQuestionAnswerRequest(@in), ct);
    }
    private async Task<IResult> EditAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, [FromBody] EditQuestionAnswerIn @in, ClaimsPrincipal cp, CT ct)
    {
        @in.Id = Id;
        return await mtrHlpr.ToResultAsync(new EditQuestionAnswerRequest(@in), ct);
    }
    private async Task<IResult> UpdateAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateQuestionAnswerIn @in, ClaimsPrincipal cp, CT ct)
       => await mtrHlpr.ToResultAsync(new UpdateQuestionAnswerRequest(@in), ct);
    private async Task<IResult> UpdateStatusAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateQuestionAnswerStatusIn @in, ClaimsPrincipal cp, CT ct)
       => await mtrHlpr.ToResultAsync(new UpdateQuestionAnswerStatusRequest(@in), ct);

    private async Task<IResult> GetAllAsync([FromServices] IMediatorHelper mtrHlpr, string? SearchValue, int PageNo, int PageSize, string? SortColumn, string? SortOrder, int? CategoryId, CT ct)
    {
        GetQuestionAnswersIn @in = new()
        {
            SearchValue = SearchValue,
            PageNo = PageNo,
            PageSize = PageSize,
            SortColumn = SortColumn,
            SortOrder = SortOrder,
            CategoryId = CategoryId,
            IsViewAll = true,
            IsShowActiveOnly = false
        };
        return await mtrHlpr.ToResultAsync(new GetQuestionAnswersRequest(@in), ct);
    }

    private async Task<IResult> GetByIdAsync([FromServices] IMediatorHelper mtrHlpr, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetQuestionAnswerByIdIn @in = new() { Id = Id };
        return await mtrHlpr.ToResultAsync(new GetQuestionAnswerByIdRequest(@in), ct);
    }

    #endregion
}