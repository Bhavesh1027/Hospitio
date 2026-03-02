using Dapper;
using MediatR;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Queries.GetCustomerPropertyEmergencyNumberById;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Queries.GetCustomerPropertyEmergencyNumbers;
public record GetCustomerPropertyEmergencyNumbersRequest(GetCustomerPropertyEmergencyNumbersIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerPropertyEmergencyNumbersHandler : IRequestHandler<GetCustomerPropertyEmergencyNumbersRequest,AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerPropertyEmergencyNumbersHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
            _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerPropertyEmergencyNumbersRequest request ,CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("PropertyId", request.In.PropertyId, DbType.Int32);

        var customerPropertyEmergencyNumbers = await _dapper
            .GetAll<CustomerPropertyEmergencyNumbersOut>("[dbo].[GetCustomerPropertyEmergencyNumbers]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customerPropertyEmergencyNumbers == null || customerPropertyEmergencyNumbers.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerPropertyEmergencyNumbersOut("Get customer property emergency number successful.", customerPropertyEmergencyNumbers));
    }
}
