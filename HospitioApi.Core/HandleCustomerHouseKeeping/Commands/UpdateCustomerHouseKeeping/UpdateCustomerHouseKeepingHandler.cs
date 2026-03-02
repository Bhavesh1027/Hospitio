using MediatR;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerHouseKeeping.Commands.UpdateCustomerHouseKeeping;
public record UpdateCustomerHouseKeepingRequest(UpdateCustomerHouseKeepingIn In,string CustomerId) : IRequest<AppHandlerResponse>;
public class UpdateCustomerHouseKeepingHandler : IRequestHandler<UpdateCustomerHouseKeepingRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;
    public UpdateCustomerHouseKeepingHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomerHouseKeepingRequest request, CancellationToken cancellationToken)
    {
        var customerHouseKeepingCategory = await _commonRepository.CustomersHouseKeepingMultipleUpdateWithItems(request.In.CustomerHouseKeepingCategories, Convert.ToInt32(request.CustomerId),_db, cancellationToken);

        //var updatedCustomerHouseKeepingOutList = new List<UpdatedCustomerHouseKeepingOut>();
        //foreach (var category in customerHouseKeepingCategory)
        //{
            var updatedCustomerHouseKeepingOut = new UpdatedCustomerHouseKeepingOut()
            {
                Id = customerHouseKeepingCategory.Id,
                CustomerGuestAppBuilderId = customerHouseKeepingCategory.CustomerGuestAppBuilderId,
                CategoryName = customerHouseKeepingCategory.CategoryName,
                DisplayOrder = customerHouseKeepingCategory.DisplayOrder
            };
    //        updatedCustomerHouseKeepingOutList.Add(updatedCustomerHouseKeepingOut);
    //}

        return _response.Success(new UpdateCustomerHouseKeepingOut("Update customer house keeping successful.", updatedCustomerHouseKeepingOut));
    }
}
