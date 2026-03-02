using MediatR;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersConcierge.Commands.CreateCustomerConcierge;
public record CreateCustomerConciergeRequest(CreateCustomerConciergeIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerConciergeHandler : IRequestHandler<CreateCustomerConciergeRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;

    public CreateCustomerConciergeHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerConciergeRequest request, CancellationToken cancellationToken)
    {
        var customerConciergeCategory = await _commonRepository.CustomersConciergeMultipleAddWithItems(request.In.CustomerConciergeCategories, _db, cancellationToken);

        var createCustomerConciergeOutList = new List<CreatedCustomerConciergeOut>();

        foreach (var category in customerConciergeCategory)
        {
            var createCustomerConciergeOut = new CreatedCustomerConciergeOut()
            {
                Id = category.Id,
                CustomerId = category.CustomerId,
                CustomerGuestAppBuilderId = category.CustomerGuestAppBuilderId,
                CategoryName = category.CategoryName,
                DisplayOrder = category.DisplayOrder
            };
            createCustomerConciergeOutList.Add(createCustomerConciergeOut);
        }


        return _response.Success(new CreateCustomerConciergeOut("Create customer concierge category successful.", createCustomerConciergeOutList));
    }
}
