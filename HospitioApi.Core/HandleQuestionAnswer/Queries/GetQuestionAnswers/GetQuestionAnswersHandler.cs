using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;
namespace HospitioApi.Core.HandleQuestionAnswer.Queries.GetQuestionAnswers;
public record GetQuestionAnswersRequest(GetQuestionAnswersIn? In)
    : IRequest<AppHandlerResponse>;

public class GetQuestionAnswersHandler : IRequestHandler<GetQuestionAnswersRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    public GetQuestionAnswersHandler( IDapperRepository dapper,
    IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetQuestionAnswersRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("SearchValue", request.In.SearchValue == null ? "" : request.In.SearchValue, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        spParams.Add("SortColumn", request.In.SortColumn == null ? "" : request.In.SortColumn, DbType.String);
        spParams.Add("SortOrder", request.In.SortOrder == null ? "" : request.In.SortOrder, DbType.String);
        spParams.Add("CategoryId", request.In.CategoryId, DbType.Int32);
        spParams.Add("IsViewAll", request.In.IsViewAll, DbType.Boolean);
        spParams.Add("IsShowActiveOnly", request.In.IsShowActiveOnly, DbType.Boolean);

        var questionAnswersOut = await _dapper.GetAll<QuestionAnswersOut>("[dbo].[GetQuestionAnswers]"
        , spParams, cancellationToken,
        commandType: CommandType.StoredProcedure);
        if (questionAnswersOut == null || questionAnswersOut.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetQuestionAnswersOut("Get question answer successful.", questionAnswersOut));
    }
}