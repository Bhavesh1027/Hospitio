using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleGuestRequest.Queries.GetGuestRequests;

public record GetGuestRequestsRequest(GetGuestRequestsIn In, int CustomerId) : IRequest<AppHandlerResponse>;

public class GetGuestRequestsHandler : IRequestHandler<GetGuestRequestsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetGuestRequestsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetGuestRequestsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.CustomerId, DbType.Int32);
        spParams.Add("SortColumn", request.In.SortColumn, DbType.String);
        spParams.Add("SortOrder", request.In.SortOrder, DbType.String);
        spParams.Add("PageNo", request.In.PageNo, DbType.Int32);
        spParams.Add("PageSize", request.In.PageSize, DbType.Int32);
        
        var result = await _dapper
            .GetAll<GuestRequestsOut>("[dbo].[GetGuestRequests_v1]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetGuestRequestsOut("Get guest requests successful.", result));
    }
}
