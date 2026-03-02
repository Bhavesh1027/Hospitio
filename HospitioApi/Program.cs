global using CT = System.Threading.CancellationToken;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Hospitio.BackGroundService.Receiver;
using Hospitio.BackGroundService.Sender;
using Hospitio.BackGroundService.TaxiTransfer;
using Hospitio.BackGroundService.Vonage;
using HospitioApi;
using HospitioApi.Authorization;
using HospitioApi.BackGroundService.Receiver;
using HospitioApi.BackGroundService.Sender;
using HospitioApi.Core.HandleAccount.Commands.Login;
using HospitioApi.Core.Options;
using HospitioApi.Core.RabbitMQ;
using HospitioApi.Core.Services.BackGroundServiceData;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.InstanceGuid;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Core.Services.Language;
using HospitioApi.Core.Services.LanguageTranslator;
using HospitioApi.Core.Services.PiiSerializer;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Core.Services.UserFiles;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Data.MultiTenancy;
using HospitioApi.Endpoints;
using HospitioApi.Helpers;
using HospitioApi.Middleware;
using HospitioApi.Pipeline;
using HospitioApi.Shared;
using HospitioApi.Shared.Constants;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;
using Vonage;

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .ConfigureLogging((hostingContext, loggingBuilder) =>
    {
        /**
         * These are already added by default by the framework
         * https://github.com/dotnet/aspnetcore/blob/774b2af72b7337789e937546d39d97e0796415cd/src/DefaultBuilder/src/WebHost.cs#L192
         * loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
         * loggingBuilder.AddConsole();
         * loggingBuilder.AddDebug();
         * loggingBuilder.AddEventSourceLogger();
         */
        if (!hostingContext.HostingEnvironment.IsDevelopment()) /** If in Azure */
        {
            loggingBuilder.ClearProviders(); /** Clear all preexisting providers and only add the ones needed in Azure */
            loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            loggingBuilder.AddAzureWebAppDiagnostics(); /** Adds Azure logging capability under 'Monitoring' > 'App Service Logs' */
        }
    })
    .ConfigureServices(services => services.Configure<AzureBlobLoggerOptions>(options =>
    {
        options.IncludeScopes = true;
    }));

var Configuration = builder.Configuration;


//DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var enableSensitiveDataLogging = Configuration.GetSection("EFSettings").GetValue<bool>("EnableSensitiveDataLogging");
    var enableDetailedErrors = Configuration.GetSection("EFSettings").GetValue<bool>("EnableDetailedErrors");

    options
        .UseSqlServer(
            Configuration.GetConnectionString("HospitioConnection"),
            sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                sqlServerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            })
        .ConfigureWarnings(w => { /** Configure warnings here */ })
        .EnableSensitiveDataLogging(enableSensitiveDataLogging)
        /**
         * For performance reasons, DetailedErrors is Disabled by default
         * https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/extensions-logging?tabs=v3#detailed-query-exceptions
         */
        .EnableDetailedErrors(enableDetailedErrors);
});

builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(LoginHandler).Assembly);

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();


/** Get Languages from Third Party */
var thirdPartyAPIUrlSection = Configuration.GetSection(ThirdPartyAPIUrlOptions.ThirdPartyAPIUrl);
var thirdPartyAPIUrl = thirdPartyAPIUrlSection.Get<ThirdPartyAPIUrlOptions>();


/** Get RabbitMQ Settings*/
var rabbitMQSettingsOptions = Configuration.GetSection(RabbitMQSettingsOptions.RabbitMQSettings);
var rabbitMQSettings = rabbitMQSettingsOptions.Get<RabbitMQSettingsOptions>();

/** Get Vonage Settings*/
var vonageSettingsOptions = Configuration.GetSection(VonageSettingsOptions.VonageSettings);
var vonageSettings = vonageSettingsOptions.Get<VonageSettingsOptions>();

var frontEndLinksSettingsOptions = Configuration.GetSection(FrontEndLinksSettingsOptions.FrontEndLinksSettings);
var frontEndLinksSettings = frontEndLinksSettingsOptions.Get<FrontEndLinksSettingsOptions>();

/** Mediatr Pipeline Behaviors (Order is important) */
builder.Services
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>))
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(ShallowValidationPipelineBehavior<,>))
    .AddScoped(typeof(IPipelineBehavior<,>), typeof(DbTransactionPipelineBehavior<,>));


builder.Services.AddAuthorization(options =>
{
    #region SetUp Policy For Admin

    #region Customer
    options.AddPolicy(PolicyTypes.Customer.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Customer.IsView); });
    options.AddPolicy(PolicyTypes.Customer.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Customer.IsEdit); });
    options.AddPolicy(PolicyTypes.Customer.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Customer.IsUpload); });
    options.AddPolicy(PolicyTypes.Customer.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Customer.IsReply); });
    options.AddPolicy(PolicyTypes.Customer.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Customer.IsSend); });
    #endregion

    #region CustomerProfile
    options.AddPolicy(PolicyTypes.CustomerProfile.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.CustomerProfile.IsView); });
    options.AddPolicy(PolicyTypes.CustomerProfile.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.CustomerProfile.IsEdit); });
    options.AddPolicy(PolicyTypes.CustomerProfile.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.CustomerProfile.IsUpload); });
    options.AddPolicy(PolicyTypes.CustomerProfile.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.CustomerProfile.IsReply); });
    options.AddPolicy(PolicyTypes.CustomerProfile.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.CustomerProfile.IsSend); });
    #endregion

    #region AccountSettings
    options.AddPolicy(PolicyTypes.AccountSettings.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.AccountSettings.IsView); });
    options.AddPolicy(PolicyTypes.AccountSettings.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.AccountSettings.IsEdit); });
    options.AddPolicy(PolicyTypes.AccountSettings.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.AccountSettings.IsUpload); });
    options.AddPolicy(PolicyTypes.AccountSettings.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.AccountSettings.IsReply); });
    options.AddPolicy(PolicyTypes.AccountSettings.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.AccountSettings.IsSend); });
    #endregion

    #region eKeys
    options.AddPolicy(PolicyTypes.eKeys.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.eKeys.IsView); });
    options.AddPolicy(PolicyTypes.eKeys.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.eKeys.IsEdit); });
    options.AddPolicy(PolicyTypes.eKeys.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.eKeys.IsUpload); });
    options.AddPolicy(PolicyTypes.eKeys.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.eKeys.IsReply); });
    options.AddPolicy(PolicyTypes.eKeys.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.eKeys.IsSend); });
    #endregion

    #region Tickets
    options.AddPolicy(PolicyTypes.Tickets.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Tickets.IsView); });
    options.AddPolicy(PolicyTypes.Tickets.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Tickets.IsEdit); });
    options.AddPolicy(PolicyTypes.Tickets.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Tickets.IsUpload); });
    options.AddPolicy(PolicyTypes.Tickets.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Tickets.IsReply); });
    options.AddPolicy(PolicyTypes.Tickets.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Tickets.IsSend); });
    #endregion

    #region Offers
    options.AddPolicy(PolicyTypes.Offers.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Offers.IsView); });
    options.AddPolicy(PolicyTypes.Offers.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Offers.IsEdit); });
    options.AddPolicy(PolicyTypes.Offers.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Offers.IsUpload); });
    options.AddPolicy(PolicyTypes.Offers.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Offers.IsReply); });
    options.AddPolicy(PolicyTypes.Offers.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Offers.IsSend); });
    #endregion

    #region Upsell
    options.AddPolicy(PolicyTypes.Upsell.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Upsell.IsView); });
    options.AddPolicy(PolicyTypes.Upsell.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Upsell.IsEdit); });
    options.AddPolicy(PolicyTypes.Upsell.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Upsell.IsUpload); });
    options.AddPolicy(PolicyTypes.Upsell.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Upsell.IsReply); });
    options.AddPolicy(PolicyTypes.Upsell.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Upsell.IsSend); });
    #endregion

    #region QA
    options.AddPolicy(PolicyTypes.QA.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.QA.IsView); });
    options.AddPolicy(PolicyTypes.QA.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.QA.IsEdit); });
    options.AddPolicy(PolicyTypes.QA.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.QA.IsUpload); });
    options.AddPolicy(PolicyTypes.QA.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.QA.IsReply); });
    options.AddPolicy(PolicyTypes.QA.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.QA.IsSend); });
    #endregion

    #region Notifications
    options.AddPolicy(PolicyTypes.Notifications.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Notifications.IsView); });
    options.AddPolicy(PolicyTypes.Notifications.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Notifications.IsEdit); });
    options.AddPolicy(PolicyTypes.Notifications.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Notifications.IsUpload); });
    options.AddPolicy(PolicyTypes.Notifications.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Notifications.IsReply); });
    options.AddPolicy(PolicyTypes.Notifications.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Notifications.IsSend); });
    #endregion

    #region AlertSettings
    options.AddPolicy(PolicyTypes.AlertSettings.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.AlertSettings.IsView); });
    options.AddPolicy(PolicyTypes.AlertSettings.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.AlertSettings.IsEdit); });
    options.AddPolicy(PolicyTypes.AlertSettings.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.AlertSettings.IsUpload); });
    options.AddPolicy(PolicyTypes.AlertSettings.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.AlertSettings.IsReply); });
    options.AddPolicy(PolicyTypes.AlertSettings.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.AlertSettings.IsSend); });
    #endregion

    #region JourneySettings
    options.AddPolicy(PolicyTypes.JourneySettings.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.JourneySettings.IsView); });
    options.AddPolicy(PolicyTypes.JourneySettings.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.JourneySettings.IsEdit); });
    options.AddPolicy(PolicyTypes.JourneySettings.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.JourneySettings.IsUpload); });
    options.AddPolicy(PolicyTypes.JourneySettings.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.JourneySettings.IsReply); });
    options.AddPolicy(PolicyTypes.JourneySettings.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.JourneySettings.IsSend); });
    #endregion

    #region EventLogs
    options.AddPolicy(PolicyTypes.EventLogs.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.EventLogs.IsView); });
    options.AddPolicy(PolicyTypes.EventLogs.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.EventLogs.IsEdit); });
    options.AddPolicy(PolicyTypes.EventLogs.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.EventLogs.IsUpload); });
    options.AddPolicy(PolicyTypes.EventLogs.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.EventLogs.IsReply); });
    options.AddPolicy(PolicyTypes.EventLogs.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.EventLogs.IsSend); });
    #endregion

    #region KPIs
    options.AddPolicy(PolicyTypes.KPIs.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.KPIs.IsView); });
    options.AddPolicy(PolicyTypes.KPIs.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.KPIs.IsEdit); });
    options.AddPolicy(PolicyTypes.KPIs.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.KPIs.IsUpload); });
    options.AddPolicy(PolicyTypes.KPIs.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.KPIs.IsReply); });
    options.AddPolicy(PolicyTypes.KPIs.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.KPIs.IsSend); });
    #endregion

    #region LiveChat
    options.AddPolicy(PolicyTypes.LiveChat.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.LiveChat.IsView); });
    options.AddPolicy(PolicyTypes.LiveChat.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.LiveChat.IsEdit); });
    options.AddPolicy(PolicyTypes.LiveChat.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.LiveChat.IsUpload); });
    options.AddPolicy(PolicyTypes.LiveChat.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.LiveChat.IsReply); });
    options.AddPolicy(PolicyTypes.LiveChat.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.LiveChat.IsSend); });
    #endregion

    #region BI
    options.AddPolicy(PolicyTypes.BI.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.BI.IsView); });
    options.AddPolicy(PolicyTypes.BI.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.BI.IsEdit); });
    options.AddPolicy(PolicyTypes.BI.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.BI.IsUpload); });
    options.AddPolicy(PolicyTypes.BI.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.BI.IsReply); });
    options.AddPolicy(PolicyTypes.BI.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.BI.IsSend); });
    #endregion

    #region Financials
    options.AddPolicy(PolicyTypes.Financials.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Financials.IsView); });
    options.AddPolicy(PolicyTypes.Financials.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Financials.IsEdit); });
    options.AddPolicy(PolicyTypes.Financials.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Financials.IsUpload); });
    options.AddPolicy(PolicyTypes.Financials.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Financials.IsReply); });
    options.AddPolicy(PolicyTypes.Financials.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Financials.IsSend); });
    #endregion

    #region Payments
    options.AddPolicy(PolicyTypes.Payments.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Payments.IsView); });
    options.AddPolicy(PolicyTypes.Payments.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Payments.IsEdit); });
    options.AddPolicy(PolicyTypes.Payments.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Payments.IsUpload); });
    options.AddPolicy(PolicyTypes.Payments.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Payments.IsReply); });
    options.AddPolicy(PolicyTypes.Payments.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Payments.IsSend); });
    #endregion

    #region DigitalMarketPlace
    options.AddPolicy(PolicyTypes.DigitalMarketPlace.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.DigitalMarketPlace.IsView); });
    options.AddPolicy(PolicyTypes.DigitalMarketPlace.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.DigitalMarketPlace.IsEdit); });
    options.AddPolicy(PolicyTypes.DigitalMarketPlace.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.DigitalMarketPlace.IsUpload); });
    options.AddPolicy(PolicyTypes.DigitalMarketPlace.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.DigitalMarketPlace.IsReply); });
    options.AddPolicy(PolicyTypes.DigitalMarketPlace.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.DigitalMarketPlace.IsSend); });
    #endregion

    #region Integration
    options.AddPolicy(PolicyTypes.Integration.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Integration.IsView); });
    options.AddPolicy(PolicyTypes.Integration.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Integration.IsEdit); });
    options.AddPolicy(PolicyTypes.Integration.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Integration.IsUpload); });
    options.AddPolicy(PolicyTypes.Integration.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Integration.IsReply); });
    options.AddPolicy(PolicyTypes.Integration.CanSend, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, Permissions.Integration.IsSend); });
    #endregion

    #endregion

    #region SetUp Policy For Customer

    #region Module
    options.AddPolicy(CustomerUserModulePolicyTypes.PermitAdmin, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModulePermissions.Admin); });
    options.AddPolicy(CustomerUserModulePolicyTypes.PermitCommunication, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModulePermissions.Communication); });
    options.AddPolicy(CustomerUserModulePolicyTypes.PermitGuestJourney, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModulePermissions.GuestJourney); });
    options.AddPolicy(CustomerUserModulePolicyTypes.PermitOnlineCheckin, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModulePermissions.OnlineCheckin); });
    options.AddPolicy(CustomerUserModulePolicyTypes.PermitGuestPortal, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModulePermissions.GuestPortal); });
    options.AddPolicy(CustomerUserModulePolicyTypes.PermiteKeys, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModulePermissions.eKeys); });
    options.AddPolicy(CustomerUserModulePolicyTypes.PermitAnalytics, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModulePermissions.Analytics); });
    options.AddPolicy(CustomerUserModulePolicyTypes.PermitUserAndPermissions, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModulePermissions.UserAndPermissions); });
    options.AddPolicy(CustomerUserModulePolicyTypes.PermitIntegrations, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModulePermissions.Integrations); });
    options.AddPolicy(CustomerUserModulePolicyTypes.PermitPricePerRoomOrAppartments, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModulePermissions.PricePerRoomOrAppartments); });
    options.AddPolicy(CustomerUserModulePolicyTypes.PermitMinimumAmount, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModulePermissions.MinimumAmount); });
    options.AddPolicy(CustomerUserModulePolicyTypes.PermitPriceingPeriod, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModulePermissions.PriceingPeriod); });
    #endregion

    #region Module Service
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitSettings, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.Settings); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitDigitalAssistant, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.DigitalAssistant); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitGuestAlerts, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.GuestAlerts); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitPayments, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.Payments); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitWhatsApp, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.WhatsApp); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitWhatsAppBranded, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.WhatsAppBranded); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitTelegram, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.Telegram); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitTelegramBranded, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.TelegramBranded); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitMessenger, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.Messenger); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitSMS, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.SMS); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitEmail, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.Email); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitWebChat, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.WebChat); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitChatBot, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.ChatBot); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitChatGPT, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.ChatGPT); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitMenu, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.Menu); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitPropertyInfo, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.PropertyInfo); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitEnhanceyourStay, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.EnhanceyourStay); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitReception, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.Reception); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitHousekeeping, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.Housekeeping); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitRoomService, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.RoomService); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitConcierge, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.Concierge); });
    options.AddPolicy(CustomerUserModuleServicesPolicyTypes.PermitLocalExperiences, policy => { policy.RequireClaim(CustomClaimTypes.Permission, CustomerUserModuleServicesPermissions.LocalExperiences); });
    #endregion

    #region Guests
    options.AddPolicy(CustomerPolicyTypes.Guests.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Guests.IsView); });
    options.AddPolicy(CustomerPolicyTypes.Guests.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Guests.IsEdit); });
    options.AddPolicy(CustomerPolicyTypes.Guests.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Guests.IsUpload); });
    options.AddPolicy(CustomerPolicyTypes.Guests.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Guests.IsReply); });
    options.AddPolicy(CustomerPolicyTypes.Guests.CanDownload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Guests.IsDownload); });
    #endregion

    #region Communication
    options.AddPolicy(CustomerPolicyTypes.Communication.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Communication.IsView); });
    options.AddPolicy(CustomerPolicyTypes.Communication.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Communication.IsEdit); });
    options.AddPolicy(CustomerPolicyTypes.Communication.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Communication.IsUpload); });
    options.AddPolicy(CustomerPolicyTypes.Communication.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Communication.IsReply); });
    options.AddPolicy(CustomerPolicyTypes.Communication.CanDownload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Communication.IsDownload); });
    #endregion

    #region GuestJourney
    options.AddPolicy(CustomerPolicyTypes.GuestJourney.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.GuestJourney.IsView); });
    options.AddPolicy(CustomerPolicyTypes.GuestJourney.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.GuestJourney.IsEdit); });
    options.AddPolicy(CustomerPolicyTypes.GuestJourney.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.GuestJourney.IsUpload); });
    options.AddPolicy(CustomerPolicyTypes.GuestJourney.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.GuestJourney.IsReply); });
    options.AddPolicy(CustomerPolicyTypes.GuestJourney.CanDownload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.GuestJourney.IsDownload); });
    #endregion

    #region eKeys
    options.AddPolicy(CustomerPolicyTypes.eKeys.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.eKeys.IsView); });
    options.AddPolicy(CustomerPolicyTypes.eKeys.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.eKeys.IsEdit); });
    options.AddPolicy(CustomerPolicyTypes.eKeys.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.eKeys.IsUpload); });
    options.AddPolicy(CustomerPolicyTypes.eKeys.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.eKeys.IsReply); });
    options.AddPolicy(CustomerPolicyTypes.eKeys.CanDownload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.eKeys.IsDownload); });
    #endregion

    #region Analytics
    options.AddPolicy(CustomerPolicyTypes.Analytics.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Analytics.IsView); });
    options.AddPolicy(CustomerPolicyTypes.Analytics.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Analytics.IsEdit); });
    options.AddPolicy(CustomerPolicyTypes.Analytics.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Analytics.IsUpload); });
    options.AddPolicy(CustomerPolicyTypes.Analytics.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Analytics.IsReply); });
    options.AddPolicy(CustomerPolicyTypes.Analytics.CanDownload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Analytics.IsDownload); });
    #endregion

    #region HelpCenter
    options.AddPolicy(CustomerPolicyTypes.HelpCenter.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.HelpCenter.IsView); });
    options.AddPolicy(CustomerPolicyTypes.HelpCenter.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.HelpCenter.IsEdit); });
    options.AddPolicy(CustomerPolicyTypes.HelpCenter.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.HelpCenter.IsUpload); });
    options.AddPolicy(CustomerPolicyTypes.HelpCenter.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.HelpCenter.IsReply); });
    options.AddPolicy(CustomerPolicyTypes.HelpCenter.CanDownload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.HelpCenter.IsDownload); });
    #endregion

    #region Admin
    options.AddPolicy(CustomerPolicyTypes.Admin.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Admin.IsView); });
    options.AddPolicy(CustomerPolicyTypes.Admin.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Admin.IsEdit); });
    options.AddPolicy(CustomerPolicyTypes.Admin.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Admin.IsUpload); });
    options.AddPolicy(CustomerPolicyTypes.Admin.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Admin.IsReply); });
    options.AddPolicy(CustomerPolicyTypes.Admin.CanDownload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.Admin.IsDownload); });
    #endregion

    #region OnlineCheckin
    options.AddPolicy(CustomerPolicyTypes.OnlineCheckin.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.OnlineCheckin.IsView); });
    options.AddPolicy(CustomerPolicyTypes.OnlineCheckin.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.OnlineCheckin.IsEdit); });
    options.AddPolicy(CustomerPolicyTypes.OnlineCheckin.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.OnlineCheckin.IsUpload); });
    options.AddPolicy(CustomerPolicyTypes.OnlineCheckin.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.OnlineCheckin.IsReply); });
    options.AddPolicy(CustomerPolicyTypes.OnlineCheckin.CanDownload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.OnlineCheckin.IsDownload); });
    #endregion

    #region GuestPortal
    options.AddPolicy(CustomerPolicyTypes.GuestPortal.CanView, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.GuestPortal.IsView); });
    options.AddPolicy(CustomerPolicyTypes.GuestPortal.CanEdit, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.GuestPortal.IsEdit); });
    options.AddPolicy(CustomerPolicyTypes.GuestPortal.CanUpload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.GuestPortal.IsUpload); });
    options.AddPolicy(CustomerPolicyTypes.GuestPortal.CanReply, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.GuestPortal.IsReply); });
    options.AddPolicy(CustomerPolicyTypes.GuestPortal.CanDownload, policy =>
    { policy.RequireClaim(CustomClaimTypes.Permission, CustomerPermissions.GuestPortal.IsDownload); });
    #endregion

    #endregion

    /** Apply Authorize policy globally to
     *  minimal api endpoints. Minimal endpoints can override this filter if needed. */
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
      .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
      .RequireAuthenticatedUser()
      .Build();
});

var jwtSettingsSection = Configuration.GetSection(JwtSettingsOptions.JwtSettings);
var jwtSettings = jwtSettingsSection.Get<JwtSettingsOptions>();
var jwtSettingsForGr4vyOptions = Configuration.GetSection(JwtSettingsForGr4vyOptions.JwtSettingsForGr4vy);


/** Configure DI Services. */
builder.Services
    .AddHttpContextAccessor()
    .AddSingleton<IAppInstanceGuid>(x => new AppInstanceGuid(AppInstanceGuidValue.Value))
    .AddSingleton<IHandlerResponseFactory, HandlerResponseFactory>()
    .AddSingleton<AppRsaSecurityKey>(x =>
    {
        /** It's required to register the AppRsaSecurityKey key with depedency injection.
         *  If you don't do this, the RSA instance will be prematurely disposed. */
        var publicKey = jwtSettings.JwtPemPublicKey
            .Replace("-----BEGIN PUBLIC KEY-----", string.Empty)
            .Replace("-----END PUBLIC KEY-----", string.Empty);
        RSA rsa = RSA.Create();
        rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(publicKey), out _);
        return new(rsa) { CryptoProviderFactory = new() { CacheSignatureProviders = false } };
    })
    .AddScoped<IMediatorHelper, MediatorHelper>()
    .AddScoped<IJwtService, JwtService>()
    .AddScoped<IPiiSerializer, PiiSerializer>()
    .AddScoped<ITenantService, TenantService>()
    .AddScoped<MultiTenantServiceMiddleware>()
     .AddScoped<IUserFilesService, UserFilesService>()
    .AddSingleton<IDapperRepository, DapperRepository>()
    .AddScoped<ICommonDataBaseOprationService, CommonDataBaseOprationService>()
    .AddScoped<ILanguageService, LanguageService>()
    .AddTransient<IChatService, ChatService>()
    .AddScoped<IVonageService, VonageService>()
    .AddScoped<ITokenGenerator, Jwt>()
    .AddSingleton<ILanguageTranslatorService, LanguageTranslatorService>()
    .AddSingleton<IBackGroundServiceData, BackGroundServiceData>()
    .AddScoped<ISendEmail, SendEmail>()
    .AddSingleton<IRabbitMQClient, RabbitMQClient>()
    .AddTransient<ChatHub>()
    .Configure<JwtSettingsOptions>(jwtSettingsSection) /** https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-5.0 */
    .Configure<JwtSettingsForGr4vyOptions>(jwtSettingsForGr4vyOptions)
    .Configure<ThirdPartyAPIUrlOptions>(Configuration.GetSection(ThirdPartyAPIUrlOptions.ThirdPartyAPIUrl))
    .Configure<RabbitMQSettingsOptions>(Configuration.GetSection(RabbitMQSettingsOptions.RabbitMQSettings))
    .Configure<VonageSettingsOptions>(Configuration.GetSection(VonageSettingsOptions.VonageSettings))
    .Configure<FrontEndLinksSettingsOptions>(Configuration.GetSection(FrontEndLinksSettingsOptions.FrontEndLinksSettings))
    .Configure<ChatWidgetLinksSettingsOptions>(Configuration.GetSection(ChatWidgetLinksSettingsOptions.ChatWidgetLinksSettings))
    .Configure<Gr4vyApiSettingsOptions>(Configuration.GetSection(Gr4vyApiSettingsOptions.Gr4vyApiSettings))
    .Configure<MiddlewareApiSettingsOptions>(Configuration.GetSection(MiddlewareApiSettingsOptions.MiddlewareApiSettings))
    .Configure<HospitioApiStorageAccountOptions>(Configuration.GetSection(HospitioApiStorageAccountOptions.HospitioApiStorageAccount))
    .Configure<SMTPEmailSettingsOptions>(Configuration.GetSection(SMTPEmailSettingsOptions.SMTPEmailSettings))
    .Configure<AzurLanguageTranslatorSettingsOptions>(Configuration.GetSection(AzurLanguageTranslatorSettingsOptions.AzurLanguageTranslatorSettings))
    .Configure<ResetPasswordSettingsOptions>(Configuration.GetSection(ResetPasswordSettingsOptions.ResetPasswordSettings))
    .Configure<CustomerCredentialSendEmailOptions>(Configuration.GetSection(CustomerCredentialSendEmailOptions.CustomerCredentialEmailSettings))
    .Configure<VonageTemplateEmailSettingsOptions>(Configuration.GetSection(VonageTemplateEmailSettingsOptions.VonageTemplateEmailSettings))
    .Configure<EndpointSettings>(Configuration.GetSection(EndpointSettings.CenturionEndpointSettings))
    .Configure<MusementSettingsOptions>(Configuration.GetSection(MusementSettingsOptions.MusementSettings))
    .Configure<BackGroundServicesSettingsOptions>(Configuration.GetSection(BackGroundServicesSettingsOptions.BackGroundServicesSettings))
    .Configure<CustomerGuestCheckInFormSendPDftoEmailSettingsOptions>(Configuration.GetSection(CustomerGuestCheckInFormSendPDftoEmailSettingsOptions.CustomerGuestCheckInFormSendPDftoEmailSettings))
    .Configure<CenturionAPIGetTokenCredentialOptions>(Configuration.GetSection(CenturionAPIGetTokenCredentialOptions.CenturionAPITokenCredentials))
    .Configure<WelComePickUpsSettingsOptions>(Configuration.GetSection(WelComePickUpsSettingsOptions.WelComePickUpsSettings));

builder.Services
    .AddAuthentication(auth =>
    {
        auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        /** Configure Jwt Bearer Token when receiving the token from request */
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
        var issuerSigningKey = builder.Services.BuildServiceProvider().GetRequiredService<AppRsaSecurityKey>();
#pragma warning restore ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'
        options.IncludeErrorDetails = true;
        options.Audience = jwtSettings.Audience;
        options.ClaimsIssuer = jwtSettings.Issuer;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = jwtSettings.Audience,
            ValidIssuer = jwtSettings.Issuer,
            IssuerSigningKey = issuerSigningKey,
            CryptoProviderFactory = new() { CacheSignatureProviders = false },
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new()
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Query["token"];
                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(token) &&
                  (path.StartsWithSegments("/hub/signalr")))
                {
                    // Read the token out of the query string
                    context.Token = token;
                }

                return Task.CompletedTask;
            },
            OnForbidden = async context =>
            {
                if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden
                && (context.Response.ContentLength is null || context.Response.ContentLength == 0))
                {
                    context.Response.ContentType = "application/json";
                    var json = JsonSerializer.Serialize(
                        new BaseResponseOut("You don't have the required roles to perform this action."),
                        new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                    await context.Response.WriteAsync(json);
                }
            }
        };
    });


/** Configure Swagger. */
builder.Services.AddSwaggerGen(swaggerOptions =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        },
        Description = @"<h5>
            JWT Authorization header using 'Bearer {token}'.
            <br><br>Enter only the {token}.
            <br>The 'Bearer ' prefix will be added automatically.
        </h5>",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    };

    swaggerOptions.CustomSchemaIds(type => $"{type}");
    swaggerOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "HospitioApi", Version = "v1" });
    swaggerOptions.AddSecurityDefinition("Bearer", securityScheme);
    swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, new List<string>() } });
    swaggerOptions.TagActionsBy(api =>
    {
        var groupName = (api.GroupName ?? api.RelativePath!)
            .Replace("-", " ")
            .Split('/')
            .Aggregate((a, b) => $"{a} > {b}");
        return new[] { groupName };
    });
    swaggerOptions.DocInclusionPredicate((name, api) => true);
    swaggerOptions.OperationFilter<SwaggerFileOperationFilter>();
});

builder.Services.RegisterEndpointsModules(); /** Minimal endpoints */
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSignalR();
//builder.Services.AddSignalR().AddAzureSignalR(Configuration["Azure:SignalR:ConnectionString"]);

#region HttpClient

builder.Services.AddHttpClient("Centurion", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Endpoints:Centurion:Url"]);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
#endregion


//For Now have applied for both the server IIS & Kestrel 
builder.Services.Configure<IISServerOptions>(options =>
{
    //Set the Size of Limit in future
    options.MaxRequestBodySize = null;
});
builder.Services.Configure<KestrelServerOptions>(options =>
{
    //Set the Size of Limit in future
    options.Limits.MaxRequestBodySize = null;
});

#region  BackGround Service
/** BackGround Service Settings */
var backGroundServicesSettingsOption = Configuration.GetSection(BackGroundServicesSettingsOptions.BackGroundServicesSettings);
var backGroundServicesSettings = backGroundServicesSettingsOption.Get<BackGroundServicesSettingsOptions>();

if (backGroundServicesSettings != null && backGroundServicesSettings.CustomerQueueEnabled)
{
    builder.Services.AddHostedService<CustomerQueue>();
}
if (backGroundServicesSettings != null && backGroundServicesSettings.CustomerQueueConsumerEnabled)
{
    builder.Services.AddHostedService<CustomerQueueConsume>();
}
if (backGroundServicesSettings != null && backGroundServicesSettings.GuestMessageConsumerEnabled)
{
    builder.Services.AddHostedService<GuestMessageConsume>();
}
builder.Services.AddHostedService<ConnectionDefinationsBackgroundService>();
builder.Services.AddHostedService<PaymentServiceDefinitionByIdBackgroundService>();
builder.Services.AddHostedService<TemplateStatusCheckService>();
builder.Services.AddHostedService<ReceiveWPMessageConsume>();
builder.Services.AddHostedService<SendAlertMessageBackGroundService>();
if (backGroundServicesSettings != null && backGroundServicesSettings.TaxiTransferStatusCheckServiceEnabled)
{
    builder.Services.AddHostedService<TaxiTransferStatusCheckService>();
}
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.ConfigurePII(Configuration)
    .AddNoCacheHeaders()
    .ConfigureCors(app.Environment)
    .ConfigureSwagger(app.Environment)
    .ConfigureErrorResponses()
    .ConfigureHsts(app.Environment)
    .UseHttpsRedirection()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

app.UseMiddleware<MultiTenantServiceMiddleware>();

//app.MapRazorPages();
app.MapMinimalEndpoints();
//app.MapMinimalEndpoints().UseEndpoints(option=> option.MapHub<ChatHub>("/chat"));

//Register SignalR Endpoint
//app.UseAzureSignalR(option => { option.MapHub<ChatHub>("/chat"); });
app.MapHub<ChatHub>("/chat");

app.Run();


namespace HospitioApi
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder ConfigurePII(this IApplicationBuilder app, IConfiguration Configuration)
        {
            IdentityModelEventSource.ShowPII = Configuration.GetValue<bool>("LoggingShowPII");
            return app;
        }

        public static IApplicationBuilder AddNoCacheHeaders(this IApplicationBuilder app)
        {
            return app.Use((context, next) =>
            {
                context.Response.Headers.Add("cache-control", "no-cache, no-store, must-revalidate");
                context.Response.Headers.Add("pragma", "no-cache");
                context.Response.Headers.Add("expires", "0");
                return next(context);
            });
        }

        //public static IApplicationBuilder ConfigureCors(this IApplicationBuilder app, IWebHostEnvironment env)
        //{
        //    return app.UseCors(builder =>
        //    {
        //        builder.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:3002", "https://hospitio-dev.appdemoserver.com", "https://hospitio-customer-dev.appdemoserver.com", "https://hospitio-guest-dev.appdemoserver.com", "https://192.168.13.30:7018", "https://middleware.hospitio.com")
        //            .AllowAnyHeader()
        //            .AllowAnyMethod()
        //            .AllowCredentials().WithExposedHeaders("Content-Disposition", "file-id", "inboxuser-id").SetIsOriginAllowedToAllowWildcardSubdomains();
        //    });
        //}

        public static IApplicationBuilder ConfigureCors(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            return app.UseCors(builder =>
            {
                builder.SetIsOriginAllowed(origin => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithExposedHeaders("Content-Disposition", "file-id", "inboxuser-id");
                    //.SetIsOriginAllowedToAllowWildcardSubdomains();
            });
        }

        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsEnvironment("PROD"))
            {
                return app.UseSwagger().UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HospitioApi");
                    c.DocExpansion(DocExpansion.None);
                    c.EnableFilter();
                });
            }
            else
            {
                return app;
            }
        }

        public static IApplicationBuilder ConfigureErrorResponses(this IApplicationBuilder app)
        {
            return app.UseExceptionHandler($"/api/error/{AppInstanceGuidValue.Value}");
        }

        public static IApplicationBuilder ConfigureHsts(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            /** The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts. */
            return env.IsDevelopment()
                ? app
                : app.UseHsts();
        }
    }

    internal static class AppInstanceGuidValue
    {
        public static string Value { get; } = Guid.NewGuid().ToString();
    }

    internal class AppRsaSecurityKey : RsaSecurityKey
    {
        public AppRsaSecurityKey(RSA rsa) : base(rsa) { }
    }
}