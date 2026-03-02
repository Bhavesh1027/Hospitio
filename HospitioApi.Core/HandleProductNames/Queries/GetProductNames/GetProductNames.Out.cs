using HospitioApi.Shared;

namespace HospitioApi.Core.HandleProductNames.Queries.GetProductNames;

public class GetProductNamesOut : BaseResponseOut
{
    public GetProductNamesOut(string message, List<ProductNamesOut> productNames) : base(message)
    {
        productNamesOuts = productNames;
    }
    public List<ProductNamesOut> productNamesOuts { get; set; } = new();
}
public class ProductNamesOut
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

}