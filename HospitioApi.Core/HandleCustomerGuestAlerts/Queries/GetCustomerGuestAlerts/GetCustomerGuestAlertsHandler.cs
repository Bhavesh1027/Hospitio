using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerGuestAlerts.Queries.GetCustomerGuestAlerts;
public record GetCustomerGuestAlertsRequest(string CustomerId) : IRequest<AppHandlerResponse>;
public class GetCustomerGuestAlertsHandler : IRequestHandler<GetCustomerGuestAlertsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerGuestAlertsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerGuestAlertsRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.CustomerId, System.Data.DbType.Int32);
        // SP Name GetCustomerGuestAlerts
        var result = await _dapper
            .GetAll<CustomerGuestAlertsOut>("[dbo].[GetCustomerGuestAlerts]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerGuestAlertsOut("Get customer guest alerts successful.", result));
    }
}
