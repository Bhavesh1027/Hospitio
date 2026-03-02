using Microsoft.AspNetCore.Mvc;
using HospitioApi.Core.HandleAccount.Commands.ChangeUserPassword;
using HospitioApi.Core.HandleAccount.Commands.EncryptPasswords;
using HospitioApi.Core.HandleAccount.Commands.Login;
using HospitioApi.Core.HandleAccount.Commands.RefreshToken;
using HospitioApi.Core.HandleAccount.Commands.ResetPassword;
using HospitioApi.Core.HandleAccount.Commands.ResetPasswordConfirmation;
using HospitioApi.Core.HandleAccount.Commands.RevokeToken;
using HospitioApi.Helpers;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class AccountEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(singular: "api/hospitio-admin/account");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {

        app.MapPost($"/{Route.Singular}/login", LoginAsync)
            .AllowAnonymous(),

         app.MapPost($"/{Route.Singular}/refresh-token", RefreshTokenAsync)
            .AllowAnonymous(),

        app.MapPost($"/{Route.Singular}/revoke-token", RevoketokenAsync)
            .AllowAnonymous(),

        //app.MapPost($"/{Route.Singular}/encrypt-password", EncryptPasswordAsync)
        //    .AllowAnonymous(),

        app.MapPost($"/{Route.Singular}/"+"change-password/{userId}", ChangePasswordAsync)
            .RequireAuthorization(),

        app.MapPost($"/{Route.Singular}/reset-password-confirmation", ResetPasswordConfirmationAsync)
            .AllowAnonymous(),

        app.MapPost($"/{Route.Singular}/reset-password", ResetPasswordAsync)
            .AllowAnonymous(),
    };

    #region Delegates
    private async Task<IResult> LoginAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] LoginIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new LoginHandlerRequest(@in), ct);

    private async Task<IResult> RefreshTokenAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] RefreshTokenIn @in, CT ct)
       => await mtrHlpr.ToResultAsync(new RefreshTokenHandlerRequest(@in), ct);

    private async Task<IResult> RevoketokenAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] RevokeTokenIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new RevokeTokenHandlerRequest(@in), ct);

    private async Task<IResult> EncryptPasswordAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] EncryptPasswordsIn @in, CT ct)
       => await mtrHlpr.ToResultAsync(new EncryptPasswordsHandlerRequest(@in), ct);

    private async Task<IResult> ChangePasswordAsync([FromServices] IMediatorHelper mtrHlpr, [FromRoute(Name = "userId")] int userId, [FromBody] ChangeUserPasswordIn @in, CT ct)
      => await mtrHlpr.ToResultAsync(new ChangeUserPasswordHandlerRequest(@in, userId), ct);

    private async Task<IResult> ResetPasswordConfirmationAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] ResetPasswordConfirmationIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new ResetPasswordConfirmationHandlerRequest(@in), ct);

    private async Task<IResult> ResetPasswordAsync([FromServices] IMediatorHelper mtrHlpr, [FromBody] ResetPasswordIn @in, CT ct)
        => await mtrHlpr.ToResultAsync(new ResetPasswordHandlerRequest(@in), ct);


    #endregion
}