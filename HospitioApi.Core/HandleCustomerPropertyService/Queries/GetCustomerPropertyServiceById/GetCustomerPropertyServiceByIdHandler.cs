using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerPropertyService.Queries.GetCustomerPropertyServiceById;
public record GetCustomerPropertyServiceByIdRequest(GetCustomerPropertyServiceByIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerPropertyServiceByIdHandler : IRequestHandler<GetCustomerPropertyServiceByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerPropertyServiceByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerPropertyServiceByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var getCustomerPropertyServiceByIdOuts = await _dapper.GetSingle<CustomerPropertyServiceByd>("[dbo].[GetCustomerPropertyServiceById]"
      , spParams, cancellationToken,
      commandType: CommandType.StoredProcedure);

        if (getCustomerPropertyServiceByIdOuts == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerPropertyServiceByIdOut("Get customer property service successful.", getCustomerPropertyServiceByIdOuts));
    }
}
