using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Queries.GetCustomerStaffAlertsById;
public record GetCustomerStaffAlertsByIdRequest(GetCustomerStaffAlertsByIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerStaffAlertsByIdHandler : IRequestHandler<GetCustomerStaffAlertsByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerStaffAlertsByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerStaffAlertsByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        //SP Name is GetCustomerStaffAlertsById
        var customerStaffAlertsOut = await _dapper
            .GetSingle<CustomerStaffAlertsByIdOut>("[dbo].[GetCustomerStaffAlertsById]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customerStaffAlertsOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerStaffAlertsByIdOut("Get customer staff alert successful.", customerStaffAlertsOut));
    }
}
