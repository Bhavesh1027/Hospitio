using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.UpdateCustomerEnhanceYourStayCategory;
public record UpdateCustomerEnhanceYourStayCategoryRequest(UpdateCustomerEnhanceYourStayCategoryIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomerEnhanceYourStayCategoryItemHandler : IRequestHandler<UpdateCustomerEnhanceYourStayCategoryRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public UpdateCustomerEnhanceYourStayCategoryItemHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomerEnhanceYourStayCategoryRequest request, CancellationToken cancellationToken)
    {
        var updateCustomerEnhanceYourStayCategory = await _db.CustomerGuestAppEnhanceYourStayCategories.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);

        if (updateCustomerEnhanceYourStayCategory == null)
        {
            return _response.Error($"Customers enhance your stay category with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }

        updateCustomerEnhanceYourStayCategory.CustomerId = request.In.CustomerId;
        updateCustomerEnhanceYourStayCategory.CustomerGuestAppBuilderId = request.In.CustomerGuestAppBuilderId;
        updateCustomerEnhanceYourStayCategory.CategoryName = request.In.CategoryName;

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new UpdateCustomerEnhanceYourStayCategoryOut("Update customers enhance your stay category successful.", new()
        {
            Id = request.In.Id,
            CustomerId = request.In.CustomerId,
            CustomerGuestAppBuilderId = request.In.CustomerGuestAppBuilderId,
            CategoryName = request.In.CategoryName

        }));
    }
}
