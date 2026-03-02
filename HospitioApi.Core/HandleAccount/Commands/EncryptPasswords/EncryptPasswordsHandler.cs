using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAccount.Commands.EncryptPasswords;

public record EncryptPasswordsHandlerRequest(EncryptPasswordsIn In)
    : IRequest<AppHandlerResponse>;

public class EncryptPasswordsHandler : IRequestHandler<EncryptPasswordsHandlerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IJwtService _jwtService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EncryptPasswordsHandler(
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

    public async Task<AppHandlerResponse> Handle(EncryptPasswordsHandlerRequest request, CancellationToken cancellationToken)
    {
        var customerUsers = await _db.CustomerUsers.ToListAsync(cancellationToken);
        var users = await _db.Users.ToListAsync(cancellationToken);

        foreach (var customerUser in customerUsers)
        {
            customerUser.Password = CryptoExtension.Encrypt(customerUser.Password, customerUser.CustomerId.ToString());
        }

        _db.UpdateRange(customerUsers);

        foreach (var user in users)
        {
            user.Password = CryptoExtension.Encrypt(user.Password, user.Id.ToString());
        }

        _db.UpdateRange(users);

        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new EncryptPasswordsOut("All Users Password Encrypted Successful."));
    }
}
