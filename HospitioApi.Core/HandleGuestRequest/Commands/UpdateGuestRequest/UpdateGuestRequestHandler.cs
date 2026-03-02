using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGuestRequest.Commands.UpdateGuestRequest;

public record UpdateGuestRequestRequest(UpdateGuestRequestIn In) : IRequest<AppHandlerResponse>;

public class UpdateGuestRequestHandler : IRequestHandler<UpdateGuestRequestRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateGuestRequestHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateGuestRequestRequest request, CancellationToken cancellationToken)
    {
        var guestRequest = await _db.GuestRequests.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (guestRequest == null)
        {
            return _response.Error($"Guest request with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        guestRequest.Status = (byte)request.In.Status;
        guestRequest.IsActive = request.In.IsActive;
        guestRequest.UpdateAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateGuestRequestOut("Update guest request successful."));
    }
}
