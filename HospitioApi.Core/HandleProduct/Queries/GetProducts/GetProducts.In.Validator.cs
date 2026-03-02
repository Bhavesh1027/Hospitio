using FluentValidation;
namespace HospitioApi.Core.HandleProduct.Queries.GetProducts;
public class GetProductsValidator : AbstractValidator<GetProductsRequest>
{
    public GetProductsValidator()
    {
    }

    public class GetProductsInValidator : AbstractValidator<GetProductsIn>
    {
        public GetProductsInValidator()
        {
        }
    }
}
