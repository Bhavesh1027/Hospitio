using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Commands.DeleteCustomersGuestJourneys;
public record DeleteCustomersGuestJourneysRequest(DeleteCustomersGuestJourneysIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomersGuestJourneysHandler : IRequestHandler<DeleteCustomersGuestJourneysRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IVonageService _vonageService;

    public DeleteCustomersGuestJourneysHandler(ApplicationDbContext db, IHandlerResponseFactory response, IVonageService vonageService)
    {
        _db = db;
        _response = response;
        _vonageService = vonageService;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomersGuestJourneysRequest request, CancellationToken cancellationToken)
    {
        var customersGuestJourneys = await _db.CustomerGuestJournies.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customersGuestJourneys == null)
        {
            return _response.Error($"Customers guest journey with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }
        _db.CustomerGuestJournies.Remove(customersGuestJourneys);
        await _db.SaveChangesAsync(cancellationToken);

        var vonageCred = await _db.VonageCredentials.Where(x => x.CustomerId == customersGuestJourneys.CutomerId).FirstOrDefaultAsync(cancellationToken);

        if (vonageCred != null && vonageCred.AppId != null && vonageCred.AppPrivatKey != null && vonageCred.WABAId != null && customersGuestJourneys.WhatsappTemplateName != null && customersGuestJourneys.VonageTemplateId != null)
        {
            await _vonageService.RemoveWhatsappTemplate(vonageCred.AppId, vonageCred.AppPrivatKey, vonageCred.WABAId, customersGuestJourneys.WhatsappTemplateName, customersGuestJourneys.VonageTemplateId);
        }

        return _response.Success(new DeleteCustomersGuestJourneysOut("Delete customers guest journey successful.", new() { Id = customersGuestJourneys.Id }));
    }
}
