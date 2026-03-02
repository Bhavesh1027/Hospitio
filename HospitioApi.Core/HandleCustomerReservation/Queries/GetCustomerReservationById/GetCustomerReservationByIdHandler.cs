using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerReservation.Queries.GetCustomerReservationById;
public record GetCustomerReservationByIdRequest(GetCustomerReservationByIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerReservationByIdHandler : IRequestHandler<GetCustomerReservationByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerReservationByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerReservationByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var customerReservationByIdOut = await _dapper
            .GetSingle<CustomerReservationByIdOut>("[dbo].[GetCustomerReservationById]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customerReservationByIdOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerReservationByIdOut("Get customer reservation successful.", customerReservationByIdOut));
    }
}
