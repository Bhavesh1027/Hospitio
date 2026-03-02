using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerRoomNames.Queries.GetCustomerRoomByCustomerRoomGuId;
public record GetCustomerRoomNameByGuIdRequest(GetCustomerRoomByCustomerRoomGuIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerRoomByCustomerRoomGuIdHandler : IRequestHandler<GetCustomerRoomNameByGuIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerRoomByCustomerRoomGuIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerRoomNameByGuIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("GuId", request.In.guid, DbType.Guid);
        spParams.Add("CustomerId", request.In.CustomerId, DbType.Int32);

        var customerRooms = await _dapper
            .GetSingle<CustomerRooms>("[dbo].[GetCustomerRoomByCustomerRoomGuId]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customerRooms == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerRoomByCustomerRoomGuIdOut("Get customer room name successful.", customerRooms));
    }
}
