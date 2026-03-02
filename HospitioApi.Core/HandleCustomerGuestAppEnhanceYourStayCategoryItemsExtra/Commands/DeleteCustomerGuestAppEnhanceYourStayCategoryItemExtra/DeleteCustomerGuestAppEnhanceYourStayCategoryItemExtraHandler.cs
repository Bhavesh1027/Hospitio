using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerGuestAppEnhanceYourStayCategoryItemsExtra.Commands.DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtra;
public record DeleteEnhanceYourStayCategoryItemExtraRequest(DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraIn In)
    : IRequest<AppHandlerResponse>;
public class DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraHandler : IRequestHandler<DeleteEnhanceYourStayCategoryItemExtraRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;
    public DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository
        )
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(DeleteEnhanceYourStayCategoryItemExtraRequest request, CancellationToken cancellationToken)
    {
        var exist = await _db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.Where(c => c.CustomerGuestAppEnhanceYourStayItemId == request.In.CustomerGuestAppEnhanceYourStayItemId).ToListAsync(cancellationToken);
        if (exist.Count == 0 || exist == null)
        {
            return _response.Error($"Enhance your stay category item extra with CustomerGuestAppEnhanceYourStayItemId {request.In.CustomerGuestAppEnhanceYourStayItemId} not found or user doesn't have the rights to delete it", AppStatusCodeError.Gone410);
        }
        await _commonRepository.CustomersCategoryItemExtraDelete(exist, _db, cancellationToken);
        RemoveEnhanceYourStayCategoryItemExtraOut remove = new() { CustomerGuestAppEnhanceYourStayItemId = request.In.CustomerGuestAppEnhanceYourStayItemId };
        return _response.Success(new DeleteCustomerGuestAppEnhanceYourStayCategoryItemExtraOut("Delete enhance your stay category item extra successfully.", remove));
    }
}
