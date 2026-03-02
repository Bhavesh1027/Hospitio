using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.CreateCustomerEnhanceYourStayCategory;
public record CreateCustomerEnhanceYourStayCategoryRequest(CreateCustomerEnhanceYourStayCategoryIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerEnhanceYourStayCategoryHandler : IRequestHandler<CreateCustomerEnhanceYourStayCategoryRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateCustomerEnhanceYourStayCategoryHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerEnhanceYourStayCategoryRequest request, CancellationToken cancellationToken)
    {
        var customerEnhanceYourStayCategory = new CustomerGuestAppEnhanceYourStayCategory
        {
            CustomerId = request.In.CustomerId,
            CustomerGuestAppBuilderId = request.In.CustomerGuestAppBuilderId,
            CategoryName = request.In.CategoryName
        };

        await _db.CustomerGuestAppEnhanceYourStayCategories.AddAsync(customerEnhanceYourStayCategory, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        var createCustomerEnhanceYourStayCategoryOut = new CreatedCustomerEnhanceYourStayCategoryOut
        {
            Id = customerEnhanceYourStayCategory.Id,
            CustomerId = customerEnhanceYourStayCategory.CustomerId,
            CustomerGuestAppBuilderId = customerEnhanceYourStayCategory.CustomerGuestAppBuilderId,
            CategoryName = customerEnhanceYourStayCategory.CategoryName
        };

        return _response.Success(new CreateCustomerEnhanceYourStayCategoryOut("Create customer enhance your stay category successful.", createCustomerEnhanceYourStayCategoryOut));
    }
}
