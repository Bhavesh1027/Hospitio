using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using HospitioApi.Core.Options;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Vonage.Messages.WhatsApp;
using Vonage.Request;
using Vonage;
using HospitioApi.Core.Services.Vonage;
using Vonage.Common;
using HospitioApi.Core.Services.Chat;
using Microsoft.AspNetCore.SignalR;
using HospitioApi.Core.SignalR.Hubs;

namespace HospitioApi.Core.HandleCustomerGuest.Commands.CreateCustomerGuest;
public record CreateCustomerGuestRequest(CreateCustomerGuestIn In, string CustomerId) : IRequest<AppHandlerResponse>;
public class CreateCustomerGuestHandler : IRequestHandler<CreateCustomerGuestRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly JwtSettingsOptions _jwtSettings;
    private readonly FrontEndLinksSettingsOptions _frontEndLinksSettings;
    private readonly IVonageService _vonageService;
    private readonly IChatService _chatService;
    private readonly IHubContext<ChatHub> _hubContext;
    public CreateCustomerGuestHandler(ApplicationDbContext db, IHandlerResponseFactory response, IOptions<JwtSettingsOptions> jwtSettings, IOptions<FrontEndLinksSettingsOptions> frontEndLinksSettings, IVonageService vonageService, IChatService chatService, IHubContext<ChatHub> hubContext)
    {
        _db = db;
        _response = response;
        _jwtSettings = jwtSettings.Value;
        _frontEndLinksSettings = frontEndLinksSettings.Value;
        _vonageService = vonageService;
        _chatService = chatService;
        _hubContext = hubContext;
    }
    public async Task<AppHandlerResponse> Handle(CreateCustomerGuestRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.CustomerGuests.Where(e => e.CustomerReservationId == request.In.CustomerReservationId).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The customer guest already exists.", AppStatusCodeError.UnprocessableEntity422);
        }
        var customerGuest = new CustomerGuest
        {
            CustomerReservationId = request.In.CustomerReservationId,
            ArrivalFlightNumber = request.In.ArrivalFlightNumber,
            BlePinCode = request.In.BlePinCode,
            City = request.In.City,
            Country = request.In.Country,
            DepartureAirline = request.In.DepartureAirline,
            DepartureFlightNumber = request.In.DepartureFlightNumber,
            Email = request.In.Email,
            FirstJourneyStep = request.In.FirstJourneyStep,
            Firstname = request.In.Firstname,
            IdProof = request.In.IdProof,
            IdProofNumber = request.In.IdProofNumber,
            IdProofType = request.In.IdProofType,
            Language = request.In.Language,
            Lastname = request.In.Lastname,
            PhoneCountry = request.In.PhoneCountry,
            PhoneNumber = request.In.PhoneNumber,
            Picture = request.In.Picture,
            Pin = request.In.Pin,
            Postalcode = request.In.Postalcode,
            Rating = request.In.Rating,
            RoomNumber = request.In.RoomNumber,
            Signature = request.In.Signature,
            Street = request.In.Street,
            StreetNumber = request.In.StreetNumber,
            TermsAccepted = request.In.TermsAccepted,
            DateOfBirth = request.In.DateOfBirth,
            Vat = request.In.Vat,
            IsActive = request.In.IsActive,
            BookingChannel = request.In.BookingChannel,
            DepartingFlightDate = request.In.DepartingFlightDate,
            AgeCategory = 1
        };

        await _db.CustomerGuests.AddAsync(customerGuest);
        await _db.SaveChangesAsync(cancellationToken);

        #region Welcome Msg Routing
        // whatsapp message

        //var customerVonageCredentials = await _db.VonageCredentials.Where(e => e.CustomerId == int.Parse(request.CustomerId)).FirstOrDefaultAsync();
        //if(customerVonageCredentials != null)
        //{
        //    //var response = await _vonageService.SendWhatsappTemplateMessage(customerVonageCredentials.AppId, customerVonageCredentials.AppPrivatKey, "");
        //    //var response = await _vonageService.SendWhatsappTemplateMessage("770300db-eea7-4d8c-81a0-9fd8f9646430", "-----BEGIN PRIVATE KEY-----MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQDUDGS/ZLzdvPtFRx2l8rX+izeP9A96plFG76XNtc2SDo2HzHO8AlFyr01bvj6R6Jv8JC3cvSqkHbYwrCF6BgPCqQotoxKt+kx04B5UuJJMpvPrsT8lLhFBwaU0prIerIGMTtuBlMsXFJy069WWCIOGbVwS7uPYX7hUyfKAvqjn90B69je0rNcfufDuA0ETvosfaUgqavo9aLjma73nEeD/3osDDdp1W2JJ50e6e3QoWmhPEd/XcImlhbyAY5hQRwmU7M8l5P55Jd8r1sHqrPWE4QTLM8V9fn5x18Oyb4zBkUmiM37U9oqV3oA/tDFT8N8kT9VQFPZRTF0/QWDFretDAgMBAAECggEAB9qzqqE1G7OBzSWetARi+Fw4wSTFpAHgFdQPnKiExkmytVp92PNvB+P1ZhYMZEQs7vB6IGiDWVPeaTwe+1GYtw+i2GDncrlINrYMeY/in2M5uru0fpCPxBNDpskb5OX34HYVTF3yDddOHKd1oFqNYJ/RXsQpfVzCS6W1TU6CnCDrqEJEZTWw+mFJbF07Q58Vde+9PSwMYZgM6BVjuVtBPR2rFEufuEoSQbUBJzsE7YknCYY8uqLsMW7KMGzpD6XMIqhtOzzKhNzcl+GbrQkfGWLWCELd+xV9HumpB2WpSCHbEoHrraoHoWHwcaLa28d/hJ7tIqDA9GHd4RIROcX+4QKBgQDqxx+AQQ5aaJGEly3KGn4Agotx9sHsn7/TQTod/TzPr7a1subWjGzuEjNL3VurqfnCx3Me0Pu8LgZvLwbfyZ6rTkWOEf0z2u9CQ2fcXfe1eULArmqOAm+os1lNCP+gtKga3KhfZ0s1rWRn+7Hy01AxAyH2EHFD+OIiJzXQgVLyIQKBgQDnN0zrSW035XqYkCJUjpjap0ksvVvMo3eYYgnVnlOOB/qtMKbUQHveYn3YgqzsIfzSmDNsaIQ3rbRjLX7xhSS5+ggGG/h6xRZY6hlqrPJm2m3DjGyZTGVXhVvTngcT4fmPVUiqGNvdOOGsUjbQz8oKjR8iNXlzsFVSkKRyYuo44wKBgF0Lf2N/5OVVrd4jYAVJzIf9NLB8v8w8X3Sk1BiQhSo2FC4ccbmzu3P6iJPXbX4yBgb4rpoW7r1cbDZiJ9JHAHgZtB0i6ftEGDBlsGK4ZvRn01mzhg0zz/bG5WxPVafmEP1cV+o/cKIcTr+bE6INtylFeY8gYxMyLsSkT4KLzkihAoGAUfvz1EH27ij6bXo+Egl1/aHvemOyVz9nOqsYnZxEOEpwAlUL8pri0RnPUaQUOK5cfTfmk/wDVdLL8ZbCOlVhjmgvzCrC0pVrtR9c38xvLzUoUnxKTZkfHgd1ZfXUwKIR4Vb2Kwohe7Gdo4KLWUM0esVLUs/vPqw5tMsA5GZacs0CgYEAw9Lc9+dhgRfulLI9Vsi43Ix2uweHRgQM+P54uSRQ5uyLqEm5yrMrdapV0iXTP1+2UR1ruL+6VH/ZSccwD7Y3t511CiVkjExRjOQN5U1Y8RBPFZ9RuPB08wW5ffAdAwyBo8apI204B2Uu8uQQjFfs6CO2anjvidZt31/J2vN1/iQ=-----END PRIVATE KEY-----", "");
        //    var response = _vonageService.SendWhatsappTemplateMessage("5a2659bc-9058-4d79-8d9f-d18c24d24a31", "-----BEGIN PRIVATE KEY-----MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQChLZyclROwPf+satz3AJH2w4dwC5PLsDAvjb2htSJs0QX6cgKUI+A0TGrL1O0x4CjePGvDrlLnxo7ZVjpXgB7NJ2OrQgfQNVdaiVJQI1slwZnnY4xiVC0mQMl958a4FilFaLcrmGkl6VPbzc06+cWGL8ZwnvBSjlYX/U+XUP7ttT7+3sUeDoimPgkGgJzmMDCfoNxXi3DnsrZm6m8oUtUOR06lVx5A7ewoojPJm62KSVtqpnL3jPWxXblR/ErXldhdWLrGI7ADceo0LFR8KagM+6hXxqfBj4DPB3WP3o9qwXaRPZh8GVnKlDxPA9roDDoqZZ0R7O8P+/VbirS7UQGRAgMBAAECggEAMhWnXezhQlnxshU+9q5BrUmTM5kVYy0rvAsyiyZrPR8y2WFGNdx0FixM32waDO6YJH7oCdWIw6cqypSF6pzQdXWw/g21udhpfaPAZVCnSTNA7Os9O2zm3sUxF6PHV3rjdkMU8EIbIoG/4kSwaowk+g6sfmCVU0IRtMCtU9sCbMDwHAJNjnuZ8rn9AopFGt1quckiSavtTBVYCN4eQ8oCXvN7+7ilxGbHhVifTSeXjoC4nxZr9r+TwVgQOqQOl3B6VqK5OVV5p8+EMXmfQeAlwPh3JtVdAX/Nl14B92yx5NHomTnKNSQ2T1wRm7csvxZtuUmNkIJHf4Dh1GC7W//K1QKBgQDVpSVE6skivTrOMoqw88YpYZK3FIPTHR5qzYKRPsTmQj4uSmflwjoKs6hC7Bu0L6QIMHeQkH4YeziZp1VymAMIMJGbhFjRoaIqheoFmmpsqx1JhuIA5pk/y2L/6KzIWbBFqLGqVXxmul+TmEWUNIBDlh6b16ID9wGEj9BkyENVPwKBgQDBIa4ESDlps9CPopRw9GGWBD6xNm5rRR8y5TeOcTZ28xTrHb7Xye5FpJeHukdIyVOKEAVWg9NJJ/e5XvK0jd/y+AfmbLZUFua3FSBe9A6oV7iupeUxv9y1mrIU96AUm698uEu6El6BdKpB3S/VQtYurDiJtK6Nf+02UG2sOI3lLwKBgQCM/3TdSuZ7ms9Yjlqh9gBuBwtA8LUfezQ74G2vVfG01Tscadav98M+lNsTb6fI/zgOf44pRnMxzQDJx3nJKzG1EfjG3k2P7FCOJ9sO354lIbkucWpulcHGLICly/VcNHT1RCQc+lYjphS13+TrrsqH0GdbCrDOVRIXXqJ2IQTvGQKBgQCFVflMH4jzvx8Ya0hMi4vsBFY8BrZI/NnDS5kFkIfnq38fq9OcK1+DWVT8cdDRIZ25TcJBrpVqhlty8Whi2yhoGHFr1lYyy/TRJZbJt3l/I8DvYr1PkYSRJJIaA7PTRoDrfFlbx17TxXXeLxTdCV3RrzkBaWqxakadHv34zrq4JQKBgHZwaeizEbChYwFDpEef7RJwwh/0Db6HLPeqeiBNqSNCmiBBrlPClCoa3CxlgGRs+J3PJD9NG5ACf6k2I0L9qev/QeVqF2vhpMLEQ4upvIsWUoYfMqO13TYs5Swp14ZSD/exE4bMOjearCERIzEES2JqP0u0Rrs1C1cQO2alOCff-----END PRIVATE KEY-----", "");

        //    var customerUser = await _db.CustomerUsers.Where(e=>e.CustomerId == int.Parse(request.CustomerId)).FirstOrDefaultAsync();
        //    if (customerUser != null && customerGuest.Id != null)
        //    {
        //        var SenderUserType = ((int)UserTypeEnum.Customer).ToString();
        //        var receiverUserType = ((int)UserTypeEnum.Guest).ToString();
        //        var channels = await _chatService.GetChannelDataFromUsersDetail(customerGuest.Id, receiverUserType, customerUser.Id, SenderUserType);

        //        if (channels != null)
        //        {
        //            // send message in this channel
        //            var isMessageSend = await SendMessage(channels.Id, customerUser.Id.ToString(), SenderUserType, customerGuest.Id.ToString(), receiverUserType, "Hospitio Hotel Booking Confirmation\r\nThank you for choosing Luxury Inn Hotel! Your booking for 3 nights starting from September 10th,2023 has been confirmed. We look forward to hosting you. If you have any special requests or need assistance, feel free to contact us.", null, 1, "Text", null, null, null);
        //        }
        //        else
        //        {
        //            var chatId = await _chatService.CreateChat(_db, customerUser.Id.ToString(), SenderUserType, customerGuest.Id, ((ChatUserTypeEnum)int.Parse(receiverUserType)).ToString());
        //            var isMessageSend = await SendMessage(chatId, customerUser.Id.ToString(), SenderUserType, customerGuest.Id.ToString(), receiverUserType, "Hospitio Hotel Booking Confirmation\r\nThank you for choosing Luxury Inn Hotel! Your booking for 3 nights starting from September 10th,2023 has been confirmed. We look forward to hosting you. If you have any special requests or need assistance, feel free to contact us.", null, 1, "Text", null, null, null);
        //        }
        //    }
        //}
        // 
        #endregion
        var createdCustomerGuestOut = new CreatedCustomerGuestOut
        {
            Id = customerGuest.Id,
            CustomerReservationId = customerGuest.CustomerReservationId,
            ArrivalFlightNumber = customerGuest.ArrivalFlightNumber,
            BlePinCode = customerGuest.BlePinCode,
            City = customerGuest.City,
            Country = customerGuest.Country,
            DepartureAirline = customerGuest.DepartureAirline,
            DepartureFlightNumber = customerGuest.DepartureFlightNumber,
            Email = customerGuest.Email,
            FirstJourneyStep = customerGuest.FirstJourneyStep,
            Firstname = customerGuest.Firstname,
            IdProof = customerGuest.IdProof,
            IdProofNumber = customerGuest.IdProofNumber,
            IdProofType = customerGuest.IdProofType,
            Language = customerGuest.Language,
            Lastname = customerGuest.Lastname,
            PhoneCountry = customerGuest.PhoneCountry,
            PhoneNumber = customerGuest.PhoneNumber,
            Picture = customerGuest.Picture,
            Pin = customerGuest.Pin,
            Postalcode = customerGuest.Postalcode,
            Rating = customerGuest.Rating,
            RoomNumber = customerGuest.RoomNumber,
            Signature = customerGuest.Signature,
            Street = customerGuest.Street,
            StreetNumber = customerGuest.StreetNumber,
            TermsAccepted = customerGuest.TermsAccepted,
            DateOfBirth = customerGuest.DateOfBirth,
            Vat = customerGuest.Vat,
            IsActive = customerGuest.IsActive,
         
        };

        createdCustomerGuestOut.Link = _frontEndLinksSettings.GuestPortal + "?token=" + Uri.EscapeDataString(GenerateToken(createdCustomerGuestOut.CustomerReservationId, request.CustomerId, createdCustomerGuestOut.Id));

        return _response.Success(new CreateCustomerGuestOut("Create customer guest successful.", createdCustomerGuestOut));
    }

    public string GenerateToken(int? reservationId, string customerId, int customerGuestId)
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
             new("ReservationId", reservationId.ToString(),ClaimValueTypes.Integer64),
             new("GuestId", customerGuestId.ToString(), ClaimValueTypes.Integer64),
             new("UserId", customerGuestId.ToString(), ClaimValueTypes.Integer64),
             new("CustomerId", customerId.ToString(),ClaimValueTypes.Integer64),
             new("UserType", ((int)UserTypeEnum.Guest).ToString(),ClaimValueTypes.String),
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

    public async Task<int> SendMessage(int chatId, string senderId, string senderUserType, string receiverId, string receiverUserType, string message, string message_uuid, byte source, string type, string attachment, int? requestId, string url)
    {
        var chat = await _chatService.SendMessage(_db, chatId, senderId, senderUserType, message, source, type, attachment, requestId, url, message_uuid, 1, null);

        var totalUnreadMessageCount = await _chatService.GetTotalUnreadMessageCount(receiverId, int.Parse(receiverUserType));
        await _hubContext.Clients.Group($"user-{senderUserType}-{senderId}").SendAsync("GetNewMessage", chat);
        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetNewMessage", chat);

        var totalUnreadMessageCountResponse = new { Type = "Communication", Id = chatId, Count = totalUnreadMessageCount };

        await _hubContext.Clients.Group($"user-{receiverUserType}-{receiverId}").SendAsync("GetTotalUnreadCount", totalUnreadMessageCountResponse);
        return 1;
    }
}


