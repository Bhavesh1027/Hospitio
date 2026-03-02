using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleProduct.Commands.EditProduct;

public record EditProductRequest(EditProductIn In, int Id)
    : IRequest<AppHandlerResponse>;

public class EditProductHandler : IRequestHandler<EditProductRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public EditProductHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(EditProductRequest request, CancellationToken cancellationToken)
    {
        var Product = await _db.Products.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

        if (Product == null)
        {
            return _response.Error("Product not found.", AppStatusCodeError.Conflict409);
        }

        if (await _db.Products.AnyAsync(m => m.Name == request.In.Name, cancellationToken))
        {
            return _response.Error("The product name already exists in the system.", AppStatusCodeError.Conflict409);
        }


        Product.Name = request.In.Name;

        _db.Products.Update(Product);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new EditProductOut("Edit product successful."));
    }
}
