using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplates;
public record GetGuestJourneyMessagesTemplatesRequest() : IRequest<AppHandlerResponse>;
public class GetGuestJourneyMessagesTemplatesHandler : IRequestHandler<GetGuestJourneyMessagesTemplatesRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetGuestJourneyMessagesTemplatesHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetGuestJourneyMessagesTemplatesRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();

        var result = await _dapper
            .GetAll<GuestJourneyMessagesTemplatesOut>("[dbo].[GetGuestJourneyMessagesTemplates]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

        if (result == null || result.Count == 0) 
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }
        return _response.Success(new GetGuestJourneyMessagesTemplatesOut("Get guest journey messages templates successful.", result));
    }
}
