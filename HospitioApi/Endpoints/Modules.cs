using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Endpoints;
public interface IEndpointsModule
{
    AppRoute Route { get; init; }
    RouteHandlerBuilder[] MapEndpoints(WebApplication endpoints);
    void AddGroupNameToEndpoint(params RouteHandlerBuilder[] routeHandlerBuilders)
    {
        routeHandlerBuilders.ToList().ForEach(routeHandlerBuilder =>
        {
            if (Route.PluralNullable is not null)
            {
                routeHandlerBuilder.WithGroupName(Route.PluralNullable);
            }
            else if (Route.SingularNullable is not null)
            {
                routeHandlerBuilder.WithGroupName(Route.SingularNullable);
            }
        });
    }
}

public static class EndpointsModuleExtensions
{
    /** this could also be added into the DI container */
    static private readonly List<IEndpointsModule> registeredEndpointsModules = new();

    public static IServiceCollection RegisterEndpointsModules(this IServiceCollection services)
    {
        var modules = DiscoverEndpointsModules();
        foreach (var module in modules)
        {
            registeredEndpointsModules.Add(module);
        }

        return services;
    }

    public static WebApplication MapMinimalEndpoints(this WebApplication app)
    {
        foreach (var module in registeredEndpointsModules)
        {
            module.AddGroupNameToEndpoint(module.MapEndpoints(app));
        }
        return app;
    }

    private static IEnumerable<IEndpointsModule> DiscoverEndpointsModules()
    {
        return typeof(IEndpointsModule).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IEndpointsModule)))
            .Select(Activator.CreateInstance)
            .Cast<IEndpointsModule>();
    }
}

public class AppRoute
{
    public AppRoute(string? plural__ = null, string? singular = null)
    {
        if (plural__ is null && singular is null) { throw new AppException("At least one Plural or Singular routes need to be provided", AppStatusCodeError.InternalServerError500); }
        if (IsRouteWhiteSpace(plural__)) { throw new AppException(  /**/"Plural route cannot be whitespace", AppStatusCodeError.InternalServerError500); }
        if (IsRouteWhiteSpace(singular)) { throw new AppException(/**/"Singular route cannot be whitespace", AppStatusCodeError.InternalServerError500); }
        PluralNullable = plural__;
        SingularNullable = singular;
    }

    public string Plural =>     /**/ PluralNullable ?? throw new AppException(  /**/"Plural Route Name was never set", AppStatusCodeError.InternalServerError500);
    public string Singular => /**/ SingularNullable ?? throw new AppException(/**/"Singular Route Name was never set", AppStatusCodeError.InternalServerError500);
    public string? PluralNullable { get; }
    public string? SingularNullable { get; }
    private static bool IsRouteWhiteSpace(string? route) => route is not null && string.IsNullOrWhiteSpace(route);
}
