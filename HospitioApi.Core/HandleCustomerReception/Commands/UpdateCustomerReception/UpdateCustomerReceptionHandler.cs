using MediatR;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerReception.Commands.UpdateCustomerReception;
public record UpdateCustomerReceptionRequest(UpdateCustomerReceptionIn In, string CustomerId) : IRequest<AppHandlerResponse>;
public class UpdateCustomerReceptionHandler : IRequestHandler<UpdateCustomerReceptionRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;
    public UpdateCustomerReceptionHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomerReceptionRequest request, CancellationToken cancellationToken)
    {
        var customerReceptionCategory = await _commonRepository.CustomersReceptionMultipleUpdateWithItems(request.In.CustomerReceptionCategories,Convert.ToInt32(request.CustomerId), _db, cancellationToken);

        //var updatedCustomerReceptionOutList = new List<UpdatedCustomerReceptionOut>();
        //foreach (var category in customerReceptionCategories)
        //{
            var updatedCustomerReceptionOut = new UpdatedCustomerReceptionOut()
            {
                Id = customerReceptionCategory.Id,
                CustomerGuestAppBuilderId = customerReceptionCategory.CustomerGuestAppBuilderId,
                CategoryName = customerReceptionCategory.CategoryName,
                DisplayOrder = customerReceptionCategory.DisplayOrder,
            };
        //    updatedCustomerReceptionOutList.Add(updatedCustomerReceptionOut);
        //}
        return _response.Success(new UpdateCustomerReceptionOut("Create customer reception successful.", updatedCustomerReceptionOut));
    }
}
