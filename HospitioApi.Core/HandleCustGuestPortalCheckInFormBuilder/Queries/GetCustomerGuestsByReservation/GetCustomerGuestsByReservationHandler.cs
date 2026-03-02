using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Queries.GetCustomerGuestsByReservation;
public record GetCustomerGuestsByReservationRequest(GetCustomerGuestsByReservationIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerGuestsByReservationHandler: IRequestHandler<GetCustomerGuestsByReservationRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerGuestsByReservationHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerGuestsByReservationRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("ReservationId", request.In.ReservationId, DbType.Int32);

        var result = await _dapper
            .GetAll<CustomerGuestsByReservationOut>("[dbo].[GetCustomerGuestsByReservation]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerGuestsByReservationOut("Get customer guests successful.", result));
    }
}
