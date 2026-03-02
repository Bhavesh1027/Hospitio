using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleAccount.Commands.ChangeCustomerPassword;
using HospitioApi.Core.HandleAccount.Commands.CustomerLogin;
using HospitioApi.Core.HandleAccount.Commands.CustomerRefreshToken;
using HospitioApi.Core.HandleAccount.Commands.CustomerRevokeToken;
using HospitioApi.Core.HandleAccount.Commands.ResetPassword;
using HospitioApi.Core.HandleAccount.Commands.ResetPasswordConfirmation;
using HospitioApi.Helpers;

namespace HospitioApi.Endpoints.HospitioCustomer;

public class AccountEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(singular: "api/hospitio-customer/account");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {

        app.MapPost($"/{Route.Singular}/login", LoginAsync)
            .AllowAnonymous(),
          app.MapPost($"/{Route.Singular}/refresh-token", CustomerRefreshTokenAsync)
            .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/revoke-token", CustomerRevoketokenAsync)
            .AllowAnonymous(),
         app.MapPost($"/{Route.Singular}/"+"change-password/{customerId}", ChangePasswordAsync)
            .RequireAuthorization(),
         app.MapPost($"/{Route.Singular}/reset-password-confirmation", ResetPasswordConfirmationAsync)
            .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/reset-password", ResetPasswordAsync)
            .AllowAnonymous(),
    };

    #region Delegates

    private async Task<IResult> LoginAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CustomerLoginIn @in , CT ct)
        => await mtrHlpr.ToResultAsync(new CustomerLoginHandlerRequest(@in), ct);
    private async Task<IResult> CustomerRefreshTokenAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CustomerRefreshTokenIn @in, CT ct)
       => await mtrHlpr.ToResultAsync(new CustomerRefreshTokenHandlerRequest(@in), ct);
    private async Task<IResult> CustomerRevoketokenAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] CustomerRevokeTokenIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new CustomerRevokeTokenHandlerRequest(@in), ct);
    private async Task<IResult> ChangePasswordAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute(Name = "customerId")] int customerId, [FromBody] ChangeCustomerPasswordIn @in, CT ct)
     => await mtrHlpr.ToResultAsync(new ChangeCustomerPasswordHandlerRequest(@in, customerId), ct);

    private async Task<IResult> ResetPasswordConfirmationAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] ResetPasswordConfirmationIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new ResetPasswordConfirmationHandlerRequest(@in), ct);

    private async Task<IResult> ResetPasswordAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] ResetPasswordIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new ResetPasswordHandlerRequest(@in), ct);

    #endregion
}