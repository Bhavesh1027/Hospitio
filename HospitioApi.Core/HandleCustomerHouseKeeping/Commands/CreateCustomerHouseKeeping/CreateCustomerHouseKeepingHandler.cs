using MediatR;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.CreateCustomerHouseKeeping;
public record CreateCustomerHouseKeepingRequest(CreateCustomerHouseKeepingIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerHouseKeepingHandler : IRequestHandler<CreateCustomerHouseKeepingRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;

    public CreateCustomerHouseKeepingHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerHouseKeepingRequest request, CancellationToken cancellationToken)
    {
        var customerHouseKeepingCategory = await _commonRepository.CustomersHouseKeepingMultipleAddWithItems(request.In.CustomerHouseKeepingCategories, _db, cancellationToken);

        var createCustomerHouseKeepingOutList = new List<CreatedCustomerHouseKeepingOut>();

        foreach (var category in customerHouseKeepingCategory)
        {
            var createCustomerHouseKeepingOut = new CreatedCustomerHouseKeepingOut()
            {
                Id = category.Id,
                CustomerId = category.CustomerId,
                CustomerGuestAppBuilderId = category.CustomerGuestAppBuilderId,
                CategoryName = category.CategoryName,
                DisplayOrder = category.DisplayOrder,
            };
            createCustomerHouseKeepingOutList.Add(createCustomerHouseKeepingOut);
        }

        return _response.Success(new CreateCustomerHouseKeepingOut("Create customer house keeping successful.", createCustomerHouseKeepingOutList));
    }
}
