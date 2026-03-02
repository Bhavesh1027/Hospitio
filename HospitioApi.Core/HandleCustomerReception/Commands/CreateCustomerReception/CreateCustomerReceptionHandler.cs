using MediatR;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerReception.Commands.CreateCustomerReception;
public record CreateCustomerReceptionRequest(CreateCustomerReceptionIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerReceptionHandler : IRequestHandler<CreateCustomerReceptionRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;

    public CreateCustomerReceptionHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerReceptionRequest request, CancellationToken cancellationToken)
    {
        var customerReceptionCategories = await _commonRepository.CustomersReceptionMultipleAddWithItems(request.In.CuastomerReceptionCategories, _db, cancellationToken);

        var createCustomerReceptionOutList = new List<CreatedCustomerReceptionOut>();

        foreach (var category in customerReceptionCategories)
        {
            var createCustomerReceptionOut = new CreatedCustomerReceptionOut()
            {
                Id = category.Id,
                CustomerId = category.CustomerId,
                CustomerGuestAppBuilderId = category.CustomerGuestAppBuilderId,
                CategoryName = category.CategoryName,
                DisplayOrder = category.DisplayOrder
            };
            createCustomerReceptionOutList.Add(createCustomerReceptionOut);
        }
            

        return _response.Success(new CreateCustomerReceptionOut("Create customer reception category successful.", createCustomerReceptionOutList));
    }
}
