using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleHospitioOnBoarding.Commands.UpdateHospitioOnBoardings;
public record UpdateHospitioOnBoardingsRequest(UpdateHospitioOnBoardingsIn In) : IRequest<AppHandlerResponse>;
public class UpdateHospitioOnBoardingsHandler : IRequestHandler<UpdateHospitioOnBoardingsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public UpdateHospitioOnBoardingsHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateHospitioOnBoardingsRequest request, CancellationToken cancellationToken)
    {
        if (request.In.Id > 0)
        {
            var hospitioOnBoarding = await _db.HospitioOnboardings.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
            if (hospitioOnBoarding == null)
            {
                return _response.Error($"The hospitio on boarding with given Id {request.In.Id} not exists.", AppStatusCodeError.UnprocessableEntity422);
            }
            hospitioOnBoarding.WhatsappCountry = request.In.WhatsappCountry;
            hospitioOnBoarding.WhatsappNumber = request.In.WhatsappNumber;
            hospitioOnBoarding.ViberCountry = request.In.ViberCountry;
            hospitioOnBoarding.ViberNumber = request.In.ViberNumber;
            hospitioOnBoarding.TelegramCounty = request.In.TelegramCountry;
            hospitioOnBoarding.TelegramNumber = request.In.TelegramNumber;
            hospitioOnBoarding.PhoneCountry = request.In.PhoneCountry;
            hospitioOnBoarding.PhoneNumber = request.In.PhoneNumber;
            hospitioOnBoarding.SmsTitle = request.In.SmsTitle;
            hospitioOnBoarding.Messenger = request.In.Messenger;
            hospitioOnBoarding.Email = request.In.Email;
            hospitioOnBoarding.Cname = request.In.Cname;
            hospitioOnBoarding.IncomingTranslationLangage = request.In.IncomingTranslationLanguage;
            hospitioOnBoarding.NoTranslateWords = request.In.NoTranslateWords;

            await _db.SaveChangesAsync(cancellationToken);

            var updatedhospitioOnBoarding = new UpdatedHospitioOnBoardingsOut()
            {
                Id = hospitioOnBoarding.Id
            };

            return _response.Success(new UpdateHospitioOnBoardingsOut("Update hospitio on boarding successful.", updatedhospitioOnBoarding));
        }
        else
        {
            var hospitioOnboarding = new HospitioOnboarding()
            {
                WhatsappCountry = request.In.WhatsappCountry,
                WhatsappNumber = request.In.WhatsappNumber,
                ViberCountry = request.In.ViberCountry,
                ViberNumber = request.In.ViberNumber,
                TelegramCounty = request.In.TelegramCountry,
                TelegramNumber = request.In.TelegramNumber,
                PhoneCountry = request.In.PhoneCountry,
                PhoneNumber = request.In.PhoneNumber,
                SmsTitle = request.In.SmsTitle,
                Messenger = request.In.Messenger,
                Email = request.In.Email,
                Cname = request.In.Cname,
                IncomingTranslationLangage = request.In.IncomingTranslationLanguage,
                NoTranslateWords = request.In.NoTranslateWords
            };

            await _db.HospitioOnboardings.AddAsync(hospitioOnboarding, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            
            var updatedhospitioOnBoarding = new UpdatedHospitioOnBoardingsOut()
            {
                Id = request.In.Id
            };
            return _response.Success(new UpdateHospitioOnBoardingsOut("Create hospitio on boarding successful.", updatedhospitioOnBoarding));
        }        
    }
}
