using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleProductModuleService.Queries.GetProductModuleServiceById;

public record GetProductModuleServiceByIdRequest(int Id)
    : IRequest<AppHandlerResponse>;

public class GetProductModuleServiceByIdHandler : IRequestHandler<GetProductModuleServiceByIdRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public GetProductModuleServiceByIdHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetProductModuleServiceByIdRequest request, CancellationToken cancellationToken)
    {
        var productModuleService = await _db.ProductModuleServices.Include(x => x.Product).Include(x => x.ProductModule).Include(x => x.ModuleService).FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (productModuleService == null)
        {
            return _response.Error("Product module service not found.", AppStatusCodeError.Conflict409);
        }

        var getProductModuleServiceByIdResponseOut = new GetProductModuleServiceByIdResponseOut()
        {
            Id = productModuleService.Id,
            ProductId = productModuleService.ProductId,
            ProductName = productModuleService?.Product?.Name,
            ModuleServiceId = productModuleService?.ModuleServiceId,
            ModuleServiceName = productModuleService?.ModuleService?.Name,
            ProductModuleId = productModuleService?.ModuleServiceId,
        };

        return _response.Success(new GetProductModuleServiceByIdOut("Get product module service successful.", getProductModuleServiceByIdResponseOut));
    }
}
