using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using HospitioApi.Core.Options;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Constants;
using HospitioApi.Shared.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace HospitioApi.Core.Services.Jwt;

public class JwtService : IJwtService
{
    private readonly ApplicationDbContext _db;
    private readonly JwtSettingsOptions _jwtSettings;
    private readonly JwtSettingsForGr4vyOptions _jwtSettingsForGr4Vy;
    private readonly ResetPasswordSettingsOptions _resetPasswordSettings;
    public JwtService(
        ApplicationDbContext db,
        IOptions<JwtSettingsOptions> jwtSettings,
        IOptions<JwtSettingsForGr4vyOptions> jwtSettingsForGr4Vy,
        IOptions<ResetPasswordSettingsOptions> reserPasswordSettings
        )
    {
        _db = db;
        _jwtSettings = jwtSettings.Value;
        _jwtSettingsForGr4Vy = jwtSettingsForGr4Vy.Value;
        _resetPasswordSettings = reserPasswordSettings.Value;
    }

    public async Task<AccessToken> GenerateJWTokenAsync(User user, CancellationToken cancellationToken)
    {
        using RSA rsaFromPrivateKey = RSA.Create();
        rsaFromPrivateKey.ImportFromPem(_jwtSettings.JwtPemPrivateKey);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsaFromPrivateKey), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new() { CacheSignatureProviders = false }
        };

        var utcNow = DateTime.UtcNow;
        var claims = await GetClaimsAsync(user, utcNow, cancellationToken);

        /** Create the JWT security token and encode it. */
        var jwtSecurityToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
          issuer: _jwtSettings.Issuer,
          audience: _jwtSettings.Audience,
          subject: new ClaimsIdentity(claims),
          notBefore: utcNow,
          expires: utcNow.Add(TimeSpan.FromMinutes(_jwtSettings.JwtValidForMinutes)),
          issuedAt: utcNow,
          signingCredentials: signingCredentials
          );

        jwtSecurityToken.Header.Add("kid", "default");

        return new AccessToken(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
    }

    public async Task<AccessToken> GenerateHubConnectionJWTokenAsync(User user, CancellationToken cancellationToken)
    {
        using RSA rsaFromPrivateKey = RSA.Create();
        rsaFromPrivateKey.ImportFromPem(_jwtSettings.JwtPemPrivateKey);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsaFromPrivateKey), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new() { CacheSignatureProviders = false }
        };

        var utcNow = DateTime.UtcNow;
        var claims = await GetClaimsAsync(user, utcNow, cancellationToken);

        /** Create the JWT security token and encode it. */
        var jwtSecurityToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
          issuer: _jwtSettings.Issuer,
          audience: _jwtSettings.Audience,
          subject: new ClaimsIdentity(claims),
          notBefore: utcNow,
          expires: utcNow.Add(TimeSpan.FromDays(30)),
          issuedAt: utcNow,
          signingCredentials: signingCredentials
          );

        jwtSecurityToken.Header.Add("kid", "default");

        return new AccessToken(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
    }

    public async Task<AccessToken> GenerateJWTokenForCustomerAsync(CustomerUser customerUser, CancellationToken cancellationToken)
    {
        using RSA rsaFromPrivateKey = RSA.Create();
        rsaFromPrivateKey.ImportFromPem(_jwtSettings.JwtPemPrivateKey);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsaFromPrivateKey), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new() { CacheSignatureProviders = false }
        };

        var utcNow = DateTime.UtcNow;
        var claims = await GetCustomerUserClaimsAsync(customerUser, utcNow, cancellationToken);

        /** Create the JWT security token and encode it. */
        var jwtSecurityToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
          issuer: _jwtSettings.Issuer,
          audience: _jwtSettings.Audience,
          subject: new ClaimsIdentity(claims),
          notBefore: utcNow,
          expires: utcNow.Add(TimeSpan.FromMinutes(_jwtSettings.JwtValidForMinutes)),
          issuedAt: utcNow,
          signingCredentials: signingCredentials
          );

        jwtSecurityToken.Header.Add("kid", "default");

        return new AccessToken(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
    }
    public async Task<AccessToken> GenerateJWTokenForChatHubConnectionCustomerAsync(CustomerUser customerUser, CancellationToken cancellationToken)
    {
        using RSA rsaFromPrivateKey = RSA.Create();
        rsaFromPrivateKey.ImportFromPem(_jwtSettings.JwtPemPrivateKey);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsaFromPrivateKey), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new() { CacheSignatureProviders = false }
        };

        var utcNow = DateTime.UtcNow;
        var claims = await GetCustomerUserClaimsAsync(customerUser, utcNow, cancellationToken);

        /** Create the JWT security token and encode it. */
        var jwtSecurityToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
          issuer: _jwtSettings.Issuer,
          audience: _jwtSettings.Audience,
          subject: new ClaimsIdentity(claims),
          notBefore: utcNow,
          expires: utcNow.Add(TimeSpan.FromDays(30)),
          issuedAt: utcNow,
          signingCredentials: signingCredentials
          );

        jwtSecurityToken.Header.Add("kid", "default");

        return new AccessToken(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
    }
    private async Task<List<Claim>> GetClaimsAsync(User user, DateTime utcNow, CancellationToken cancellationToken)
    {
        var IssuedAt = new DateTimeUtcUnixEpoch(utcNow);

        var claims = new List<Claim>
        {
             new(JwtRegisteredClaimNames.Sub, user.Id.ToString(), ClaimValueTypes.Integer),
             new(JwtRegisteredClaimNames.UniqueName, user.UserName??"", ClaimValueTypes.String),
             new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String),
             new(JwtRegisteredClaimNames.Iat, IssuedAt.UnixEpoch.ToString(), ClaimValueTypes.Integer64),
             new("UserId",user.Id.ToString(),ClaimValueTypes.Integer64),
             new("UserType",((int)UserTypeEnum.Hospitio).ToString(), ClaimValueTypes.String),
             new("UserLevel",user.UserLevel.LevelName.ToString(), ClaimValueTypes.String)
        };

        if (user != null && user.UserLevel != null)
        {
            if (user.UserLevel.NormalizedLevelName == Role.SuperAdmin)
            {
                var permissions = await _db.Permissions.ToListAsync(cancellationToken);

                foreach (var permission in permissions)
                {
                    claims.Add(new Claim(CustomClaimTypes.Permission, permission.NormalizedName + ".IsView"));
                    claims.Add(new Claim(CustomClaimTypes.Permission, permission.NormalizedName + ".IsEdit"));
                    claims.Add(new Claim(CustomClaimTypes.Permission, permission.NormalizedName + ".IsUpload"));
                    claims.Add(new Claim(CustomClaimTypes.Permission, permission.NormalizedName + ".IsReply"));
                    claims.Add(new Claim(CustomClaimTypes.Permission, permission.NormalizedName + ".IsSend"));
                }
            }
            else
            {
                if (user.UsersPermissions.Any())
                {
                    foreach (var claim in user.UsersPermissions)
                    {
                        if (claim.Permission != null && claim.Permission.Name != null)
                        {
                            if (claim.IsView == true)
                                claims.Add(new Claim(CustomClaimTypes.Permission, claim.Permission.NormalizedName + ".IsView"));

                            if (claim.IsEdit == true)
                                claims.Add(new Claim(CustomClaimTypes.Permission, claim.Permission.NormalizedName + ".IsEdit"));

                            if (claim.IsUpload == true)
                                claims.Add(new Claim(CustomClaimTypes.Permission, claim.Permission.NormalizedName + ".IsUpload"));

                            if (claim.IsReply == true)
                                claims.Add(new Claim(CustomClaimTypes.Permission, claim.Permission.NormalizedName + ".IsReply"));

                            if (claim.IsSend == true)
                                claims.Add(new Claim(CustomClaimTypes.Permission, claim.Permission.NormalizedName + ".IsSend"));
                        }
                    }
                }
            }
        }

        return claims;
    }
    private async Task<List<Claim>> GetCustomerUserClaimsAsync(CustomerUser customerUser, DateTime utcNow, CancellationToken cancellationToken)
    {
        var IssuedAt = new DateTimeUtcUnixEpoch(utcNow);

        var claims = new List<Claim>
        {
             new(JwtRegisteredClaimNames.Sub, customerUser.Id.ToString(), ClaimValueTypes.Integer),
             new(JwtRegisteredClaimNames.UniqueName, customerUser.UserName??"", ClaimValueTypes.String),
             new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString(), ClaimValueTypes.String),
             new(JwtRegisteredClaimNames.Iat, IssuedAt.UnixEpoch.ToString(), ClaimValueTypes.Integer64),
             new("UserId",customerUser.Id.ToString(),ClaimValueTypes.Integer64),
             new("CustomerId",customerUser.CustomerId.ToString()!,ClaimValueTypes.Integer64),
             new("UserType",((int)UserTypeEnum.Customer).ToString() , ClaimValueTypes.String),
             new("CustomerUserLevel",customerUser.CustomerLevel.LevelName.ToString(), ClaimValueTypes.String)
        };

        var modules = await _db.Modules.Include(x => x.ModuleServices).ToListAsync(cancellationToken);
        var customerProductModule = await _db.ProductModules.Where(x => customerUser.Customer != null && x.ProductId == customerUser.Customer.ProductId).ToListAsync(cancellationToken);
        var customerProductModuleServices = await _db.ProductModuleServices.Where(x => customerUser.Customer != null && x.ProductId == customerUser.Customer.ProductId).ToListAsync(cancellationToken);


        if(customerUser != null && customerUser.CustomerLevel != null)
        {
            if (customerUser.CustomerLevel.NormalizedLevelName == Role.SuperAdmin)
            {

                foreach (var module in modules)
                {
                    if (customerProductModule != null && customerProductModule.Any(x => x.ModuleId == module.Id && x.IsActive.HasValue && x.IsActive == true))
                    {
                        if (!string.IsNullOrEmpty(module.Name))
                            claims.Add(new Claim(CustomClaimTypes.Permission, module.Name));
                    }

                    if (module.ModuleServices != null && module.ModuleServices.Any())
                    {
                        foreach (var moduleServices in module.ModuleServices)
                        {
                            if (customerProductModuleServices.Any(x => x.ModuleServiceId == moduleServices.Id && x.IsActive.HasValue && x.IsActive == true))
                            {
                                if (!string.IsNullOrEmpty(moduleServices.Name))
                                    claims.Add(new Claim(CustomClaimTypes.Permission, moduleServices.Name));
                            }
                        }
                    }
                }
            }
            else
            {
                if (customerUser.CustomerUsersPermissions.Any())
                {
                    foreach (var claim in customerUser.CustomerUsersPermissions)
                    {
                        if (claim.CustomerPermission != null && claim.CustomerPermission.Name != null)
                        {
                            if (claim.IsView == true)
                                claims.Add(new Claim(CustomClaimTypes.Permission, claim.CustomerPermission.NormalizedName + ".IsView"));

                            if (claim.IsEdit == true)
                                claims.Add(new Claim(CustomClaimTypes.Permission, claim.CustomerPermission.NormalizedName + ".IsEdit"));

                            if (claim.IsUpload == true)
                                claims.Add(new Claim(CustomClaimTypes.Permission, claim.CustomerPermission.NormalizedName + ".IsUpload"));

                            if (claim.IsReply == true)
                                claims.Add(new Claim(CustomClaimTypes.Permission, claim.CustomerPermission.NormalizedName + ".IsReply"));

                            if (claim.IsDownload == true)
                                claims.Add(new Claim(CustomClaimTypes.Permission, claim.CustomerPermission.NormalizedName + ".IsDownload"));
                        }
                    }
                }
            }
        }

        return claims;
    }

    public string GenerateRefreshTokenValue(int userId)
    {
        var size = 64;
        string token = string.Empty;
        var randomNumber = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            token = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(userId.ToString())) + Convert.ToBase64String(randomNumber);
        }

        return token;
    }
    public DateTime GetRefreshTokenExpirationUtc()
    {
        return DateTime.UtcNow.AddHours(_jwtSettings.RefreshTokenValidForHours);
    }

    public DateTime GetHubConnectionTokenExpirationUtc()
    {
        return DateTime.UtcNow.AddDays(30);
    }

    public string GenerateJWTokenForGr4vy()
    {
        using ECDsa ecdsaFromPrivateKey = ECDsa.Create();
        ecdsaFromPrivateKey.ImportFromPem(_jwtSettingsForGr4Vy.JwtPemPrivateKey);

        var signingCredentials = new SigningCredentials(new ECDsaSecurityKey(ecdsaFromPrivateKey), SecurityAlgorithms.EcdsaSha512)
        {
            CryptoProviderFactory = new() { CacheSignatureProviders = false }
        };

        var utcNow = DateTime.UtcNow;
        var claims = new List<Claim>()
        {
            new Claim(CustomClaimTypes.Scopes, "merchant-accounts.write"),
            new Claim(CustomClaimTypes.Scopes, "merchant-accounts.read"),
            new Claim(CustomClaimTypes.Scopes, "buyers.read"),
            new Claim(CustomClaimTypes.Scopes, "buyers.write"),
            new Claim(CustomClaimTypes.Scopes, "connections.read"),
            new Claim(CustomClaimTypes.Scopes, "connections.write"),
            new Claim(CustomClaimTypes.Scopes, "payment-services.read"),
            new Claim(CustomClaimTypes.Scopes, "payment-services.write"),
            new Claim(CustomClaimTypes.Scopes, "transactions.read"),
            new Claim(CustomClaimTypes.Scopes, "transactions.write"),
            new Claim(CustomClaimTypes.Scopes, "digital-wallets.read"),
            new Claim(CustomClaimTypes.Scopes, "digital-wallets.write"),
            new Claim(CustomClaimTypes.Scopes, "payment-methods.read"),
            new Claim(CustomClaimTypes.Scopes, "payment-methods.write"),
            new Claim(CustomClaimTypes.Scopes, "payment-method-definitions.read"),
            new Claim(CustomClaimTypes.Scopes, "payment-method-definitions.write"),
            new Claim(CustomClaimTypes.Scopes, "payment-service-definitions.read"),
            new Claim(CustomClaimTypes.Scopes, "payment-service-definitions.write"),
            new Claim(CustomClaimTypes.Scopes, "payment-options.read"),
            new Claim(CustomClaimTypes.Scopes, "payment-options.write"),
            new Claim(CustomClaimTypes.Scopes, "card-scheme-definitions.read"),
            new Claim(CustomClaimTypes.Scopes, "card-scheme-definitions.write"),
            new Claim(CustomClaimTypes.Scopes, "flows.write"),
        };

        var jwtSecurityToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
          subject: new ClaimsIdentity(claims),
          expires: utcNow.Add(TimeSpan.FromMinutes(_jwtSettingsForGr4Vy.JwtValidForMinutes)),
          signingCredentials: signingCredentials
          );

        jwtSecurityToken.Header.Add("kid", _jwtSettingsForGr4Vy.JwtKidKey);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    public string GenerateJWTResetPasswordTokenAsync(User user)
    {
        using RSA rsaFromPrivateKey = RSA.Create();
        rsaFromPrivateKey.ImportFromPem(_jwtSettings.JwtPemPrivateKey);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsaFromPrivateKey), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new() { CacheSignatureProviders = false }
        };

        var utcNow = DateTime.UtcNow;
        var claims = new List<Claim>
        {
             new("Id",user.Id.ToString(),ClaimValueTypes.Integer64),
             new("Email",user.Email.ToString(),ClaimValueTypes.String),
             new("IsUser",true.ToString(), ClaimValueTypes.Boolean)
        };

        /** Create the JWT security token and encode it. */
        var jwtSecurityToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
          subject: new ClaimsIdentity(claims),
          expires: utcNow.Add(TimeSpan.FromHours(_resetPasswordSettings.ExpirationHours)),
          issuedAt: utcNow,
          signingCredentials: signingCredentials
          );

        jwtSecurityToken.Header.Add("kid", "default");

        var accessToken = new AccessToken(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
        string token = accessToken.Jwt;

        return token;
    }
    public string GenerateJWTResetPasswordTokenForCustomerAsync(CustomerUser customerUser)
    {
        using RSA rsaFromPrivateKey = RSA.Create();
        rsaFromPrivateKey.ImportFromPem(_jwtSettings.JwtPemPrivateKey);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsaFromPrivateKey), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new() { CacheSignatureProviders = false }
        };

        var utcNow = DateTime.UtcNow;
        var claims = new List<Claim>
        {
             new("Id",customerUser.Id.ToString(),ClaimValueTypes.Integer64),
             new("Email",customerUser.Email.ToString(),ClaimValueTypes.String),
             new("IsUser",false.ToString(), ClaimValueTypes.Boolean)
        };

        /** Create the JWT security token and encode it. */
        var jwtSecurityToken = new JwtSecurityTokenHandler().CreateJwtSecurityToken(
          subject: new ClaimsIdentity(claims),
          expires: utcNow.Add(TimeSpan.FromHours(_resetPasswordSettings.ExpirationHours)),
          issuedAt: utcNow,
          signingCredentials: signingCredentials
          );

        jwtSecurityToken.Header.Add("kid", "default");

        var accessToken = new AccessToken(new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
        string token = accessToken.Jwt;

        return token;
    }
}
