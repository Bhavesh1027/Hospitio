using HospitioApi.Data.MultiTenancy;

namespace HospitioApi.Middleware;

public class MultiTenantServiceMiddleware : IMiddleware
{
    private readonly ITenantService setter;
    public MultiTenantServiceMiddleware(ITenantService setter)
    {
        this.setter = setter;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        //var tenantId = PrincipalExtensions.TenantId(context.User);
        //var tenantName = PrincipalExtensions.TenantName(context.User);

        var tenantId = "";
        var tenantName = "";
        if (!string.IsNullOrEmpty(tenantId) && !string.IsNullOrEmpty(tenantName))
            setter.SetTenant(tenantId, tenantName);

        await next(context);
    }
}

