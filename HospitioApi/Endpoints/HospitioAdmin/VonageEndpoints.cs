using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using HospitioApi.Core.HandleVonage.Commands.DeliveryReceiptSMSWebhook;
using HospitioApi.Core.HandleVonage.Commands.InboundWebhook;
using HospitioApi.Core.HandleVonage.Commands.ReceiveSMSWebhook;
using HospitioApi.Core.HandleVonage.Commands.StatusWebook;
using HospitioApi.Core.HandleVonage.Commands.Vonage;
using HospitioApi.Helpers;
using System.Security.Claims;
using Vonage.Utility;

namespace HospitioApi.Endpoints.HospitioAdmin;

public class VonageEndpoints : IEndpointsModule
{
    public AppRoute Route { get; init; } = new(null, singular: "api/hospitio-admin/vonage");

    public RouteHandlerBuilder[] MapEndpoints(WebApplication app) => new[]
    {
        app.MapGet($"/{Route.Singular}",VonageServiceAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/Get",GetVonageServiceAsync)
        .AllowAnonymous(),
        app.MapPost($"/{Route.Singular}/Status",GetVonageServiceStatusAsync)
        .AllowAnonymous(),
         app.MapGet($"/{Route.Singular}/inbound-sms",GetInboundSMSWebHookAsync)
        .AllowAnonymous(),
        app.MapGet($"/{Route.Singular}/sms-delivery-receipt",GetSMSDeliveryReceiptAsync)
        .AllowAnonymous()
    };

    #region Delegate
    private async Task<IResult> VonageServiceAsync([FromServices] IMediatorHelper mtrHlpr, ClaimsPrincipal cp, CT ct)
        => await mtrHlpr.ToResultAsync(new VonageRequest(), ct);
    
    private async Task<IActionResult> GetVonageServiceAsync(HttpContext context, [FromServices] IMediatorHelper mtrHlpr)
    {
        string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        //mtrHlpr.ToResultAsync(new GetInBoundSMSHandlerRequest(context.Request), CancellationToken.None);
        //context.Response.StatusCode = StatusCodes.Status200OK;
        //var mtrHlpr = context.RequestServices.GetService<IMediatorHelper>();
        // Console.WriteLine(context.Request.Query);
        var webhookData = JsonConvert.DeserializeObject<GetInboundWebhookIn>(requestBody);
        //var webhookData = WebhookParser.ParseWebhook<GetInboundWebhookIn>(context.Request.Body, "application/json");
        //GetInboundWebhookIn webhookData = new GetInboundWebhookIn();
        await mtrHlpr.ToResultAsync(new GetInBoundWebhookHandlerRequest(webhookData), CancellationToken.None);
        return new OkResult();
    }

    private async Task<IActionResult> GetVonageServiceStatusAsync(HttpContext context, [FromServices] IMediatorHelper mtrHlpr)
    {
        //string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        //Console.WriteLine(requestBody);
        //return new OkResult();

        var webhookData = WebhookParser.ParseWebhook<GetStatusWebhookIn>(context.Request.Body, "application/json");
        await mtrHlpr.ToResultAsync(new GetStatusWebhookHandlerRequest(webhookData), CancellationToken.None);
        return new OkResult();
    }
    private async Task<IActionResult> GetInboundSMSWebHookAsync(HttpContext context, [FromServices] IMediatorHelper mtrHlpr)
    {
        //string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        //Console.WriteLine(requestBody);
        //return new OkResult();

        var webhookData = WebhookParser.ParseWebhook<ReceiveSMSWebhookIn>(context.Request.Body, "application/json");
        await mtrHlpr.ToResultAsync(new ReceiveSMSWebhookHandlerRequest(webhookData), CancellationToken.None);
        return new OkResult();
    }
    private async Task<IActionResult> GetSMSDeliveryReceiptAsync(HttpContext context, [FromServices] IMediatorHelper mtrHlpr)
    {
        //string requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        //Console.WriteLine(requestBody);
        //return new OkResult();

        var webhookData = WebhookParser.ParseWebhook<GetDeliveryReceiptWebhookIn>(context.Request.Body, "application/json");
        await mtrHlpr.ToResultAsync(new GetDeliveryReceiptWebhookHandlerRequest(webhookData), CancellationToken.None);
        return new OkResult();
    }
    #endregion
}

