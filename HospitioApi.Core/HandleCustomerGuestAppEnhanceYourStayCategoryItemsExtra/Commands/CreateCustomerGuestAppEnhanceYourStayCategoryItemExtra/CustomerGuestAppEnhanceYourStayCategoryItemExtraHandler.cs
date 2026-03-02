using MediatR;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.CreateCustomerGuestAppEnhanceYourStayCategoryItemExtra;
public record CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraRequest(CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraIn In) : IRequest<AppHandlerResponse>;
public class CustomerGuestAppEnhanceYourStayCategoryItemExtraHandler : IRequestHandler<CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;

    public CustomerGuestAppEnhanceYourStayCategoryItemExtraHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerGuestAppEnhanceYourStayCategoryItemExtraRequest request, CancellationToken cancellationToken)
    {
        /*var customerReceptionCategory = await _commonRepository.CustomersGuestAppEnhanceYourStayCategoryItemExtra(request.In.createCustomerGuestAppEnhanceYourStayCategoryItems, request.In.CustomerGuestAppEnhanceYourStayItemId, _db, cancellationToken);

        var created = new CreatedCustomersGuestAppEnhanceYourStayCategoryItemExtraOut()
        {
            CustomerGuestAppEnhanceYourStayItemId = customerReceptionCategory[0].CustomerGuestAppEnhanceYourStayItemId.Value
        };*/

        var created = new CreatedCustomersGuestAppEnhanceYourStayCategoryItemExtraOut();
        return _response.Success(new CustomerGuestAppEnhanceYourStayCategoryItemExtraOut("Create customer enhance your stay category item extra successful.", created));
    }
}
