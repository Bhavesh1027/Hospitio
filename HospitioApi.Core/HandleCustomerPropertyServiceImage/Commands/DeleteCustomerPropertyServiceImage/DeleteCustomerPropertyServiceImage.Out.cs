using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyServiceImage.Commands.DeleteCustomerPropertyServiceImage;

public class DeleteCustomerPropertyServiceImageOut : BaseResponseOut
{
    public DeleteCustomerPropertyServiceImageOut(string message, RemoveCustomerPropertyServiceImageOut removeCustomerPropertyServiceImageOut) : base(message)
    {
        RemoveCustomerPropertyServiceImageOut = removeCustomerPropertyServiceImageOut;
    }
    public RemoveCustomerPropertyServiceImageOut RemoveCustomerPropertyServiceImageOut { get; set; }
}

public class RemoveCustomerPropertyServiceImageOut
{
    public int DeletedCustomerPropertyServiceImageId { get; set; }
}
