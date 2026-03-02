using MediatR;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.CreateCustomerEnhanceYourStayItem;
public record CreateCustomerEnhanceYourStayItemRequest(CreateCustomerEnhanceYourStayItemIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerEnhanceYourStayItemHandler : IRequestHandler<CreateCustomerEnhanceYourStayItemRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;

    public CreateCustomerEnhanceYourStayItemHandler(ApplicationDbContext db, IHandlerResponseFactory response,ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerEnhanceYourStayItemRequest request, CancellationToken cancellationToken)
    {
        var customerEnhanceYourStayItem = await _commonRepository.CustomerEnhanceYourStayItemAdd(request.In, _db, cancellationToken);

        if (customerEnhanceYourStayItem != null)
        {
            var customerEnhanceYourStayItemImages = await _commonRepository.CustomersEnhanceYourStayItemImageAdd(request.In.ItemsImages, customerEnhanceYourStayItem.Id, _db, cancellationToken);
            var customerEnhanceYourStayItemExtra = await _commonRepository.CustomersEnhanceYourStayCategoryItemExtraAdd(request.In.CustomerEnhanceYourStayCategoryItemExtra, customerEnhanceYourStayItem.Id, _db, cancellationToken);

            var createEnhanceYourStayItemImagesOut = new List<CreatedEnhanceYourStayItemImageOut>();
            foreach (var entity in customerEnhanceYourStayItemImages)
            {
                CreatedEnhanceYourStayItemImageOut createdEnhanceYourStayItemImageOut = new CreatedEnhanceYourStayItemImageOut();
                createdEnhanceYourStayItemImageOut.Id = entity.Id;
                createdEnhanceYourStayItemImageOut.CustomerGuestAppEnhanceYourStayItemId = entity.CustomerGuestAppEnhanceYourStayItemId;
                createdEnhanceYourStayItemImageOut.ItemsImages = entity.ItemsImages;
                createdEnhanceYourStayItemImageOut.DisaplayOrder = entity.DisaplayOrder;
                createEnhanceYourStayItemImagesOut.Add(createdEnhanceYourStayItemImageOut);
            }

            var createdEnhanceYourStayCategoryItemExtraOutList = new List<CreatedEnhanceYourStayCategoryItemExtraOut>();
            foreach (var itemExtra in customerEnhanceYourStayItemExtra)
            {
                CreatedEnhanceYourStayCategoryItemExtraOut createdEnhanceYourStayCategoryItemExtraOut = new CreatedEnhanceYourStayCategoryItemExtraOut()
                {
                    Id = itemExtra.Id,
                    QueType = itemExtra.QueType,
                    Questions = itemExtra.Questions,
                    OptionValues = itemExtra.OptionValues
                };
                createdEnhanceYourStayCategoryItemExtraOutList.Add(createdEnhanceYourStayCategoryItemExtraOut);
            }

            var createCustomerEnhanceYourStayItemOut = new CreatedCustomerEnhanceYourStayItemOut
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
                ItemsImages = createEnhanceYourStayItemImagesOut,
                CustomerEnhanceYourStayCategoryItemsExtra = createdEnhanceYourStayCategoryItemExtraOutList

            };
            return _response.Success(new CreateCustomerEnhanceYourStayItemOut("Create customer enhance your stay item successful.", createCustomerEnhanceYourStayItemOut));
        } else
        {
            return _response.Error("Unable to add enhance your stay item.", AppStatusCodeError.Forbidden403);
        }
    }
}
