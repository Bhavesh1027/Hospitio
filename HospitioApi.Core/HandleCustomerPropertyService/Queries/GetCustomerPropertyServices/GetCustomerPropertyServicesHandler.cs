using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerPropertyService.Queries.GetCustomerPropertyServices;
public record GetCustomerPropertyServicesRequest(GetCustomerPropertyServicesIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerPropertyServicesHandler : IRequestHandler<GetCustomerPropertyServicesRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerPropertyServicesHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerPropertyServicesRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerPropertyInformationId", request.In.CustomerPropertyInformationId, DbType.Int32);

        var result = await _dapper
            .GetAllJsonData<CustomerPropertyServicesOut>("[dbo].[GetCustomerPropertyServicesForAPPBuilder]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (result == null || result.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomerPropertyServicesOut("Get customer property services successful.", result));
    }
}
