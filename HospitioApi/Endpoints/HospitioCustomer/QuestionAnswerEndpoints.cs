using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleQuestionAnswer.Queries.GetQuestionAnswers;
using HospitioApi.Helpers;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class QuestionAnswerEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
       plural__: "api/hospitio-customer/questionanswers");      
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {
          app.MapGet($"/{Route.Plural}" , GetAllAsync)
            .RequireAuthorization(),
    };

    #region Delegates    
    private async Task<IResult> GetAllAsync([FromServices] IMediatorHelper mtrHlpr, string? SearchValue, int PageNo, int PageSize, string? SortColumn, string? SortOrder, int? CategoryId, bool? IsViewAll,CT ct)
    {
        GetQuestionAnswersIn @in = new()
        {
            SearchValue = SearchValue,
            PageNo = PageNo,
            PageSize = PageSize,
            SortColumn = SortColumn,
            SortOrder = SortOrder,
            CategoryId = CategoryId,
            IsViewAll = IsViewAll,
            IsShowActiveOnly = true
        };
        return await mtrHlpr.ToResultAsync(new GetQuestionAnswersRequest(@in), ct);
    }
    #endregion
}
