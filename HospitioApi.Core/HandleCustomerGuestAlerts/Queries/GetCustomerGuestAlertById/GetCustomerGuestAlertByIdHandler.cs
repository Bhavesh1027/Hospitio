using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerGuestAlerts.Queries.GetCustomerGuestAlertById;
public record GetCustomerGuestAlertByIdRequest(GetCustomerGuestAlertByIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerGuestAlertByIdHandler : IRequestHandler<GetCustomerGuestAlertByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerGuestAlertByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerGuestAlertByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var customerGuestAlertByIdOut = await _dapper
            .GetSingle<CustomerGuestAlertByIdOut>("[dbo].[GetCustomerGuestAlertById]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customerGuestAlertByIdOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerGuestAlertByIdOut("Get customer guest alert successful.", customerGuestAlertByIdOut));
    }
}
