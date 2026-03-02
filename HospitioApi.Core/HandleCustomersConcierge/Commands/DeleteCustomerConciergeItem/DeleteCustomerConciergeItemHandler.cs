using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.HandleCustomersConcierge.Commands.DeleteCustomerConcierge;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomersConcierge.Commands.DeleteCustomerConciergeItem;
public record DeleteCustomerConciergeItemRequest(DeleteCustomerConciergeItemIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerConciergeItemHandler : IRequestHandler<DeleteCustomerConciergeItemRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteCustomerConciergeItemHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerConciergeItemRequest request, CancellationToken cancellationToken)
    {
        var CustomerConciergeItem = await _db.CustomerGuestAppConciergeItems.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (CustomerConciergeItem == null)
        {
            return _response.Error($"Customers concierge item with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }
        if(CustomerConciergeItem.JsonData != null)
        {
            var result = System.Text.Json.JsonSerializer.Deserialize<DeleteCustomerConciergeItem>(CustomerConciergeItem.JsonData);

            result.IsDeleted = true;
            CustomerConciergeItem.JsonData = JsonConvert.SerializeObject(result);
        }
        else
        {
        _db.CustomerGuestAppConciergeItems.Remove(CustomerConciergeItem);
        }
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteCustomerConciergeItemOut("Delete customers concierge item successful.", new() { Id = CustomerConciergeItem.Id }));
    }
}
