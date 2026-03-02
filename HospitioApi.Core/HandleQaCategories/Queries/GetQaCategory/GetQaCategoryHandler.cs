using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleQaCategories.Queries.GetQaCategory;
public record GetQaCategoryRequest(GetQaCategoryIn In) : IRequest<AppHandlerResponse>;

public class GetQaCategoryHandler : IRequestHandler<GetQaCategoryRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    public GetQaCategoryHandler(IHandlerResponseFactory response, IDapperRepository dapper)
    {
        _response = response;
        _dapper = dapper;
    }
    public async Task<AppHandlerResponse> Handle(GetQaCategoryRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var qaCategory = await _dapper.GetSingle<QaCategoryOut>("[dbo].[GetQaCategoryById]", spParams, cancellationToken, CommandType.StoredProcedure);
        if (qaCategory == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetQaCategoryOut("Get qa category successful.", qaCategory));
    }
}
