using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleUserAccount.Commands.UpdateUserProfile;
public record UpdateUserProfileRequest(UpdateUserProfileIn In)
    : IRequest<AppHandlerResponse>;

public class UpdateUserProfileHandler: IRequestHandler<UpdateUserProfileRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public UpdateUserProfileHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateUserProfileRequest request, CancellationToken cancellationToken)
    {
        if (request.In.UserType == (int)UserTypeEnum.Hospitio)
        {
            var FindUserName = await _db.Users.Where(s => s.Id != request.In.UserId && s.UserName == request.In.UserName).FirstOrDefaultAsync();

            var FindUserEmail = await _db.Users.Where(s => s.Id != request.In.UserId && s.Email == request.In.Email).FirstOrDefaultAsync();
            var userDetail = await _db.Users.Where(e => e.Id == request.In.UserId).FirstOrDefaultAsync(cancellationToken);
            if (FindUserName != null)
            {
                return _response.Error($"The UserName {request.In.UserName} already exists.", AppStatusCodeError.UnprocessableEntity422);
            }
            if (FindUserEmail != null)
            {
                return _response.Error($"The Email {request.In.Email} already exists.", AppStatusCodeError.UnprocessableEntity422);
            }

            var hospitioOnboarding = await _db.HospitioOnboardings.FirstOrDefaultAsync(cancellationToken);

            userDetail.FirstName= request.In.FirstName;
            userDetail.LastName = request.In.LastName;
            userDetail.Email = request.In.Email;
            userDetail.PhoneCountry = request.In.PhoneCountry;
            userDetail.PhoneNumber = request.In.PhoneNumber;
            userDetail.UserName = request.In.UserName;
            userDetail.Title = request.In.Title;
            userDetail.ProfilePicture = request.In.ProfilePicture;
            if (request.In.Password != null)
                userDetail.Password = CryptoExtension.Encrypt(request.In.Password, request.In.UserId.ToString());
            hospitioOnboarding.IncomingTranslationLangage = request.In.IncomingTranslationLangage;
            hospitioOnboarding.NoTranslateWords = request.In.NoTranslateWords;
            if (request.In.TaxiTransferCommission == null)
            {
                return _response.Error($"Taxi Transfer Commission Field is Must Required...", AppStatusCodeError.Forbidden403);
            }
            hospitioOnboarding.TaxiTransCommission = (int)request.In.TaxiTransferCommission;

            await _db.SaveChangesAsync(cancellationToken);
        }
        else if (request.In.UserType == (int)UserTypeEnum.Customer)
        {
            var FindUserName = await _db.CustomerUsers.Where(s => s.Id != request.In.UserId && s.UserName == request.In.UserName).FirstOrDefaultAsync();
            var FindUserEmail = await _db.CustomerUsers.Where(s => s.Id != request.In.UserId && s.Email == request.In.Email).FirstOrDefaultAsync();
            var customerUserDetail = await _db.CustomerUsers.Where(e => e.Id == request.In.UserId).FirstOrDefaultAsync(cancellationToken);
            if (FindUserName != null)
            {
                return _response.Error($"The UserName {request.In.UserName} already exists.", AppStatusCodeError.UnprocessableEntity422);
            }

            if (FindUserEmail != null)
            {
                return _response.Error($"The Email {request.In.Email} already exists.", AppStatusCodeError.UnprocessableEntity422);
            }

            customerUserDetail.FirstName = request.In.FirstName;
            customerUserDetail.LastName = request.In.LastName;
            customerUserDetail.Email = request.In.Email;
            customerUserDetail.PhoneCountry = request.In.PhoneCountry;
            customerUserDetail.PhoneNumber = request.In.PhoneNumber;
            customerUserDetail.UserName = request.In.UserName;
            customerUserDetail.Title = request.In.Title;
            customerUserDetail.ProfilePicture = request.In.ProfilePicture;
            if (request.In.Password != null)
                customerUserDetail.Password = CryptoExtension.Encrypt(request.In.Password, request.In.CustomerId.ToString());

            await _db.SaveChangesAsync(cancellationToken);
        }

        return _response.Success(new UpdateUserProfileOut("Update User Details successfully."));
    }
}

