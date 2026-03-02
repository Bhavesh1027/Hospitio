using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAdminCustomerAlerts.Queries.GetAdminCustomerAlerts;
public record GetAdminCustomerAlertsRequest() : IRequest<AppHandlerResponse>;
public class GetAdminCustomerAlertsHandler : IRequestHandler<GetAdminCustomerAlertsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetAdminCustomerAlertsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetAdminCustomerAlertsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();

        var result = await _dapper
            .GetAll<AdminCustomerAlertsOut>("[dbo].[GetAdminCustomerAlerts]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetAdminCustomerAlertsOut("Get admin customer alerts successful.", result));
    }
}
