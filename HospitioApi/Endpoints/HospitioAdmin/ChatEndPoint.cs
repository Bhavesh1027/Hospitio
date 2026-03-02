using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleChat.Chat;
using HospitioApi.Core.HandleCustomers.Queries.GetCustomersMainInfo;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class ChatEndPoint : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
  singular: "ws/chat");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
            app.MapPost($"/{Route.Singular}",ChatAsync)
            .AllowAnonymous()
    };
    #region Delegates
    private async Task<IResult> ChatAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        {
            return await mtrHlpr.ToResultAsync(new ChatRequest(), ct);
        }
    #endregion
}
