using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerGuest.Queries.GetMainGuestByReservationId;
public record GetCustomerGuestByReservationIdRequest(GetMainGuestByReservationIdIn In) : IRequest<AppHandlerResponse>;
public class GetMainGuestByReservationIdHandler : IRequestHandler<GetCustomerGuestByReservationIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetMainGuestByReservationIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerGuestByReservationIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("ReservationId", request.In.ReservationId, DbType.Int32);

        var customerGuestByIdOut = await _dapper
            .GetSingle<CustomerGuestByReservationIdOut>("[dbo].[GetMainCustomerGuestByReservationId]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customerGuestByIdOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetMainGuestByReservationIdOut("Get customer guest successful.", customerGuestByIdOut));
    }
}
