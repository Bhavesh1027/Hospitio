using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleUserAccount.Commands.CreateEditUserAccount;
using HospitioApi.Core.HandleUserAccount.Commands.EditUserAccount;
using HospitioApi.Core.HandleUserAccount.Commands.UpdateUserProfile;
using HospitioApi.Core.HandleUserAccount.Commands.UpdateUserStatus;
using HospitioApi.Core.HandleUserAccount.Queries.GetDepartmentsUsers;
using HospitioApi.Core.HandleUserAccount.Queries.GetUserById;
using HospitioApi.Core.HandleUserAccount.Queries.GetUserProfile;
using HospitioApi.Core.HandleUserAccount.Queries.GetUsers;
using HospitioApi.Core.HandleUserAccount.Queries.GetUsersByGroupId;
using HospitioApi.Core.HandleUserAccount.Queries.GetUsersForDropdown;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class UserEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
        plural__: "api/hospitio-admin/users",
        singular: "api/hospitio-admin/user");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {

        app.MapGet($"/{Route.Plural}", GetUsersAsync)
            .AllowAnonymous(),
         app.MapGet($"/{Route.Plural}" + "/all", GetDepartmentsUsersAsync)
            .AllowAnonymous(),
         app.MapGet($"/{Route.Singular}"  + "/{Id}", GetUserByIdAsync)
            .AllowAnonymous(),
                  app.MapGet($"/{Route.Plural}"  + "/{GroupId}", GetUserByGroupIdAsync)
            .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}", CreateUserAsync)
            .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}"  + "/edit", EditUserAsync)
            .AllowAnonymous(),
         app.MapPost($"/{Route.Singular}"  + "/status-update", UpdateUserAsync)
            .AllowAnonymous(),
         app.MapGet($"/{Route.Plural}"  + "/allforalert", GetActiveUsersAsync)
            .RequireAuthorization(),
           app.MapGet($"/{Route.Singular}"  + "/Profile", GetUserProfileAsync)
            .RequireAuthorization(),
             app.MapPost($"/{Route.Singular}"  + "/ProfileUpdate", UpdateUserProfileAsync)
            .RequireAuthorization(),

    };

    #region Delegates
    private async Task<IResult> GetUsersAsync([FromServices] IMediatorHelper mtrHlpr, CT ct, int? DepartmentId, int? GroupId, int? UserLevel, int? UserId)
    {
        GetUsersIn @In = new()
        {
            DepartmentId = DepartmentId,
            GroupId = GroupId,
            UserLevelId = UserLevel,
            UserId = UserId == null ? 0 : UserId.Value
        };
        return await mtrHlpr.ToResultAsync(new GetUsersRequest(@In), ct);
    }

    private async Task<IResult> GetUserProfileAsync([FromServices] IMediatorHelper mtrHlpr, CT ct, ClaimsPrincipal cp)
    {
        GetUserProfileIn @In = new()
        {
            UserId = Convert.ToInt32(cp.UserId()),
            UserType = cp.UserType()
        };
        return await mtrHlpr.ToResultAsync(new GetUserProfileRequest(@In), ct);
    }
    private async Task<IResult> GetDepartmentsUsersAsync([FromServices] IMediatorHelper mtrHlpr, string? SearchValue, int PageNo, int PageSize, string? SortColumn, string? SortOrder, CT ct)
    {
        GetDepartmentsUsersIn @In = new()
        {
            //SearchColumn = SearchColumn,
            SearchValue = SearchValue,
            PageNo = PageNo,
            PageSize = PageSize,
            SortColumn = SortColumn,
            SortOrder = SortOrder
        };
        return await mtrHlpr.ToResultAsync(new GetDepartmentsUsersRequest(@In), ct);
    }

    private async Task<IResult> GetUserByGroupIdAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int GroupId, CT ct)
    {
        GetUsersByGroupIdIn @In = new()
        {
            GroupId = GroupId
        };
        return await mtrHlpr.ToResultAsync(new GetUsersByGroupIdRequest(@In), ct);
    }

    private async Task<IResult> GetUserByIdAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, CT ct)
    {
        GetUserByIdIn @In = new()
        {
            Id = Id
        };
        return await mtrHlpr.ToResultAsync(new GetUserByIdRequest(@In), ct);
    }

    private async Task<IResult> CreateUserAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateUserAccountIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CreateUserAccountRequest(@in), ct);
    private async Task<IResult> EditUserAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] EditUserAccountIn @in, CT ct)
    => await mtrHlpr.ToResultAsync(new EditUserAccountRequest(@in), ct);
    private async Task<IResult> UpdateUserAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] UpdateUserStatusIn @in, CT ct)
    => await mtrHlpr.ToResultAsync(new UpdateUserStatusRequest(@in), ct);

    private async Task<IResult> GetActiveUsersAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp,CT ct)
        => await mtrHlpr.ToResultAsync(new GetUsersForDropdownRequest(Convert.ToInt32(cp.UserId())), ct);

    private async Task<IResult> UpdateUserProfileAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateUserProfileIn @in, CT ct)
    {

        @in.UserType = Convert.ToInt32(cp.UserType());
        return await mtrHlpr.ToResultAsync(new UpdateUserProfileRequest(@in), ct);
    }
    #endregion
}