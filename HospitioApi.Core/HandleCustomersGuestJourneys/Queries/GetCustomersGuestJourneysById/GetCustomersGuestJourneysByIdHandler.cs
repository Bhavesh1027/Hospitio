using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Queries.GetCustomersGuestJourneysById;
public record GetCustomersGuestJourneysByIdRequest(GetCustomersGuestJourneysByIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomersGuestJourneysByIdHandler : IRequestHandler<GetCustomersGuestJourneysByIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;

    public GetCustomersGuestJourneysByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomersGuestJourneysByIdRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var result = await _dapper.GetSingle<CustomersGuestJourneysByIdOut>("[dbo].[GetCustomersGuestJourneysById]"
     , spParams, cancellationToken,
     commandType: CommandType.StoredProcedure);

        if (result == null)
        {
            return _response.Error("Customers guest journey could not be found", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomersGuestJourneysByIdOut("Get customers guest journey successful.", result));
    }
}
