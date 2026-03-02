using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerRoomService.Commands.DeleteCustomerRoomService;
public record DeleteCustomerRoomServiceRequest(DeleteCustomerRoomServiceIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerRoomServicesHandler : IRequestHandler<DeleteCustomerRoomServiceRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;

    public DeleteCustomerRoomServicesHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerRoomServiceRequest request, CancellationToken cancellationToken)
    {
        var customerRoomService = await _db.CustomerGuestAppRoomServiceCategories.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerRoomService == null)
        {
            return _response.Error($"Customers room service with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        //_commonRepository.CustomersRoomServiceDelete(customerRoomService, _db, cancellationToken);
        _db.CustomerGuestAppRoomServiceCategories.Remove(customerRoomService);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteCustomerRoomServiceOut("Delete customers room service successful.", new() { Id = customerRoomService.Id }));
    }
}
