using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleQaCategories.Queries.GetQaCategories;
public record GetQaCategoriesRequest() : IRequest<AppHandlerResponse>;
public class GetQaCategoriesHandler : IRequestHandler<GetQaCategoriesRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetQaCategoriesHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetQaCategoriesRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();

        var result = await _dapper.GetAll<QaCategoriesOut>("[dbo].[GetQaCategories]", spParams, cancellationToken, System.Data.CommandType.StoredProcedure);

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetQaCategoriesOut("Get qa categories successful.", result));
    }
}


