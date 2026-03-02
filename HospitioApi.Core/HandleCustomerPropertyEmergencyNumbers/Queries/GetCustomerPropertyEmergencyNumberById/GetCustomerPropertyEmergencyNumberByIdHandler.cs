using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Queries.GetCustomerPropertyEmergencyNumberById;
public record GetCustomerPropertyEmergencyNumberByIdRequest(GetCustomerPropertyEmergencyNumberByIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerPropertyEmergencyNumberByIdHandler : IRequestHandler<GetCustomerPropertyEmergencyNumberByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomerPropertyEmergencyNumberByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerPropertyEmergencyNumberByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var customerPropertyEmergencyNumber = await _dapper
            .GetSingle<CustomerPropertyEmergencyNumberByIdOut>("[dbo].[GetCustomerPropertyEmergencyNumberById]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (customerPropertyEmergencyNumber == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerPropertyEmergencyNumberByIdOut("Get customer property emergency number successful.", customerPropertyEmergencyNumber));
    }
}
