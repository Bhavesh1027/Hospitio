using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace HospitioApi.Core.HandleAccount.Commands.ResetPassword;

public record ResetPasswordHandlerRequest(ResetPasswordIn In)
    : IRequest<AppHandlerResponse>;

public class ResetPasswordHandler : IRequestHandler<ResetPasswordHandlerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IJwtService _jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ResetPasswordHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response,
        IJwtService jwtService,
        IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _response = response;
        _jwtService = jwtService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AppHandlerResponse> Handle(ResetPasswordHandlerRequest request, CancellationToken cancellationToken)
    {
        ResetPasswordTokenOut tokenOut = new ResetPasswordTokenOut();
        string returnMessage = "Please add user type.";

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(request.In.Token) as JwtSecurityToken;

        int id = Convert.ToInt32(token.Claims.First(claim => claim.Type == "Id").Value);
        bool isUser = Convert.ToBoolean(token.Claims.First(claim => claim.Type == "IsUser").Value);
        string email = token.Claims.First(claim => claim.Type == "Email").Value;
         
        if (isUser)
        {
            var user = await _db.Users.Include(i => i.UserLevel).FirstOrDefaultAsync(x => x.Id == id && x.Email == email, cancellationToken);

            if (user == null)
            {
                return _response.Error("User not found.", AppStatusCodeError.Forbidden403);
            }

            var encryptedPassword = CryptoExtension.Encrypt(request.In.Password, user.Id.ToString());
            user.Password = encryptedPassword;

            await _db.SaveChangesAsync(cancellationToken);

            tokenOut = await GetToken(isUser, user.Id, user, null, cancellationToken);
            returnMessage = "Login successful.";
        }
        else if (!isUser)
        {
            var customer = await _db.CustomerUsers.Include(i => i.Customer).Include(i => i.CustomerLevel).FirstOrDefaultAsync(x => x.Id == id && x.Email == email, cancellationToken);

            if (customer == null)
            {
                return _response.Error("Customer not found.", AppStatusCodeError.Forbidden403);
            }

            var encryptedPassword = CryptoExtension.Encrypt(request.In.Password, customer.CustomerId.ToString());
            customer.Password = encryptedPassword;

            await _db.SaveChangesAsync(cancellationToken);

            tokenOut = await GetToken(isUser, customer.Id, null, customer, cancellationToken);
            returnMessage = "Customer login successful.";
        }

        return _response.Success(new ResetPasswordOut(returnMessage, id, tokenOut.AccessToken, tokenOut.RefreshToken));
    }

    public async Task<ResetPasswordTokenOut> GetToken(bool isUser, int id, User? user, CustomerUser? customerUser, CancellationToken cancellationToken)
    {
        ResetPasswordTokenOut resetPasswordTokenOut = new ResetPasswordTokenOut();

        var tokenExpiresUtc = _jwtService.GetRefreshTokenExpirationUtc();
        var refreshTokenValue = _jwtService.GenerateRefreshTokenValue(id);

        var ipAddress = _httpContextAccessor.HttpContext.IpAddress();

        if (isUser && user != null)
        {
            var dbUserRefreshToken = new Data.Models.RefreshToken(id, refreshTokenValue, tokenExpiresUtc, ipAddress);
            await _db.RefreshTokens.AddAsync(dbUserRefreshToken, cancellationToken);
            var dtoUserAccessToken = await _jwtService.GenerateJWTokenAsync(user, cancellationToken);
            var hubconnectionExpiresUtc = _jwtService.GetHubConnectionTokenExpirationUtc();
            var dtoChatHubConnectionAccessToken = await _jwtService.GenerateHubConnectionJWTokenAsync(user, cancellationToken);
            var dtoRefreshToken = new ResetPasswordRefreshTokenOut(dbUserRefreshToken.Token, dbUserRefreshToken.Id, new(dbUserRefreshToken.ExpiresUtc));

            var dtoChatHubConnectionToken = new ChatHubConnectionToken(dtoChatHubConnectionAccessToken, new(hubconnectionExpiresUtc));
            resetPasswordTokenOut.AccessToken = dtoUserAccessToken;
            resetPasswordTokenOut.RefreshToken = dtoRefreshToken;
            resetPasswordTokenOut.ChatHubConnectionAccessToken = dtoChatHubConnectionToken;
        }
        else if (!isUser && customerUser != null)
        {
            var dbCustomerRefreshToken = new Data.Models.CustomerUserRefreshToken(customerUser.Id, refreshTokenValue, tokenExpiresUtc, ipAddress);
            await _db.CustomerUserRefreshTokens.AddAsync(dbCustomerRefreshToken, cancellationToken);
            var dtoCustomerAccessToken = await _jwtService.GenerateJWTokenForCustomerAsync(customerUser, cancellationToken);
            var hubconnectionExpiresUtc = _jwtService.GetHubConnectionTokenExpirationUtc();
            var dtoChatHubConnectionAccessToken = await _jwtService.GenerateJWTokenForChatHubConnectionCustomerAsync(customerUser, cancellationToken);
            var dtoRefreshToken = new ResetPasswordRefreshTokenOut(dbCustomerRefreshToken.Token, dbCustomerRefreshToken.Id, new(dbCustomerRefreshToken.ExpiresUtc));

            var dtoChatHubConnectionToken = new ChatHubConnectionToken(dtoChatHubConnectionAccessToken, new(hubconnectionExpiresUtc));
            resetPasswordTokenOut.AccessToken = dtoCustomerAccessToken;
            resetPasswordTokenOut.RefreshToken = dtoRefreshToken;
            resetPasswordTokenOut.ChatHubConnectionAccessToken = dtoChatHubConnectionToken;
        }

        await _db.SaveChangesAsync(cancellationToken);

        return resetPasswordTokenOut;
    }
}