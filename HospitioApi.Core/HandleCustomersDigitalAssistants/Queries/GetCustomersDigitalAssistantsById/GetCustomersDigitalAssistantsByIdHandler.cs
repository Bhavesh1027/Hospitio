using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Queries.GetCustomersDigitalAssistantsById;
public record GetCustomersDigitalAssistantsByIdRequest(GetCustomersDigitalAssistantsByIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomersDigitalAssistantsByIdHandler : IRequestHandler<GetCustomersDigitalAssistantsByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomersDigitalAssistantsByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomersDigitalAssistantsByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var result = await _dapper.GetSingle<CustomersDigitalAssistantsByIdOut>("[dbo].[GetDigitalAssistantsById]"
     , spParams, cancellationToken,
     commandType: CommandType.StoredProcedure);

        if (result == null)
        {
            return _response.Error("Customers digital assistant could not be found", AppStatusCodeError.Gone410);
        }

        var customersDigitalAssistantsByIdOut = new CustomersDigitalAssistantsByIdOut
        {
            Id = result.Id,
            CustomerId = result.CustomerId,
            Name = result.Name,
            Details = result.Details,
            Icon = result.Icon
        };

        return _response.Success(new GetCustomersDigitalAssistantsByIdOut("Get customers digital assistant successful.", customersDigitalAssistantsByIdOut));
    }
}
