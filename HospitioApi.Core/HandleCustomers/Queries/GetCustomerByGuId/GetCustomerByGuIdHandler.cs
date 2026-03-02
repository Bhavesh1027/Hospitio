using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomers.Queries.GetCustomerByGuId;
public record GetCustomerByGuIdForHospitioRequest(GetCustomerByGuIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerByGuIdHandler : IRequestHandler<GetCustomerByGuIdForHospitioRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerByGuIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerByGuIdForHospitioRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("GuId", request.In.GuId, DbType.Guid);

        var customer = await _dapper.GetSingle<CustomerByGuIdOut>("[dbo].[GetCustomerByGuId]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customer == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerByGuIdOut("Get customer successful.", customer));
    }
}
