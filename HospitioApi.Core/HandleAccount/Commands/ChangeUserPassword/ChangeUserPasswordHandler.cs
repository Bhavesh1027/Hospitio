using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Jwt;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleAccount.Commands.ChangeUserPassword;

public record ChangeUserPasswordHandlerRequest(ChangeUserPasswordIn In, int UserId)
    : IRequest<AppHandlerResponse>;

public class ChangeUserPasswordHandler : IRequestHandler<ChangeUserPasswordHandlerRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public ChangeUserPasswordHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(ChangeUserPasswordHandlerRequest request, CancellationToken cancellationToken)
    {
        var user = await _db.Users.Where(x => x.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);

        if (user != null && user.Password != null)
        {
            var oldEncryptedPassword = CryptoExtension.Encrypt(request.In.OldPassword, user.Id.ToString());

            if (oldEncryptedPassword == user.Password)
            {
                var newEncryptedPassword = CryptoExtension.Encrypt(request.In.NewPassword, user.Id.ToString());

                user.Password = newEncryptedPassword;

                await _db.SaveChangesAsync(cancellationToken);

                return _response.Success(new ChangeUserPasswordOut("New Password Updated Successful"));

            }
        }

        return _response.Error("Old Password Is Invalid.", AppStatusCodeError.Forbidden403, skipEmailNotification: true);
    }
}
