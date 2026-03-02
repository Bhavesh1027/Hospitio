using HospitioApi.Shared;

namespace HospitioApi.Core.HandleProduct.Commands.CreateProduct;

public class CreateProductOut : BaseResponseOut
{
    public CreateProductOut(string message, CreatedProductOut createdProductOut) : base(message)
    {
        CreatedProductOut = createdProductOut;
    }
    public CreatedProductOut CreatedProductOut { get; set; }
}

public class CreatedProductOut
{
    public int Id { get; set; }

    public string? Name { get; set; }
}


