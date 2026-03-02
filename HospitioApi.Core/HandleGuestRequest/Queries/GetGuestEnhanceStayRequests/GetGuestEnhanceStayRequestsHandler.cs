using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleGuestRequest.Queries.GetGuestEnhanceStayRequests;
public record GetGuestEnhanceStayRequestsRequest(GetGuestEnhanceStayRequestsIn In) : IRequest<AppHandlerResponse>;
public class GetGuestEnhanceStayRequestsHandler : IRequestHandler<GetGuestEnhanceStayRequestsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetGuestEnhanceStayRequestsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetGuestEnhanceStayRequestsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.In.CustomerId, DbType.Int32);
        spParams.Add("GuestId", request.In.GuestId, DbType.Int32);

        var result = await _dapper
            .GetAll<GuestRequestsOut>("[dbo].[GetGuestWithEnhanceStayRequests]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetGuestEnhanceStayRequestsOut("Get guest requests successful.", result));
    }
}
