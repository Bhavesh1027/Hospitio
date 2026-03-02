using MediatR;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerRoomService.Commands.CreateCustomerRoomService;
public record CreateCustomerRoomServiceRequest(CreateCustomerRoomServiceIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerRoomServiceHandler : IRequestHandler<CreateCustomerRoomServiceRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;

    public CreateCustomerRoomServiceHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerRoomServiceRequest request, CancellationToken cancellationToken)
    {
        //var customerRoomServiceCategory = await _commonRepository.CustomersRoomServiceAdd(request.In, _db, cancellationToken);
        var customerRoomServiceCategory = await _commonRepository.CustomerRoomServiceMultipleAddWithItems(request.In, _db, cancellationToken);
        //var customerRoomServiceCategoryItems = new List<CustomerGuestAppRoomServiceItem>();
        if (request.In.CustomerRoomServiceCategories != null)
        {
            List<CreatedCustomerRoomServiceOut> createdCustomerRoomServiceOuts = new List<CreatedCustomerRoomServiceOut>();
            // customerRoomServiceCategoryItems = await _commonRepository.cu(request.In.CustomerRoomServiceItems, customerRoomServiceCategory.Id, _db, cancellationToken);

            foreach (var category in request.In.CustomerRoomServiceCategories)
            {
                var createCustomerRoomServiceOut = new CreatedCustomerRoomServiceOut()
                {
                    Id = category.Id,
                    CustomerId = category.CustomerId,
                    CustomerGuestAppBuilderId = category.CustomerGuestAppBuilderId,
                    CategoryName = category.CategoryName,
                    DisplayOrder = category.DisplayOrder,
                    /* CustomerRoomServiceItems = customerRoomServiceCategoryItems*/
                };
                createdCustomerRoomServiceOuts.Add(createCustomerRoomServiceOut);
            }
            return _response.Success(new CreateCustomerRoomServiceOut("Create customer room service successful.", createdCustomerRoomServiceOuts));
        }
        else
        {
            return _response.Error("Unable to add room service request successfully.", AppStatusCodeError.Forbidden403);
        }

    }
}
