using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyServiceImage.Queries.GetCustomerPropertyServiceImages;

public class GetCustomerPropertyServiceImagesOut : BaseResponseOut
{
    public GetCustomerPropertyServiceImagesOut(
        string message, List<CustomerPropertyServiceImagesOut> customerPropertyServiceImages) : base(message)
    {
        CustomerPropertyServiceImages = customerPropertyServiceImages;
    }
    public List<CustomerPropertyServiceImagesOut> CustomerPropertyServiceImages { get; set; }
}

public class CustomerPropertyServiceImagesOut
{
    public MemoryStream? MemoryStream { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
}
