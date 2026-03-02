using Dapper;
using MediatR;
using HospitioApi.Shared;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandlerCustomerCommunication.Queries.GetCustomerCommunicationByReservationId;
public record GetCustomerCommunicationByReservationIdRequest(GetCustomerCommunicationByReservationIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerCommunicationByReservationIdHandler : IRequestHandler<GetCustomerCommunicationByReservationIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerCommunicationByReservationIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerCommunicationByReservationIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, System.Data.DbType.Int32);
        spParams.Add("ReservationId", request.In.ReservationId, System.Data.DbType.Int32);

        var customerByReservationId = await _dapper
            .GetSingle<GetCustomerCommunicationByReservationIdResponseOut>("[dbo].[SP_Get_CustomerCommunicationByReservationId]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (customerByReservationId == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerCommunicationByReservationOut("Get customers guest reservation successfully.", customerByReservationId));
    }
}
