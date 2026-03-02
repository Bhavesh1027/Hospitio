using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.DeleteCustomerEnhanceYourStay;
public record DeleteCustomerEnhanceYourStayRequest(DeleteCustomerEnhanceYourStayIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerEnhanceYourStayHandler: IRequestHandler<DeleteCustomerEnhanceYourStayRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteCustomerEnhanceYourStayHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerEnhanceYourStayRequest request, CancellationToken cancellationToken)
    {
        var mainEntity = await _db.CustomerGuestAppEnhanceYourStayCategories.Where(m => m.Id == request.In.CategoryId).FirstOrDefaultAsync(cancellationToken);

        if (mainEntity == null)
        {
            return _response.Error($"Customers enhance your stay category with Id {request.In.CategoryId} could not be found.", AppStatusCodeError.Gone410);
        }

        var secondaryEntities = await _db.CustomerGuestAppEnhanceYourStayItems.Where(s => s.CustomerGuestAppBuilderCategoryId == request.In.CategoryId).ToListAsync(cancellationToken);
        if (secondaryEntities != null)
        {
            foreach (var secondaryEntity in secondaryEntities)
            {
                _db.CustomerGuestAppEnhanceYourStayItems.Remove(secondaryEntity);
            }
            
        }

        _db.CustomerGuestAppEnhanceYourStayCategories.Remove(mainEntity);
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new DeleteCustomerEnhanceYourStayOut("Delete customers enhance your stay category successful.", new() { CategoryId = mainEntity.Id }));
    }
}
