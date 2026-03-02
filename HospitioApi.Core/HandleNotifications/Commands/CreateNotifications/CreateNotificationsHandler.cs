using Dapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleNotifications.Commands.CreateNotifications;
public record CreateNotificationsRequest(CreateNotificationsIn In) : IRequest<AppHandlerResponse>;
public class CreateNotificationsHandler : IRequestHandler<CreateNotificationsRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;
    private readonly IDapperRepository _dapper;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IChatService _chatService;

    public CreateNotificationsHandler(ApplicationDbContext db, IHubContext<ChatHub> hubContext, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository,IDapperRepository dapper,IChatService chatService)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
        _dapper = dapper;
        _hubContext = hubContext;
        _chatService = chatService;
    }

    public async Task<AppHandlerResponse> Handle(CreateNotificationsRequest request, CancellationToken cancellationToken)
    {
        var notification = await _commonRepository.NotificationsAdd(request.In, _db, cancellationToken);

        if (request.In.CurrentUserType == (int)UserTypeEnum.Hospitio)
        {
            var spParams = new DynamicParameters();
            spParams.Add("businessTypeId", request.In.BusinessTypeId, DbType.Int32);
            spParams.Add("productId", request.In.ProductId, DbType.Int32);
            spParams.Add("country", request.In.Country, DbType.String);
            spParams.Add("city", request.In.City, DbType.String);
            spParams.Add("postalcode", request.In.Postalcode, DbType.String);

            var GetCustomerForNotification = await _dapper
               .GetAll<UserNotification>("[dbo].[SP_GetCustomerForNotification]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

            if(GetCustomerForNotification == null || GetCustomerForNotification.Count == 0)
            {
                return _response.Error("Customer not found", AppStatusCodeError.Forbidden403);
            }

            var notificationHistory = await _commonRepository.NotificationsHistoryAdd(request.In, _db, GetCustomerForNotification, notification.Id, cancellationToken, (int)UserTypeEnum.Customer);

            var notificationOut = new CreatedNotificationsOut()
            {
                Id = notification.Id,
                Country = notification.Country,
                City = notification.City,
                Postalcode = notification.Postalcode,
                BusinessTypeId = notification.BusinessTypeId,
                ProductId = notification.ProductId,
                Title = notification.Title,
                Message = notification.Message
            };

            notification.CreatedAt = Convert.ToDateTime(Convert.ToDateTime(notification.CreatedAt).ToString("yyyy-MM-ddTHH:mm:ss.fff"));

            var notifyobj = new
            {
                Message = notification.Message,
                Title = notification.Title,
                CreatedAt = notification.CreatedAt,
                Id = notification.Id,
                MessageType = (ChatUserTypeEnum.HospitioUser).ToString()
            };

            foreach (var item in GetCustomerForNotification)
            {
                var customerUser = await _db.CustomerUsers.Where(e => e.CustomerId == item.UserId).FirstOrDefaultAsync(cancellationToken);

                await _hubContext.Clients.Group($"user-2-{customerUser.Id}").SendAsync("GetNewNotification", notifyobj);

                var totalUnreadNotificationCount = await _chatService.GetTotalUnreadNotificationCount(item.UserId.ToString(), (int)UserTypeEnum.Customer);
                var totalUnreadNotificationCountResponse = new { Type = "Notification", Count = totalUnreadNotificationCount };

                await _hubContext.Clients.Group($"user-2-{customerUser.Id}").SendAsync("GetTotalUnreadCount", totalUnreadNotificationCountResponse);

            }
            var UserData = await _db.Users.Where(s => s.DeletedAt == null).ToListAsync(cancellationToken);
            foreach(var user in UserData)
            {
                await _hubContext.Clients.Group($"user-1-{user.Id}").SendAsync("GetCreateNotification", notifyobj);
            }
            return _response.Success(new CreateNotificationsOut("Create notification successful.", notificationOut));
        }
        else if(request.In.CurrentUserType == (int)UserTypeEnum.Customer)
        {
            var spParams = new DynamicParameters();
            spParams.Add("country", request.In.Country, DbType.String);
            spParams.Add("city", request.In.City, DbType.String);
            spParams.Add("postalcode", request.In.Postalcode, DbType.String);
            spParams.Add("customerId" , request.In.CustomerId, DbType.Int32);

            var GetGuestForNotification = await _dapper
              .GetAll<UserNotification>("[dbo].[SP_GetGuestForNotification]", spParams, cancellationToken, commandType: CommandType.StoredProcedure);

            if(GetGuestForNotification == null || GetGuestForNotification.Count == 0)
            {
                return _response.Error("Guest not found", AppStatusCodeError.Forbidden403);
            }

            var notificationHistory = await _commonRepository.NotificationsHistoryAdd(request.In, _db, GetGuestForNotification, notification.Id, cancellationToken, (int)UserTypeEnum.Guest);

            var notificationOut = new CreatedNotificationsOut()
            {
                Id = notification.Id,
                Country = notification.Country,
                City = notification.City,
                Postalcode = notification.Postalcode,
                Title = notification.Title,
                Message = notification.Message
            };

            notification.CreatedAt = Convert.ToDateTime(Convert.ToDateTime(notification.CreatedAt).ToString("yyyy-MM-ddTHH:mm:ss.fff"));

            var notifyobj = new
            {
                Message = notification.Message,
                Title = notification.Title,
                CreatedAt = notification.CreatedAt,
                Id = notification.Id,
                MessageType = (ChatUserTypeEnum.CustomerUser).ToString()
            };

            foreach (var item in GetGuestForNotification)
            {
                var customerGuest = await _db.CustomerGuests.Where(e => e.Id == item.UserId).FirstOrDefaultAsync(cancellationToken);

                await _hubContext.Clients.Group($"user-3-{customerGuest.Id}").SendAsync("GetNewNotification", notifyobj);

                var totalUnreadNotificationCount = await _chatService.GetTotalUnreadNotificationCount(item.UserId.ToString(), (int)UserTypeEnum.Guest);
                var totalUnreadNotificationCountResponse = new { Type = "Notification", Count = totalUnreadNotificationCount };

                await _hubContext.Clients.Group($"user-3-{customerGuest.Id}").SendAsync("GetTotalUnreadCount", totalUnreadNotificationCountResponse);

            }

            var customerUser = await _db.CustomerUsers.Where(s => s.CustomerId == request.In.CustomerId && s.DeletedAt == null).ToListAsync(cancellationToken);

            foreach (var user in customerUser)
            {
                await _hubContext.Clients.Group($"user-2-{user.Id}").SendAsync("GetCreateNotification", notifyobj);
            }
            return _response.Success(new CreateNotificationsOut("Create notification successful.", notificationOut));
        }

        return _response.Error("Notification Does not exits", AppStatusCodeError.Forbidden403);
    }
}
