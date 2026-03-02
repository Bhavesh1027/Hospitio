using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateIsActiveCustomersDigitalAssistants;
public record UpdateIsActiveCustomersDigitalAssistantsRequest(UpdateIsActiveCustomersDigitalAssistantsIn In) : IRequest<AppHandlerResponse>;
public class UpdateIsActiveCustomersDigitalAssistantsHandler : IRequestHandler<UpdateIsActiveCustomersDigitalAssistantsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateIsActiveCustomersDigitalAssistantsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(UpdateIsActiveCustomersDigitalAssistantsRequest request, CancellationToken cancellationToken)
    {
        var updateIsActiveCustomersDigitalAssistants = await _db.CustomerDigitalAssistants.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (updateIsActiveCustomersDigitalAssistants == null)
        {
            return _response.Error($"Customers digital assistant with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        updateIsActiveCustomersDigitalAssistants.IsActive = request.In.IsActive;

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateIsActiveCustomersDigitalAssistantsOut($"Digital assistant with {request.In.Id} has been updated. IsActive has been set to {request.In.IsActive}.", new()
        {
            Id = request.In.Id,
            IsActive = request.In.IsActive
        }));
    }
}
