namespace HospitioApi.Core.HandleCustomers.Commands.UpdateERPServicePack
{
    public class UpdateERPServicePackIn
    {
        public string? PylonUniqueCustomerId { get; set; }
        public string? PurchaseType { get;set; }
        public string? ServicePack { get; set; }
        public int ExpirationInDay { get; set; }
    }
}
