using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplates;
public record GetGuestJourneyMessagesTemplatesForCustomerRequest() : IRequest<AppHandlerResponse>;
public class GetGuestJourneyMessagesTemplatesForCustomerHandler : IRequestHandler<GetGuestJourneyMessagesTemplatesForCustomerRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetGuestJourneyMessagesTemplatesForCustomerHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetGuestJourneyMessagesTemplatesForCustomerRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();

        var result = await _dapper
            .GetAll<GuestJourneyMessagesTemplatesForCustomerOut>("[dbo].[GetGuestJourneyMessagesTemplatesForCustomer]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (result == null || result.Count == 0) 
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetGuestJourneyMessagesTemplatesForCustomerOut("Get guest journey messages templates successful.", result));
    }
}
