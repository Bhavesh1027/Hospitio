using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleDepartment.Commands.CreateCustomerDepartment;
using HospitioApi.Core.HandleDepartment.Commands.EditDepartment;
using HospitioApi.Core.HandleDepartment.Queries.GetCustomerDepartmentById.cs;
using HospitioApi.Core.HandleDepartment.Queries.GetDepartments;
using HospitioApi.Helpers;
using HospitioApi.Shared;
using System.Security.Claims;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class CustomerDepartmentEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(
      plural__: "api/hospitio-customer/departments",
      singular: "api/hospitio-customer/department");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {

        app.MapPost($"/{Route.Plural}/create", CreateAsync)
            .RequireAuthorization(),

        app.MapPost($"/{Route.Plural}/" + "{Id}/edit", EditAsync)
            .RequireAuthorization(),

        app.MapGet($"/{Route.Plural}/" +"{Id}", GetByIdAsync)
            .RequireAuthorization(),

        app.MapGet($"/{Route.Plural}" , GetAllAsync)
           .RequireAuthorization(),
    };

    #region Delegates
    private async Task<IResult> CreateAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CreateCustomerDepartmentIn @in, ClaimsPrincipal cp, CT ct)
    {
        @in.CustomerId = Convert.ToInt32(cp.CustomerId());
        return await mtrHlpr.ToResultAsync(new CreateCustomerDepartmentRequest(@in), ct);
    }

    private async Task<IResult> EditAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, [FromBody] EditDepartmentIn @in, ClaimsPrincipal cp, CT ct)
    {
        @in.UserType = Convert.ToInt32(cp.FindFirstValue("UserType"));
        return await mtrHlpr.ToResultAsync(new EditDepartmentRequest(@in, Id), ct);
    }

    private async Task<IResult> GetByIdAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute] int Id, CT ct)
        => await mtrHlpr.ToResultAsync(new GetCustomerDepartmentByIdRequest(Id), ct);

    private async Task<IResult> GetAllAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
    {
        GetDepartmentsIn @in = new()
        {
            UserType = Convert.ToInt32(cp.FindFirstValue("UserType")),
            UserId = Convert.ToInt32(cp.CustomerId())
        };
        return await mtrHlpr.ToResultAsync(new GetDepartmentsRequest(@in), ct);
    }

    #endregion
}
