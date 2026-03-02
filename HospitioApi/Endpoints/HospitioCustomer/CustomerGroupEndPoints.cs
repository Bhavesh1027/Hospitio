using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleDepartment.Commands.EditDepartment;
using HospitioApi.Core.HandleGroups.Commands.CreateGroup;
using HospitioApi.Core.HandleGroups.Commands.DeleteGroup;
using HospitioApi.Core.HandleGroups.Commands.UpdateGroup;
using HospitioApi.Core.HandleGroups.Queries.GetGroup;
using HospitioApi.Core.HandleGroups.Queries.GetGroups;
using HospitioApi.Core.HandleGroups.Queries.GetGroupsByDepartmentId;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerGroupEndPoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
    plural__: "api/hospitio-customer/groups",
    singular: "api/hospitio-customer/group");
    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
          app.MapGet($"/{Route.Plural}",GetGroupsAsync)
        .AllowAnonymous(),
          app.MapGet($"/{Route.Singular}",GetGroupAsync)
        .AllowAnonymous(),
          app.MapGet($"/{Route.Singular}/DepartmentId",GetGroupByDepartmentIdAsync)
        .AllowAnonymous(),
          app.MapDelete($"/{Route.Singular}",DeleteGroupAsync)
        .AllowAnonymous(),
          app.MapPost($"/{Route.Singular}/create",CreateGroupAsync)
        .AllowAnonymous(),
          app.MapPost($"/{Route.Singular}/update",UpdateGroupAsync)
        .AllowAnonymous(),

    };

    #region Delegate
    private async Task<IResult> GetGroupsAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        GetGroupsIn @in = new()
        {
            UserType = Convert.ToInt32(cp.FindFirstValue("UserType")),
            UserId = Convert.ToInt32(cp.CustomerId())
        };
        return await mtrHlpr.ToResultAsync(new GetGroupsRequest(@in), ct);
    }
    private async Task<IResult> GetGroupAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "Id")] int Id, CT ct)
    {
        GetGroupIn @in = new() { 
            Id = Id ,
            UserType = Convert.ToInt32(cp.FindFirstValue("UserType"))
        };
        return await mtrHlpr.ToResultAsync(new GetGroupHandlerRequest(@in), ct);
    }
    private async Task<IResult> GetGroupByDepartmentIdAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery(Name = "DepartmentId")] int Id, CT ct)
    {
        GetGroupsByDepartmentIdIn @in = new() { 
            DepartmentId = Id,
            UserType = Convert.ToInt32(cp.FindFirstValue("UserType"))
        };
        return await mtrHlpr.ToResultAsync(new GetGroupsByDepartmentIdRequest(@in), ct);
    }
    private async Task<IResult> DeleteGroupAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromQuery] int Id, CT ct)
    {
        DeleteGroupIn @in = new() { 
            GroupId = Id ,
            UserType = Convert.ToInt32(cp.FindFirstValue("UserType"))
        };
        return await mtrHlpr.ToResultAsync(new DeleteGroupRequest(@in), ct);
    }
    private async Task<IResult> CreateGroupAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] CreateGroupIn @in, CT ct)
    {
        @in.UserType = Convert.ToInt32(cp.FindFirstValue("UserType"));
        @in.UserId = Convert.ToInt32(cp.CustomerId());
        return await mtrHlpr.ToResultAsync(new CreateGroupRequest(@in), ct);
    }

    private async Task<IResult> UpdateGroupAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, [FromBody] UpdateGroupIn @in, CT ct)
    {
        @in.UserType = Convert.ToInt32(cp.FindFirstValue("UserType"));
        return await mtrHlpr.ToResultAsync(new UpdateGroupRequest(@in), ct);
    }

    #endregion
}
