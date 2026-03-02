using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.DeleteCustomerGuest;
public record DeleteCustomerGuestRequest(DeleteCustomerGuestIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerGuestHandler : IRequestHandler<DeleteCustomerGuestRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public DeleteCustomerGuestHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerGuestRequest request, CancellationToken cancellationToken)
    {
        var customerGuest = await _db.CustomerGuests.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerGuest == null)
        {
            return _response.Error($"Customer guest could not be found.", AppStatusCodeError.Gone410);
        }

        //customerGuest.DeletedAt = DateTime.UtcNow;
        //_db.CustomerGuests.Update(customerGuest);
        _db.CustomerGuests.Remove(customerGuest);

        var channlesofCustomerGuests = await _db.Channels.Where(x => _db.ChannelUserTypeCustomerGuest.Where(e => e.UserId == customerGuest.Id).Select(e => e.ChannelId).Contains(x.Id)).ToListAsync(cancellationToken);
        _db.Channels.RemoveRange(channlesofCustomerGuests);

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteCustomerGuestOut("Delete customer guest successful.", new() { Id = request.In.Id }));
    }
}
