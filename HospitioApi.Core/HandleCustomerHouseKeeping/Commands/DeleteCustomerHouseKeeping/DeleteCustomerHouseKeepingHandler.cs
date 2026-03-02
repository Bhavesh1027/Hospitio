using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.DeleteCustomerHouseKeeping;
public record DeleteCustomerHouseKeepingRequest(DeleteCustomerHouseKeepingIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerHouseKeepingHandler : IRequestHandler<DeleteCustomerHouseKeepingRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;

    public DeleteCustomerHouseKeepingHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerHouseKeepingRequest request, CancellationToken cancellationToken)
    {
        var customerHouseKeeping = await _db.CustomerGuestAppHousekeepingCategories.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerHouseKeeping == null)
        {
            return _response.Error($"Customers house keeping with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }
       // _commonRepository.CustomersHouseKeepingDelete(customerHouseKeeping, _db, cancellationToken);
        _db.CustomerGuestAppHousekeepingCategories.Remove(customerHouseKeeping);
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new DeleteCustomerHouseKeepingOut("Delete customers house keeping successful.", new() { Id = customerHouseKeeping.Id }));
    }
}
