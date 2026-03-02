using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleMusement.Commands.MusementLogin;
using HospitioApi.Core.HandleMusement.Queries.DownloadMusementData;
using HospitioApi.Core.HandleMusement.Queries.GetMusementData;
using HospitioApi.Helpers;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class MusementEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(singular: "api/hospitio-admin/musement");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {

        app.MapPost($"/{Route.Singular}/login" ,MusementLoginAsync)
           .RequireAuthorization(),
        app.MapGet($"/{Route.Singular}/getMusementData" ,GetMusementDataAsync)
           .RequireAuthorization(),
        app.MapGet($"/{Route.Singular}/downloadMusementData" ,DownloadMusementDataAsync)
           .RequireAuthorization(),
    };

    #region Delegates
    private async Task<IResult> MusementLoginAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        return await mtrHlpr.ToResultAsync(new MusementLoginRequest(), ct);
    }

    private async Task<IResult> GetMusementDataAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct , string? searchValue , int pageNo , int pageSize)
    {
        GetMusementDataIn @in = new()
        {
            SearchValue = searchValue,
            PageNo = pageNo,
            PageSize = pageSize
        };
        return await mtrHlpr.ToResultAsync(new GetMusementDataRequest(@in), ct);
    }
    private async Task<IResult> DownloadMusementDataAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
=> await mtrHlpr.ToResultAsync(new DownloadMusementDataRequest(), ct);
    #endregion
}
