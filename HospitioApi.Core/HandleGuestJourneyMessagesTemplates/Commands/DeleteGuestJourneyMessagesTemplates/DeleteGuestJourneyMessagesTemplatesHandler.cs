using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.DeleteGuestJourneyMessagesTemplates;
public record DeleteGuestJourneyMessagesTemplatesRequest(DeleteGuestJourneyMessagesTemplatesIn In) : IRequest<AppHandlerResponse>;
public class DeleteGuestJourneyMessagesTemplatesHandler : IRequestHandler<DeleteGuestJourneyMessagesTemplatesRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IVonageService _vonageService;
    private readonly VonageSettingsOptions _vonageSettingsOptions;
    public DeleteGuestJourneyMessagesTemplatesHandler(ApplicationDbContext db, IHandlerResponseFactory response,IVonageService vonageService,IOptions<VonageSettingsOptions> vonageSettingsOptions)
    {
        _db = db;
        _response = response;
        _vonageService = vonageService;
        _vonageSettingsOptions = vonageSettingsOptions.Value;
    }
    public async Task<AppHandlerResponse> Handle(DeleteGuestJourneyMessagesTemplatesRequest request, CancellationToken cancellationToken)
    {
        var deleteGuestJourneyMessagesTemplate = await _db.GuestJourneyMessagesTemplates.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (deleteGuestJourneyMessagesTemplate == null)
        {
            return _response.Error($"GuestJourneyMessagesTemplate with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }
        _db.GuestJourneyMessagesTemplates.Remove(deleteGuestJourneyMessagesTemplate);
        await _db.SaveChangesAsync(cancellationToken);

        if (deleteGuestJourneyMessagesTemplate != null && deleteGuestJourneyMessagesTemplate.WhatsappTemplateName != null && deleteGuestJourneyMessagesTemplate.VonageTemplateId != null)
        {
            var response = await _vonageService.RemoveWhatsappTemplate(_vonageSettingsOptions.AppId, _vonageSettingsOptions.AppPrivatKey, _vonageSettingsOptions.WABAId, deleteGuestJourneyMessagesTemplate.WhatsappTemplateName, deleteGuestJourneyMessagesTemplate.VonageTemplateId);
        }

        return _response.Success(new DeleteGuestJourneyMessagesTemplatesOut("Delete GuestJourneyMessagesTemplate successful.", new() { Id = deleteGuestJourneyMessagesTemplate.Id }));

    }
}
