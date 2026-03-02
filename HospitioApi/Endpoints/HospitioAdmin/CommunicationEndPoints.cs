using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleHospitioAdminCommunication.Queries.GetAdminUserCustomers;
using HospitioApi.Core.HandleHospitioAdminCommunication.Queries.GetAdminUserCustomersDetail;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;
public class CommunicationEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(plural__: "api/hospitio-admin/communications");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
     {
         app.MapGet($"/{Route.Plural}/Search",GetCommunicationAsync)
        .RequireAuthorization(),

         app.MapGet($"/{Route.Plural}/", GetAdminUserCustomersDetailByCustomerIdAsync)
         .RequireAuthorization(),
    };

    #region Delegate 
    private async Task<IResult> GetCommunicationAsync([FromServices] IMediatorHelper mtrHlpr, string? SearchValue, int PageNo, int PageSize, ClaimsPrincipal cp, CT ct)
    {
        GetAdminUserCustomersIn @in = new()
        {
            SearchString = SearchValue,
            PageNo = PageNo,
            PageSize = PageSize,
            UserId = int.Parse(cp.UserId())
        };
        return await mtrHlpr.ToResultAsync(new GetAdminUserCustomersRequest(@in), ct);
    }
    private async Task<IResult> GetAdminUserCustomersDetailByCustomerIdAsync([FromServices] IMediatorHelper mtrHlpr, int CustomerId, string UserType, ClaimsPrincipal cp, CT ct)
    {
        GetAdminUserCustomersDetailIn @in = new()
        {
            CustomerId = CustomerId,
            UserType = UserType
        };
        return await mtrHlpr.ToResultAsync(new GetAdminUserCustomersDetailRequest(@in), ct);
    }
    #endregion
}
