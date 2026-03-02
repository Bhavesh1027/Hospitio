using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStayCategories.Commands.DeleteCustomerEnhanceYourStayCategory;
public record DeleteCustomerEnhanceYourStayCategoryRequest(DeleteCustomerEnhanceYourStayCategoryIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerEnhanceYourStayCategoryHandler : IRequestHandler<DeleteCustomerEnhanceYourStayCategoryRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteCustomerEnhanceYourStayCategoryHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerEnhanceYourStayCategoryRequest request, CancellationToken cancellationToken)
    {
        var CustomerEnhanceYourStayCategory = await _db.CustomerGuestAppEnhanceYourStayCategories.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (CustomerEnhanceYourStayCategory == null)
        {
            return _response.Error($"Customers enhance your stay category with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }
        _db.CustomerGuestAppEnhanceYourStayCategories.Remove(CustomerEnhanceYourStayCategory);
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new DeleteCustomerEnhanceYourStayCategoryOut("Delete customers enhance your stay category successful.", new() { Id = CustomerEnhanceYourStayCategory.Id }));
    }
}
