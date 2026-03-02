using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerRoomNames.Queries.GetCustomerRoomByGuIds;
public record GetCustomerRoomNamesByGuIdRequest(GetCustomerRoomByGuIdsIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerRoomByGuIdsHandler : IRequestHandler<GetCustomerRoomNamesByGuIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerRoomByGuIdsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerRoomNamesByGuIdRequest request, CancellationToken cancellationToken)
    {
        var guids = request.In.guids;
        var result = new List<CustomerRooms>();
        foreach (var guid in guids)
        {
            var spParams = new DynamicParameters();
            spParams.Add("Guid", guid, DbType.Guid);

            var customerRooms = await _dapper.GetSingle<CustomerRooms>("[dbo].[GetCustomerRoomNamesByGuId]", spParams, cancellationToken, CommandType.StoredProcedure);

            if (customerRooms != null)
            {
                result.Add(customerRooms);
            }
        }

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerRoomByGuIdsOut("Get customer room name successful.", result));
    }
}
