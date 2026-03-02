using Azure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using HospitioApi.Core.Options;
using System.Reflection;

namespace HospitioApi.Endpoints.SystemAdmin;

public class EnvironmentEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(singular: "api/system-admin/environment");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[] {

        app.MapGet($"/{Route.Singular}", GetEnvironment)
            .AllowAnonymous(),

        app.MapGet($"/{Route.Singular}" + "/typescript-file", GetEnvironmentTypescriptAsync)
            //.RequireAuthorization(Policy.Admins_Only)
            .AllowAnonymous()
    };

    #region Delegates
    private IResult GetEnvironment(IWebHostEnvironment env, IConfiguration configuration, IOptions<JwtSettingsOptions> jwtSettings, CT ct)
    {
        return Results.Ok(new
        {
            env.EnvironmentName,
            UsingSecretsJson = configuration.GetValue<string?>("UsingSecretsJson"),
            jwtSettings.Value.JwtValidForMinutes,
            jwtSettings.Value.RefreshTokenValidForHours
        });
    }

    private async Task<IResult> GetEnvironmentTypescriptAsync([FromServices] IMediator _mediator, CT ct)
    {
        var filename = "Contracts.ts";
        var typescriptFileContentType = "application/x-typescript";
        var memoryStream = new MemoryStream();
        var hospitioApiDirectoryPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location) ?? throw new Exception();
        hospitioApiDirectoryPath = Path.GetFullPath(Path.Combine(hospitioApiDirectoryPath, filename));

        try
        {
            var fileContent = await File.ReadAllTextAsync(hospitioApiDirectoryPath, ct);
            var writer = new StreamWriter(memoryStream);
            writer.Write(fileContent);
            writer.Flush();
        }
        catch (RequestFailedException e)
        {
            if (e.ErrorCode == "FileNotFound")
            {
                return Results.NotFound();
            }
        }
        memoryStream.Position = 0;
        return Results.File(memoryStream, typescriptFileContentType, filename);
    }
    #endregion
}
