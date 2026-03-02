using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerStaffAlerts.Queries.GetCustomerStaffAlertsByCustomerId;
public record GetCustomerStaffAlertsByCustomerIdRequest(string CustomerId) : IRequest<AppHandlerResponse>;
public class GetCustomerStaffAlertsByCustomerIdHandler : IRequestHandler<GetCustomerStaffAlertsByCustomerIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerStaffAlertsByCustomerIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerStaffAlertsByCustomerIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.CustomerId, DbType.Int32);

        //SP Name is GetCustomerStaffAlertsByCustomerId
        var customerStaffAlertsOut = await _dapper
            .GetAll<CustomerStaffAlertsByCustomerIdOut>("[dbo].[GetCustomerStaffAlertsByCustomerId]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customerStaffAlertsOut == null || customerStaffAlertsOut.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerStaffAlertsByCustomerIdOut("Get customer staff alert successful.", customerStaffAlertsOut));
    }
}
