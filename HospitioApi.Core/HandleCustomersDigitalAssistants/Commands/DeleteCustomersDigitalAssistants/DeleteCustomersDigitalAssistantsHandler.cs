using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.DeleteCustomersDigitalAssistants;
public record DeleteCustomersDigitalAssistantsRequest(DeleteCustomersDigitalAssistantsIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomersDigitalAssistantsHandler : IRequestHandler<DeleteCustomersDigitalAssistantsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteCustomersDigitalAssistantsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomersDigitalAssistantsRequest request, CancellationToken cancellationToken)
    {
        var customersPaymentProcessors = await _db.CustomerDigitalAssistants.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customersPaymentProcessors == null)
        {
            return _response.Error($"Customer digital assistant with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }
        _db.CustomerDigitalAssistants.Remove(customersPaymentProcessors);
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new DeleteCustomersDigitalAssistantsOut("Delete Customer Digital Assistant successful.", new() { Id = customersPaymentProcessors.Id }));
    }
}
