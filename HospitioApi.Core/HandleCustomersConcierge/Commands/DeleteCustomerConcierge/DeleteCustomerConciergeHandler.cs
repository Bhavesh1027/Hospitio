using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.HandleCustomersConcierge.Commands.DisplayOrderCustomerConcierage;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersConcierge.Commands.DeleteCustomerConcierge;
public record DeleteCustomerConciergeRequest(DeleteCustomerConciergeIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerConciergeHandler : IRequestHandler<DeleteCustomerConciergeRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteCustomerConciergeHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerConciergeRequest request, CancellationToken cancellationToken)
    {
        var CustomerConcierge = await _db.CustomerGuestAppConciergeCategories.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (CustomerConcierge == null)
        {
            return _response.Error($"Customers concierge with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        if (CustomerConcierge.JsonData != null)
        {
            var result = System.Text.Json.JsonSerializer.Deserialize<DeleteCustomerConciergeJsonOut>(CustomerConcierge.JsonData);

            result.IsDeleted = true;
            CustomerConcierge.JsonData = JsonConvert.SerializeObject(result);
        }
        else
        {
            _db.CustomerGuestAppConciergeCategories.Remove(CustomerConcierge);
        }
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteCustomerConciergeOut("Delete customers concierge successful.", new() { Id = CustomerConcierge.Id }));
    }
}
