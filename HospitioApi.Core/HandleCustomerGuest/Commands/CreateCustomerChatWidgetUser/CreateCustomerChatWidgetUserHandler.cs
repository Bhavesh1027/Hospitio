using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.CreateCustomerChatWidgetUser;
public record CreateCustomerChatWidgetUserRequest(CreateCustomerChatWidgetUserIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerChatWidgetUserHandler : IRequestHandler<CreateCustomerChatWidgetUserRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly JwtSettingsOptions _jwtSettings;
    private readonly FrontEndLinksSettingsOptions _frontEndLinksSettings;
    private readonly IVonageService _vonageService;
    private readonly IChatService _chatService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly HospitioApiStorageAccountOptions _hospitioApiStorageAccount;
    public CreateCustomerChatWidgetUserHandler(ApplicationDbContext db, IHandlerResponseFactory response, IOptions<JwtSettingsOptions> jwtSettings, IOptions<FrontEndLinksSettingsOptions> frontEndLinksSettings, IVonageService vonageService, IChatService chatService, IHubContext<ChatHub> hubContext, IOptions<HospitioApiStorageAccountOptions> hospitioApiStorageAccount)
    {
        _db = db;
        _response = response;
        _jwtSettings = jwtSettings.Value;
        _frontEndLinksSettings = frontEndLinksSettings.Value;
        _vonageService = vonageService;
        _chatService = chatService;
        _hubContext = hubContext;
        _hospitioApiStorageAccount = hospitioApiStorageAccount.Value;
    }
    public async Task<AppHandlerResponse> Handle(CreateCustomerChatWidgetUserRequest request, CancellationToken cancellationToken)
    {
        var CustomerId = CryptoExtension.Decrypt(request.In.EncryptedCustomerId, (UserTypeEnum.Customer).ToString());

        var chatWidgetUser = new ChatWidgetUser
        {
            CustomerId = int.Parse(CustomerId),
        };

        await _db.ChatWidgetUsers.AddAsync(chatWidgetUser);
        await _db.SaveChangesAsync(cancellationToken);

        chatWidgetUser.ChatWidgetUserToken = GenerateToken(CustomerId.ToString(), chatWidgetUser.Id);
        await _db.SaveChangesAsync(cancellationToken);

        string Link = Uri.EscapeDataString(chatWidgetUser.ChatWidgetUserToken);

        var customer = await _db.Customers.Where(c => c.Id == int.Parse(CustomerId)).FirstOrDefaultAsync(cancellationToken);
        var CustomerUser = await _db.CustomerUsers.Where(s => s.CustomerId == int.Parse(CustomerId) && s.CustomerLevelId == 1).FirstOrDefaultAsync(cancellationToken);

        var createdCustomerChatWidgetUserOut = new CreatedCustomerChatWidgetUserOut
        {
            Link = Link,
            CustomerUserId = CustomerUser.Id,
            WidgetUserId = chatWidgetUser.Id,
            Cname = customer?.Cname,
            BusinessName = customer?.BusinessName,
        };

        return _response.Success(new CreateCustomerChatWidgetUserOut("Create customer chat widget user successful.", createdCustomerChatWidgetUserOut));
    }
    public string GenerateToken(string customerId, int customerGuestId)
    {
        var utcNow = DateTime.UtcNow;
        using RSA rsaFromPrivateKey = RSA.Create();
        rsaFromPrivateKey.ImportFromPem(_jwtSettings.JwtPemPrivateKey);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsaFromPrivateKey), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new() { CacheSignatureProviders = false }
        };

        var IssuedAt = new DateTimeUtcUnixEpoch(utcNow);

        var claims = new List<Claim>
        {
             new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String),
             new(JwtRegisteredClaimNames.Iat, IssuedAt.UnixEpoch.ToString(), ClaimValueTypes.Integer64),
             //new("ReservationId", reservationId.ToString(),ClaimValueTypes.Integer64),
             new("GuestId", customerGuestId.ToString(), ClaimValueTypes.Integer64),
             new("UserId", customerGuestId.ToString(), ClaimValueTypes.Integer64),
             new("CustomerId", customerId.ToString(),ClaimValueTypes.Integer64),
             new("UserType", ((int)UserTypeEnum.ChatWidgetUser).ToString(),ClaimValueTypes.String),
        };

        var jwtSecurityToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
         issuer: _jwtSettings.Issuer,
         audience: _jwtSettings.Audience,
         subject: new ClaimsIdentity(claims),
         notBefore: utcNow,
         expires: utcNow.Add(TimeSpan.FromDays(30)),
         issuedAt: utcNow,
         signingCredentials: signingCredentials
         );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}
