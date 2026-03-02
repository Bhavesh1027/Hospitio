using HospitioApi.Shared;

namespace HospitioApi.Core.HandleProduct.Queries.GetProducts;

public class GetProductsOut : BaseResponseOut
{
    public GetProductsOut(string message, List<GetProductsResponseOut> getProductsResponseOut) : base(message)
    {
        GetProductsResponseOut = getProductsResponseOut;
    }

    public List<GetProductsResponseOut> GetProductsResponseOut { get; set; } = new();

}

public class GetProductsResponseOut
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public bool? IsActive { get; set; }
    public int? TotalCount { get; set; }
}

