using HospitioApi.Core.HandleCustomers.Commands.CreateERPCustomer;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomers.Commands.UpdateERPServicePack
{
    public class UpdateERPServicePackOut : BaseResponseOut
    {
        public UpdateERPServicePackOut(string message, UpdatedERPServicePackOut updatedERPServiceOut) : base(message)
        {
            UpdateERPServiceOut = updatedERPServiceOut;
        }
        public UpdatedERPServicePackOut UpdateERPServiceOut { get; set; }
    }
    public class UpdatedERPServicePackOut
    {
        public string? PylonUniqueCustomerId { get; set; }

    }
}
