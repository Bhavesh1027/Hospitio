using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomersConcierge.Commands.DeleteCustomerConciergeItem;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Threading;

namespace HospitioApi.Core.HandleCustomerReception.Commands.DeleteCustomerReception;
public record DeleteCustomerReceptionRequest(DeleteCustomerReceptionIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerReceptionHandler : IRequestHandler<DeleteCustomerReceptionRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;

    public DeleteCustomerReceptionHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerReceptionRequest request, CancellationToken cancellationToken)
    {
        var CustomerReception = await _db.CustomerGuestAppReceptionCategories.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (CustomerReception == null)
        {
            return _response.Error($"Customers reception with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }
        _db.CustomerGuestAppReceptionCategories.Remove(CustomerReception);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteCustomerReceptionOut("Delete customers reception successful.", new() { Id = CustomerReception.Id }));
    }
}
