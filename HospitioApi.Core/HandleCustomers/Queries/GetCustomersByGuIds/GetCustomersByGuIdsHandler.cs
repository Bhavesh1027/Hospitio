using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomersByGuIds;
public record GetCustomersByGuIdsRequest(GetCustomersByGuIdsIn In) : IRequest<AppHandlerResponse>;
public class GetCustomersByGuIdsHandler : IRequestHandler<GetCustomersByGuIdsRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomersByGuIdsHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomersByGuIdsRequest request, CancellationToken cancellationToken)
    {
        var guids = request.In.guids;
        var result = new List<Customers>();
        foreach (var guid in guids)
        {
            var spParams = new DynamicParameters();
            spParams.Add("Guid", guid, DbType.Guid);

            var customerRooms = await _dapper.GetSingle<Customers>("[dbo].[GetCustomersByGuIds]", spParams, cancellationToken, CommandType.StoredProcedure);

            if (customerRooms != null)
            {
                result.Add(customerRooms);
            }
        }

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomersByGuIdsOut("Get customer successful.", result));
    }
}
