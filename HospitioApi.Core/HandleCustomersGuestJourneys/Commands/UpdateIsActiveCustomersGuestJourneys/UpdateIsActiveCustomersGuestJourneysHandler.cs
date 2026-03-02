using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersGuestJourneys.Commands.UpdateIsActiveCustomersGuestJourneys;
public record UpdateIsActiveCustomersGuestJourneysRequest(UpdateIsActiveCustomersGuestJourneyIn In) : IRequest<AppHandlerResponse>;
public class UpdateIsActiveCustomersGuestJourneysHandler : IRequestHandler<UpdateIsActiveCustomersGuestJourneysRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateIsActiveCustomersGuestJourneysHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(UpdateIsActiveCustomersGuestJourneysRequest request, CancellationToken cancellationToken)
    {
        var updateIsActiveCustomersGuestJourneys = await _db.CustomerGuestJournies.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (updateIsActiveCustomersGuestJourneys == null)
        {
            return _response.Error($"Customers guest journeys with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        updateIsActiveCustomersGuestJourneys.IsActive = request.In.IsActive;

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateIsActiveCustomersGuestJourneysOut($"Guest journey with {request.In.Id} has been updated. IsActive has been set to {request.In.IsActive}.", new()
        {
            Id = request.In.Id,
            IsActive = request.In.IsActive
        }));
    }
}
