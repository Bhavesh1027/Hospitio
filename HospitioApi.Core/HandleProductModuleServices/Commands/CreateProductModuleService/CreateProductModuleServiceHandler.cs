using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleProductModuleService.Commands.CreateProductModuleService;

public record CreateProductModuleServiceRequest(CreateProductModuleServiceIn In)
    : IRequest<AppHandlerResponse>;

public class CreateProductModuleServiceHandler : IRequestHandler<CreateProductModuleServiceRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateProductModuleServiceHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateProductModuleServiceRequest request, CancellationToken cancellationToken)
    {

        if (await _db.ProductModuleServices.AnyAsync(m => m.ProductModuleId == request.In.ProductModuleId && m.ProductId == request.In.ProductId && m.ModuleServiceId == request.In.ModuleServiceId, cancellationToken))
        {
            return _response.Error("The product module service is already exists in the system.", AppStatusCodeError.Conflict409);
        }

        var ProductModuleService = new HospitioApi.Data.Models.ProductModuleService()
        {
            ProductModuleId = request.In.ProductModuleId,
            ModuleServiceId = request.In.ModuleServiceId,
            ProductId = request.In.ProductId,
            IsActive = true,
        };

        await _db.ProductModuleServices.AddAsync(ProductModuleService, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new CreateProductModuleServiceOut("Create product module service successful."));
    }
}
