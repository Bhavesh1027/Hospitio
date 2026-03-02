using Microsoft.AspNetCore.Authorization;

namespace HospitioApi.Authorization;
public class HasScopeHandler : AuthorizationHandler<HasScopeRequirement>
{
    protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      HasScopeRequirement requirement
    )
    {
        var claims = context.User.Claims.ToList();
        var req = requirement.Scope;

        return Task.CompletedTask;
    }
}