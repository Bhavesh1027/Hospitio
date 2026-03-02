using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomers.Commands.UpdateCustomerProduct;
public record UpdateCustomerProductRequest(UpdateCustomerProductIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomerProductHandler : IRequestHandler<UpdateCustomerProductRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public UpdateCustomerProductHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomerProductRequest request, CancellationToken cancellationToken)
    {
        var customer = new Customer();
        customer = await _db.Customers.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);

        if (customer == null)
        {
            return _response.Error($"Customer with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        customer.ProductId = request.In.ProductId;
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateCustomerProductOut("Customer product update successful.", new() { CustomerId = customer.Id }));
    }
}
