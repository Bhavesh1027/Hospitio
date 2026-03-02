using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.DeleteCustomerEnhanceYourStayItem;
public record DeleteCustomerEnhanceYourStayItemRequest(DeleteCustomerEnhanceYourStayItemIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerEnhanceYourStayItemHandler : IRequestHandler<DeleteCustomerEnhanceYourStayItemRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteCustomerEnhanceYourStayItemHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerEnhanceYourStayItemRequest request, CancellationToken cancellationToken)
    {

        var CustomerEnhanceYourStayItem = await _db.CustomerGuestAppEnhanceYourStayItems.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (CustomerEnhanceYourStayItem == null)
        {
            return _response.Error($"Customers enhance your stay  item with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        var customerEnhanceYourStayItemExtraList = await _db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.Where(c => c.CustomerGuestAppEnhanceYourStayItemId == request.In.Id).ToListAsync(cancellationToken);

        if (customerEnhanceYourStayItemExtraList.Count > 0)
        {
            _db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.RemoveRange(customerEnhanceYourStayItemExtraList);
        }

        var itemImages = await _db.CustomerGuestAppEnhanceYourStayItemsImages.Where(c => c.CustomerGuestAppEnhanceYourStayItemId == request.In.Id).ToListAsync(cancellationToken);

        if (itemImages.Count>0)
        {
            while (itemImages.Count > 0)
            {
                var oldAttachments = itemImages.Last();
                itemImages.Remove(oldAttachments);
                _db.CustomerGuestAppEnhanceYourStayItemsImages.Remove(oldAttachments);
            }
        }

        _db.CustomerGuestAppEnhanceYourStayItems.Remove(CustomerEnhanceYourStayItem);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteCustomerEnhanceYourStayItemOut("Delete customers enhance your stay  item successful.", new() { Id = CustomerEnhanceYourStayItem.Id }));
    }
}
