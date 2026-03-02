using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleAdminStaffAlerts.Queries.GetAdminStaffAlerts;
public record GetAdminStaffAlertsRequest() : IRequest<AppHandlerResponse>;
public class GetAdminStaffAlertsHandler : IRequestHandler<GetAdminStaffAlertsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetAdminStaffAlertsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetAdminStaffAlertsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();

        var adminStaffAlertsOut = await _dapper
            .GetAll<AdminStaffAlertsOut>("[dbo].[GetAdminStaffAlerts]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (adminStaffAlertsOut == null || adminStaffAlertsOut.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetAdminStaffAlertsOut("Get admin staff alert successful.", adminStaffAlertsOut));
    }
}
