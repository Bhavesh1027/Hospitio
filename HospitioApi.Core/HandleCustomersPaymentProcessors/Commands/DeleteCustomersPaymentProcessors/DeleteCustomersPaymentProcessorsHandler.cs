using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Linq.Dynamic.Core;

namespace HospitioApi.Core.HandleCustomersPaymentProcessors.Commands.DeleteCustomersPaymentProcessors;
public record DeleteCustomersPaymentProcessorsRequest(DeleteCustomersPaymentProcessorsIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomersPaymentProcessorsHandler : IRequestHandler<DeleteCustomersPaymentProcessorsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteCustomersPaymentProcessorsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomersPaymentProcessorsRequest request, CancellationToken cancellationToken)
    {
        var customersPaymentProcessors = await _db.CustomerPaymentProcessors.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customersPaymentProcessors == null)
        {
            return _response.Error($"Customers payment pocessors with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        _db.CustomerPaymentProcessors.Remove(customersPaymentProcessors);
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new DeleteCustomersPaymentProcessorsOut("Delete customers payment processors successful.", new() { Id = customersPaymentProcessors.Id }));
    }
}
