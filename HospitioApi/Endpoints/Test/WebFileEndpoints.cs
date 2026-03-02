using HospitioApi.Shared.Constants;
using System.Security.Claims;

namespace HospitioApi.Endpoints.Test;

public class WebFileEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(singular: "api/test");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {

        app.MapGet($"/{Route.Singular}/rotate", getMsgAsync)
            .RequireAuthorization(  PolicyTypes.Customer.CanView )
    };

    #region Delegates
    private IResult getMsgAsync(ClaimsPrincipal cp)
    {
        return Results.NotFound("File not found");
    }
    #endregion
}


