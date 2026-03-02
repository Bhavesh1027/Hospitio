using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleGuestJourneyMessagesTemplates.Commands.CreateGuestJourneyMessagesTemplates;
public record CreateGuestJourneyMessagesTemplatesRequest(CreateGuestJourneyMessagesTemplatesIn In) : IRequest<AppHandlerResponse>;
public class CreateGuestJourneyMessagesTemplatesHandler : IRequestHandler<CreateGuestJourneyMessagesTemplatesRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IVonageService _vonageService;
    private readonly VonageSettingsOptions _vonageSettings;
    public CreateGuestJourneyMessagesTemplatesHandler(ApplicationDbContext db, IHandlerResponseFactory response,IVonageService vonageService, IOptions<VonageSettingsOptions> vonageSettings)
    {
        _db = db;
        _response = response;
        _vonageService = vonageService;
        _vonageSettings = vonageSettings.Value;
    }
    public async Task<AppHandlerResponse> Handle(CreateGuestJourneyMessagesTemplatesRequest request, CancellationToken cancellationToken)
    {
        //var checkExist = await _db.GuestJourneyMessagesTemplates.Where(e => e.Name == request.In.Name).FirstOrDefaultAsync(cancellationToken);
        //if (checkExist != null)
        //{
        //    return _response.Error($"The guest journey messages templates {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
        //}

        var guestJourneyMessagesTemplate = new GuestJourneyMessagesTemplate();
        await _db.GuestJourneyMessagesTemplates.AddAsync(guestJourneyMessagesTemplate, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        int uniqeId = guestJourneyMessagesTemplate.Id;
        string stepName = ((JourneyStepTempleteTypeEnums)request.In.TempleteType).ToString();
        #region Comment
        //var templateResponse = await _vonageService.CreateTemplate("5a2659bc-9058-4d79-8d9f-d18c24d24a31", "-----BEGIN PRIVATE KEY-----MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQChLZyclROwPf+satz3AJH2w4dwC5PLsDAvjb2htSJs0QX6cgKUI+A0TGrL1O0x4CjePGvDrlLnxo7ZVjpXgB7NJ2OrQgfQNVdaiVJQI1slwZnnY4xiVC0mQMl958a4FilFaLcrmGkl6VPbzc06+cWGL8ZwnvBSjlYX/U+XUP7ttT7+3sUeDoimPgkGgJzmMDCfoNxXi3DnsrZm6m8oUtUOR06lVx5A7ewoojPJm62KSVtqpnL3jPWxXblR/ErXldhdWLrGI7ADceo0LFR8KagM+6hXxqfBj4DPB3WP3o9qwXaRPZh8GVnKlDxPA9roDDoqZZ0R7O8P+/VbirS7UQGRAgMBAAECggEAMhWnXezhQlnxshU+9q5BrUmTM5kVYy0rvAsyiyZrPR8y2WFGNdx0FixM32waDO6YJH7oCdWIw6cqypSF6pzQdXWw/g21udhpfaPAZVCnSTNA7Os9O2zm3sUxF6PHV3rjdkMU8EIbIoG/4kSwaowk+g6sfmCVU0IRtMCtU9sCbMDwHAJNjnuZ8rn9AopFGt1quckiSavtTBVYCN4eQ8oCXvN7+7ilxGbHhVifTSeXjoC4nxZr9r+TwVgQOqQOl3B6VqK5OVV5p8+EMXmfQeAlwPh3JtVdAX/Nl14B92yx5NHomTnKNSQ2T1wRm7csvxZtuUmNkIJHf4Dh1GC7W//K1QKBgQDVpSVE6skivTrOMoqw88YpYZK3FIPTHR5qzYKRPsTmQj4uSmflwjoKs6hC7Bu0L6QIMHeQkH4YeziZp1VymAMIMJGbhFjRoaIqheoFmmpsqx1JhuIA5pk/y2L/6KzIWbBFqLGqVXxmul+TmEWUNIBDlh6b16ID9wGEj9BkyENVPwKBgQDBIa4ESDlps9CPopRw9GGWBD6xNm5rRR8y5TeOcTZ28xTrHb7Xye5FpJeHukdIyVOKEAVWg9NJJ/e5XvK0jd/y+AfmbLZUFua3FSBe9A6oV7iupeUxv9y1mrIU96AUm698uEu6El6BdKpB3S/VQtYurDiJtK6Nf+02UG2sOI3lLwKBgQCM/3TdSuZ7ms9Yjlqh9gBuBwtA8LUfezQ74G2vVfG01Tscadav98M+lNsTb6fI/zgOf44pRnMxzQDJx3nJKzG1EfjG3k2P7FCOJ9sO354lIbkucWpulcHGLICly/VcNHT1RCQc+lYjphS13+TrrsqH0GdbCrDOVRIXXqJ2IQTvGQKBgQCFVflMH4jzvx8Ya0hMi4vsBFY8BrZI/NnDS5kFkIfnq38fq9OcK1+DWVT8cdDRIZ25TcJBrpVqhlty8Whi2yhoGHFr1lYyy/TRJZbJt3l/I8DvYr1PkYSRJJIaA7PTRoDrfFlbx17TxXXeLxTdCV3RrzkBaWqxakadHv34zrq4JQKBgHZwaeizEbChYwFDpEef7RJwwh/0Db6HLPeqeiBNqSNCmiBBrlPClCoa3CxlgGRs+J3PJD9NG5ACf6k2I0L9qev/QeVqF2vhpMLEQ4upvIsWUoYfMqO13TYs5Swp14ZSD/exE4bMOjearCERIzEES2JqP0u0Rrs1C1cQO2alOCff-----END PRIVATE KEY-----", "109260598919210", request.In);
        #endregion

        var jsonButtons = JsonConvert.SerializeObject(request.In.Buttons);
        var guestJourneyTemplate = await _db.GuestJourneyMessagesTemplates.Where(x=> x.Id == uniqeId).FirstOrDefaultAsync(cancellationToken);
        guestJourneyTemplate.Name = request.In.Name;
        guestJourneyTemplate.TempleteType = request.In.TempleteType;
        guestJourneyTemplate.TempletMessage = request.In.TempletMessage;
        guestJourneyTemplate.IsActive = request.In.IsActive;
        guestJourneyTemplate.Buttons = jsonButtons == "[]" ? null : jsonButtons;

        var templateResponse = await _vonageService.CreateTemplate(_vonageSettings.AppId, uniqeId, _vonageSettings.AppPrivatKey, _vonageSettings.WABAId, request.In,cancellationToken, MessageSenderEnum.Hospitio.ToString(), stepName, request.In.UserId);

        var jsonResponse = JsonConvert.DeserializeObject<dynamic>(templateResponse.Response);

        guestJourneyTemplate.WhatsappTemplateName =  templateResponse.TemplateName;

        if (templateResponse.Status == "failed")
        {
            string errorDetails = jsonResponse.detail;
            if (errorDetails.Contains("not a valid URI"))
            {
                return _response.Error($"Please Enter a Right URI or Select a Right Parameter For URL", AppStatusCodeError.UnprocessableEntity422);
            }
            else if (errorDetails.Contains("not a valid phone number"))
            {
                return _response.Error($"Please Enter a Right Phone Number or Select a Right Parameter For Phone Number", AppStatusCodeError.UnprocessableEntity422);
            }
            else
            {
                return _response.Error($"{jsonResponse.detail}", AppStatusCodeError.UnprocessableEntity422);
            }
        } 
        else
        {
            guestJourneyTemplate.VonageTemplateId = jsonResponse.id;
            guestJourneyTemplate.VonageTemplateStatus = jsonResponse.status;
        }

        await _db.SaveChangesAsync(cancellationToken);

        var guestJourneyMessagesTemplatesOut = new CreatedGuestJourneyMessagesTemplatesOut
        {
            Id = guestJourneyTemplate.Id,
            Name = guestJourneyTemplate.Name,
            WhatsappTemplateName = guestJourneyTemplate.WhatsappTemplateName,
            TempleteType = guestJourneyTemplate?.TempleteType,
            TempletMessage = guestJourneyTemplate?.TempletMessage,
            VonageTemplateId = guestJourneyTemplate?.VonageTemplateId,
            VonageTemplateStatus = guestJourneyTemplate?.VonageTemplateStatus,
            Buttons = guestJourneyTemplate?.Buttons
        };

        return _response.Success(new CreateGuestJourneyMessagesTemplatesOut("Create guest journey message templates successful.", guestJourneyMessagesTemplatesOut));
    }
}
