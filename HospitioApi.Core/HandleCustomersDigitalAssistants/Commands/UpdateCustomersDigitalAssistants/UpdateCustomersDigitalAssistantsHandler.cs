using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersDigitalAssistants.Commands.UpdateCustomersDigitalAssistants;
public record UpdateCustomersDigitalAssistantsRequest(UpdateCustomersDigitalAssistantsIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomersDigitalAssistantsHandler : IRequestHandler<UpdateCustomersDigitalAssistantsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateCustomersDigitalAssistantsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomersDigitalAssistantsRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.CustomerDigitalAssistants.Where(e => e.Name == request.In.Name && e.Id != request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The digital assistant {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        var updateCustomersDigitalAssistants = await _db.CustomerDigitalAssistants.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (updateCustomersDigitalAssistants == null)
        {
            return _response.Error($"Customers digital assistants with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        updateCustomersDigitalAssistants.Name = request.In.Name;
        updateCustomersDigitalAssistants.Details = request.In.Details;
        updateCustomersDigitalAssistants.Icon = request.In.Icon;

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateCustomersDigitalAssistantsOut("Update customers digital assistants", new()
        {
            Id = request.In.Id,
            Name = request.In.Name,
            Details = request.In.Details,
            Icon = request.In.Icon
        }));
    }
}
