namespace HospitioApi.Core.HandleHospitioAdminCommunication.Queries.GetAdminUserCustomers;
public class GetAdminUserCustomersIn
{
    public string SearchString { get; set; }
    public int? PageNo { get; set; }
    public int? PageSize { get; set; }
    public int? UserId { get; set; }
}
