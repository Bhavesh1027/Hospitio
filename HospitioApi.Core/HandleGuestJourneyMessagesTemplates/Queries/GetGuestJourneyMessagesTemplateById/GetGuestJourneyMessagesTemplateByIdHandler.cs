using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Queries.GetGuestJourneyMessagesTemplateById;
public record GetGuestJourneyMessagesTemplateByIdReuest(GetGuestJourneyMessagesTemplateByIdIn In) : IRequest<AppHandlerResponse>;
public class GetGuestJourneyMessagesTemplateByIdHandler : IRequestHandler<GetGuestJourneyMessagesTemplateByIdReuest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetGuestJourneyMessagesTemplateByIdHandler(IDapperRepository dapper, IHandlerResponseFactory response)
    {
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetGuestJourneyMessagesTemplateByIdReuest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("Id", request.In.Id, DbType.Int32);

        var guestJourneyMessagesTemplateByIdOut = await _dapper
            .GetSingle<GuestJourneyMessagesTemplateByIdOut>("[dbo].[GetGuestJourneyMessagesTemplatesById]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

        if (guestJourneyMessagesTemplateByIdOut == null)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetGuestJourneyMessagesTemplateByIdOut("Get GuestJourneyMessagesTemplate successful.", guestJourneyMessagesTemplateByIdOut));
    }
}
