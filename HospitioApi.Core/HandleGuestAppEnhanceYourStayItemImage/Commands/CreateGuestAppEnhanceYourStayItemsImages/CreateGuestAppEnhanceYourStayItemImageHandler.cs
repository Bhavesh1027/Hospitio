using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleGuestAppEnhanceYourStayItemImage.Commands.CreateGuestAppEnhanceYourStayItemsImages;
public record CreateGuestAppEnhanceYourStayItemImageRequest(CreateGuestAppEnhanceYourStayItemImageIn In) : IRequest<AppHandlerResponse>;
public class CreateGuestAppEnhanceYourStayItemImageHandler : IRequestHandler<CreateGuestAppEnhanceYourStayItemImageRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public CreateGuestAppEnhanceYourStayItemImageHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateGuestAppEnhanceYourStayItemImageRequest request, CancellationToken cancellationToken)
    {
        var stayImgIn = request.In;
        List<CustomerGuestAppEnhanceYourStayItemsImage> listOfStayItems = new();
        if (stayImgIn.ItemsImages.Any())
        {
            foreach (var attachment in stayImgIn.ItemsImages)
            {
                var EnhanceYourStayItemImage = new CustomerGuestAppEnhanceYourStayItemsImage
                {
                    CustomerGuestAppEnhanceYourStayItemId = stayImgIn.CustomerGuestAppEnhanceYourStayItemId,
                    ItemsImages = attachment.ItemsImage,
                    DisaplayOrder = attachment.DisplayOrder,
                    IsActive = true
                };
                listOfStayItems.Add(EnhanceYourStayItemImage);
            }
            await _db.CustomerGuestAppEnhanceYourStayItemsImages.AddRangeAsync(listOfStayItems, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }
        var createCustomersProprtiesInfoOut = new List<CreatedEnhanceYourStayItemImageOut>();
        foreach (var entity in listOfStayItems)
        {
            CreatedEnhanceYourStayItemImageOut createdEnhanceYourStayItemImageOut = new CreatedEnhanceYourStayItemImageOut();
            createdEnhanceYourStayItemImageOut.Id = entity.Id;
            createdEnhanceYourStayItemImageOut.CustomerGuestAppEnhanceYourStayItemId = entity.CustomerGuestAppEnhanceYourStayItemId;
            createdEnhanceYourStayItemImageOut.ItemsImages = entity.ItemsImages;
            createdEnhanceYourStayItemImageOut.DisaplayOrder = entity.DisaplayOrder;
            createCustomersProprtiesInfoOut.Add(createdEnhanceYourStayItemImageOut);
        }
        return _response.Success(new CreateGuestAppEnhanceYourStayItemImageOut("Create guest app enhance your stay item images successful.", createCustomersProprtiesInfoOut));
    }
}