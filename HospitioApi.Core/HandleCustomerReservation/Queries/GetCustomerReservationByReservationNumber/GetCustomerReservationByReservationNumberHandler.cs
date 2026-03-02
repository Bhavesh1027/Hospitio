using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationByReservationNumber;
public record GetCustomerReservationByNumberRequest(GetCustomerReservationByReservationNumberIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerReservationByReservationNumberHandler : IRequestHandler<GetCustomerReservationByNumberRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerReservationByReservationNumberHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerReservationByNumberRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("ReservationNumber", request.In.ReservationNumber, DbType.String);

        var customerReservationByNumberOut = await _dapper
            .GetSingle<CustomerReservationByNumberOut>("[dbo].[GetCustomerReservationByNumber]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customerReservationByNumberOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerReservationByReservationNumberOut("Get customer reservation successful.", customerReservationByNumberOut));
    }
}
