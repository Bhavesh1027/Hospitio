using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.Services.BackGroundServiceData;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.Vonage;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustGuestPortalCheckInFormBuilder.Commands.EditCustomerGuestPortalCheckInReservation;
public record EditGuestAppCustomerReservationRequest(EditCustomerGuestPortalCheckInReservationIn In) : IRequest<AppHandlerResponse>;
public class EditCustomerGuestPortalCheckInReservationHandler : IRequestHandler<EditGuestAppCustomerReservationRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public readonly IChatService _chatService;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IVonageService _vonage;

    public EditCustomerGuestPortalCheckInReservationHandler(ApplicationDbContext db, IHandlerResponseFactory response, IChatService chatService, IHubContext<ChatHub> hubContext, IVonageService vonage)
    {
        _db = db;
        _response = response;
        _chatService = chatService;
        _hubContext = hubContext;
        _vonage = vonage;
    }
    public async Task<AppHandlerResponse> Handle(EditGuestAppCustomerReservationRequest request, CancellationToken cancellationToken)
    {
        var customerReservation = await _db.CustomerReservations.Include(x => x.Customer).Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerReservation == null)
        {
            return _response.Error($"Customer reservation could not be found.", AppStatusCodeError.Gone410);
        }

        if(request.In.NoOfGuestAdults != null) 
            customerReservation.NoOfGuestAdults = request.In.NoOfGuestAdults;
        if(request.In.NoOfGuestChilderns != null)
            customerReservation.NoOfGuestChildrens = request.In.NoOfGuestChilderns;

        await _db.SaveChangesAsync(cancellationToken);

        //send template message to guest when complete check in
        //if ((bool)request.In.IsCheckInCompleted)
        //{
        //    var customerGuest = await _db.CustomerGuests.Where(x => x.CustomerReservationId == customerReservation.Id).FirstOrDefaultAsync(cancellationToken);

        //    var customerGuestJournies = await _db.CustomerGuestJournies.Where(x => x.JourneyStep == (int)JourneyStepTempleteTypeEnums.OnlineCheckIn && x.CutomerId ==customerReservation.CustomerId).ToListAsync(cancellationToken);

        //    var vonageCred = await _db.VonageCredentials.Where(x => x.CustomerId == customerReservation.CustomerId).FirstOrDefaultAsync(cancellationToken);

        //    string bussinesAdress = $"{customerReservation.Customer.City},{customerReservation.Customer.Country},{customerReservation.Customer.Postalcode}";

        //    foreach (var item in customerGuestJournies)
        //    {
        //        if (item.VonageTemplateStatus == "APPROVED")
        //        {
        //            CustomerGuestJorneyDetails CustomerGuestJorneyDetails = new CustomerGuestJorneyDetails();
        //            CustomerGuestJorneyDetails.TempletMessage = item.TempletMessage;
        //            CustomerGuestJorneyDetails.Buttons = item.Buttons;
        //            CustomerGuestJorneyDetails.BussinessName = customerReservation.Customer.BusinessName;
        //            CustomerGuestJorneyDetails.BussinessAddress = bussinesAdress;
        //            CustomerGuestJorneyDetails.BussinessPhoneNumber = customerReservation.Customer.PhoneNumber;
        //            CustomerGuestJorneyDetails.ReservationNumber = customerReservation.ReservationNumber;
        //            CustomerGuestJorneyDetails.GuestName = customerGuest.Firstname;
        //            CustomerGuestJorneyDetails.CheckinDate = (DateTime)customerReservation.CheckinDate;
        //            CustomerGuestJorneyDetails.CheckoutDate = (DateTime)customerReservation.CheckoutDate;
        //            CustomerGuestJorneyDetails.CustomerWhatsAppNumber = customerReservation.Customer.WhatsappNumber;
        //            CustomerGuestJorneyDetails.Phone = customerGuest.PhoneNumber;
        //            CustomerGuestJorneyDetails.APPId = vonageCred.AppId;
        //            CustomerGuestJorneyDetails.PrivateKey = vonageCred.AppPrivatKey;
        //            CustomerGuestJorneyDetails.TemplateName = item.Name;

        //            await SendWebChatMessage(CustomerGuestJorneyDetails, _vonage);
        //        }
        //    }
        //}

        var updateCustomerGuestAlertsOut = new UpdatedCustomerReservationOut()
        {
            Id = customerReservation.Id,
            CustomerId = customerReservation.CustomerId,
            ReservationNumber = customerReservation.ReservationNumber,
            CheckinDate = customerReservation.CheckinDate,
            CheckoutDate = customerReservation.CheckoutDate,
            NoOfGuestAdults = customerReservation.NoOfGuestAdults,
            NoOfGuestChilderns = customerReservation.NoOfGuestChildrens,
            Uuid = customerReservation.Uuid,
            Source = customerReservation.Source,
            IsActive = customerReservation.IsActive
        };

        return _response.Success(new EditCustomerGuestPortalCheckInReservationOut("Update customer reservation successful.", updateCustomerGuestAlertsOut));
    }
    public async Task<dynamic> SendWebChatMessage(CustomerGuestJorneyDetails message, IVonageService vonageService)
    {
        bool hasBodyParameters = false;

        var realTemplateMessage = message.TempletMessage;
        var templateMessage = realTemplateMessage;
        var buttons = message.Buttons;
        var buttonObject = JsonConvert.DeserializeObject<List<button>>(buttons); // buttonobject null >> no button
   
        if (buttonObject != null)
        {
            if (buttonObject.Count > 0)
            {
                foreach (var item in buttonObject)
                {
                    if (item.type == "URL")
                    {
                        item.value = "https://hospitio-guest-dev.appdemoserver.com/";
                    }
                }
            }
        }
        var attchement = (JsonConvert.SerializeObject(buttonObject) == "null") ? null : JsonConvert.SerializeObject(buttonObject);

        var bodyplaceholderMatches = System.Text.RegularExpressions.Regex.Matches(realTemplateMessage, @"\{([^}]+)\}").ToList();

        hasBodyParameters = (bodyplaceholderMatches.Count > 0) ? true : false;
        int templateMessagePlaceholders = bodyplaceholderMatches.Count;

        Guid MessgaeUuid = new Guid();
        TimeSpan bookingDays = message.CheckoutDate - message.CheckinDate;

        if (bodyplaceholderMatches.Count > 0)
        {
            int count = 0;
            foreach (var item in bodyplaceholderMatches)
            {
                switch (item.ToString())
                {
                    case "{hotel_name}":
                        templateMessage = templateMessage.Replace("{hotel_name}", message.BussinessName);
                        break;
                    case "{booking_days}":
                        templateMessage = templateMessage.Replace("{booking_days}", bookingDays.Days.ToString());
                        break;
                    case "{hotel_address}":
                        templateMessage = templateMessage.Replace("{hotel_address}", message.BussinessAddress);
                        break;
                    case "{hotel_phonenumber}":
                        templateMessage = templateMessage.Replace("{hotel_phonenumber}", message.BussinessPhoneNumber);
                        break;
                    case "{guest_name}":
                        templateMessage = templateMessage.Replace("{guest_name}", message.GuestName);
                        break;
                    case "{guest_reservationnumber}":
                        templateMessage = templateMessage.Replace("{guest_reservationnumber}", message.ReservationNumber);
                        break;
                    case "{checkin_date}":
                        templateMessage = templateMessage.Replace("{checkin_date}", message.CheckinDate.ToString("yyyy-MM-dd"));
                        break;
                    case "{checkout_date}":
                        templateMessage = templateMessage.Replace("{checkout_date}", message.CheckoutDate.ToString("yyyy-MM-dd"));
                        break;
                    case "{guest_url}":
                        templateMessage = templateMessage.Replace("{guest_url}", "[Click Here](https://hospitio-guest-dev.appdemoserver.com/)");
                        break;
                }
                count++;
            }

            #region Code For Refrence
            //var BodyParameters = temparr.ToList();

            //string receiver = message.Phone.ToString().IndexOf('+') == 0 ? message.Phone.ToString().Trim().Substring(1) : message.Phone.ToString();

            //string sender = message.CustomerWhatsAppNumber.ToString().IndexOf('+') == 0 ? message.CustomerWhatsAppNumber.ToString().Trim().Substring(1) : message.CustomerWhatsAppNumber.ToString();
            //string templateName = message.TemplateName;


            //var response = await vonageService.SendWhatsappTemplateMessage(message.APPId, message.PrivateKey, realTemplateMessage, receiver, sender, templateName, BodyParameters, hasButton, ButtonParameters);
            //MessgaeUuid = response?.MessageUuid;
            #endregion
        }

        var SenderUserType = ((int)UserTypeEnum.Customer).ToString();
        var receiverUserType = ((int)UserTypeEnum.Guest).ToString();

        var channels = await _chatService.GetChannelDataFromUsersDetail(message.GuestId, receiverUserType, message.CustomerUserId, SenderUserType);

        if (channels != null)
        {
            var isMessageSend = await SendMessage(channels.Id, message.CustomerUserId.ToString(), SenderUserType, message.GuestId.ToString(), receiverUserType, templateMessage, null, 1, "Template", attchement, null, null);
        }
        else
        {
            var chatId = await _chatService.CreateChat(_db, message.CustomerUserId.ToString(), SenderUserType, message.GuestId, ((ChatUserTypeEnum)int.Parse(receiverUserType)).ToString());
            var isMessageSend = await SendMessage(chatId, message.CustomerUserId.ToString(), SenderUserType, message.GuestId.ToString(), receiverUserType, templateMessage,null, 1, "Template", attchement, null, null);
        }
        return new { MessageId = MessgaeUuid, TemplateMessage = templateMessage, Attachment = attchement };
    }
    public async Task<int> SendMessage(int chatId, string senderId, string senderUserType, string receiverId, string receiverUserType, string message, string message_uuid, byte source, string type, string attachment, int? requestId, string url)
    {
        var chat = await _chatService.SendMessage(_db, chatId, senderId, senderUserType, message, source, type, attachment, requestId, url, message_uuid, 3, null);

        var totalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(receiverId, int.Parse(receiverUserType));
        await _hubContext.Clients.Group($"user-{senderUserType}-{senderId}").SendAsync("GetNewMessage", chat);
        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetNewMessage", chat);

        var totalUnreadMessageCountResponse = new { Type = "Communication", Id = chatId, Count = totalUnreadMessageCount };

        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetTotalUnreadCount", totalUnreadMessageCountResponse);
        return 1;
    }
}
