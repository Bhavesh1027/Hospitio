namespace HospitioApi.Data.MultiTenancy;
public class TenantService : ITenantService
{
    public string TenantId { get; private set; } = string.Empty;
    public string TenantName { get; private set; } = string.Empty;
    public void SetTenant(string tenantId, string tenantName)
    {
        TenantId = tenantId;
        TenantName = tenantName;
    }
}
public interface ITenantService
{
    string TenantId { get; }
    string TenantName { get; }
    void SetTenant(string tenantId, string tenantName);
}
