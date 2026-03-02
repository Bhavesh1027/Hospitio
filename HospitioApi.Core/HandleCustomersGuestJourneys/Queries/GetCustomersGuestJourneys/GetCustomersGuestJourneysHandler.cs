using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Queries.GetCustomersGuestJourneys;
public record GetCustomersGuestJourneysRequest(string CustomerId) : IRequest<AppHandlerResponse>;
public class GetCustomersGuestJourneysHandler : IRequestHandler<GetCustomersGuestJourneysRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomersGuestJourneysHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomersGuestJourneysRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
       
        spParams.Add("CustomerId", request.CustomerId, DbType.Int32);
        var getCustomersGuestJourneysOuts = await _dapper.GetAll<CustomersGuestJourneysOut>("[dbo].[GetCustomersGuestJourneys]"
      , spParams, cancellationToken,
      commandType: CommandType.StoredProcedure);
        if (getCustomersGuestJourneysOuts == null || getCustomersGuestJourneysOuts.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetCustomersGuestJourneysOut("Get guest journey successful.", getCustomersGuestJourneysOuts));
    }
}
