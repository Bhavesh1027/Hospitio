using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.CreateCustomerEnhanceYourStayItem;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.UpdateCustomerEnhanceYourStayItem;
public record UpdateCustomerEnhanceYourStayItemRequest(UpdateCustomerEnhanceYourStayItemIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomerEnhanceYourStayItemHandler : IRequestHandler<UpdateCustomerEnhanceYourStayItemRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;
    public UpdateCustomerEnhanceYourStayItemHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomerEnhanceYourStayItemRequest request, CancellationToken cancellationToken)
    {
        var updateCustomerEnhanceYourStayItem = await _db.CustomerGuestAppEnhanceYourStayItems.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);

        if (updateCustomerEnhanceYourStayItem == null)
        {
            return _response.Error($"Customers enhance your stay category Item with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        var customerEnhanceYourStayItem = await _commonRepository.CustomerEnhanceYourStayItemUpdate(request.In, updateCustomerEnhanceYourStayItem, _db, cancellationToken);

        if (customerEnhanceYourStayItem != null)
        {
            var customerEnhanceYourStayItemImages = await _commonRepository.CustomersEnhanceYourStayItemImageUpdate(request.In.ItemsImages, customerEnhanceYourStayItem.Id, _db, cancellationToken);
            var customerEnhanceYourStayItemExtra = await _commonRepository.CustomersEnhanceYourStayCategoryItemExtraUpdate(request.In.CustomerEnhanceYourStayCategoryItemExtra, customerEnhanceYourStayItem.Id, _db, cancellationToken);

            var updateEnhanceYourStayItemImagesOut = new List<UpdatedEnhanceYourStayItemImageOut>();
            foreach (var entity in customerEnhanceYourStayItemImages)
            {
                UpdatedEnhanceYourStayItemImageOut updatedEnhanceYourStayItemImageOut = new UpdatedEnhanceYourStayItemImageOut();
                updatedEnhanceYourStayItemImageOut.Id = entity.Id;
                updatedEnhanceYourStayItemImageOut.CustomerGuestAppEnhanceYourStayItemId = entity.CustomerGuestAppEnhanceYourStayItemId;
                updatedEnhanceYourStayItemImageOut.ItemsImages = entity.ItemsImages;
                updatedEnhanceYourStayItemImageOut.DisaplayOrder = entity.DisaplayOrder;
                updateEnhanceYourStayItemImagesOut.Add(updatedEnhanceYourStayItemImageOut);
            }

            var updatedEnhanceYourStayCategoryItemExtraOutList = new List<UpdatedEnhanceYourStayCategoryItemExtraOut>();
            foreach (var itemExtra in customerEnhanceYourStayItemExtra)
            {
                UpdatedEnhanceYourStayCategoryItemExtraOut updatedEnhanceYourStayCategoryItemExtraOut = new UpdatedEnhanceYourStayCategoryItemExtraOut()
                {
                    Id = itemExtra.Id,
                    QueType = itemExtra.QueType,
                    Questions = itemExtra.Questions,
                    OptionValues = itemExtra.OptionValues
                };
                updatedEnhanceYourStayCategoryItemExtraOutList.Add(updatedEnhanceYourStayCategoryItemExtraOut);
            }

            var updateCustomerEnhanceYourStayItemOut = new UpdatedCustomerEnhanceYourStayItemOut
            {
                Id = customerEnhanceYourStayItem.Id,
                CustomerId = customerEnhanceYourStayItem.CustomerId,
                CustomerGuestAppBuilderId = customerEnhanceYourStayItem.CustomerGuestAppBuilderId,
                CustomerGuestAppBuilderCategoryId = customerEnhanceYourStayItem.CustomerGuestAppBuilderCategoryId,
                Badge = customerEnhanceYourStayItem.Badge,
                ShortDescription = customerEnhanceYourStayItem.ShortDescription,
                LongDescription = customerEnhanceYourStayItem.LongDescription,
                ButtonType = customerEnhanceYourStayItem.ButtonType,
                ButtonText = customerEnhanceYourStayItem.ButtonText,
                ChargeType = customerEnhanceYourStayItem.ChargeType,
                Discount = customerEnhanceYourStayItem.Discount,
                Price = customerEnhanceYourStayItem.Price,
                Currency = customerEnhanceYourStayItem.Currency,
                DisplayOrder = customerEnhanceYourStayItem.DisplayOrder,
                ItemsImages = updateEnhanceYourStayItemImagesOut,
                CustomerEnhanceYourStayCategoryItemsExtra = updatedEnhanceYourStayCategoryItemExtraOutList

            };

            return _response.Success(new UpdateCustomerEnhanceYourStayItemOut("Update customers enhance your stay category item successful.", updateCustomerEnhanceYourStayItemOut));
        }
        else
        {
            return _response.Error("Unable to add enhance your stay item.", AppStatusCodeError.Forbidden403);
        }
    }
}
