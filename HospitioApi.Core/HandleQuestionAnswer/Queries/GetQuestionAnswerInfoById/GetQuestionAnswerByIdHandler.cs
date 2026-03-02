using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleQuestionAnswer.Queries.GetQuestionAnswerInfoById;
public record GetQuestionAnswerByIdRequest(GetQuestionAnswerByIdIn In) : IRequest<AppHandlerResponse>;
public class GetQuestionAnswerByIdHandler : IRequestHandler<GetQuestionAnswerByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetQuestionAnswerByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetQuestionAnswerByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var result = await _dapper.GetAllJsonData<QuestionAnswerByIdOut>("[dbo].[GetQuestionAnswerById]"
     , spParams, cancellationToken,
     commandType: CommandType.StoredProcedure);

        if (result == null || result.Count <= 0)
        {
            return _response.Error("Question answer could not be found", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetQuestionAnswerByIdOut("Get question answer info successful.", result[0]));
    }
}
