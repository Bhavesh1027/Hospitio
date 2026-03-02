using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.HandleCustomers.Commands.CreateCustomer;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleTicket.Commands.CreateTicketReply;

public record CreateTicketReplyRequest(CreateTicketReplyIn In, UserTypeEnum UserType, string UserId)
    : IRequest<AppHandlerResponse>;

public class CreateTicketReplyHandler : IRequestHandler<CreateTicketReplyRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IChatService _chatService;
    private readonly IHandlerResponseFactory _response;
    

    public CreateTicketReplyHandler(
        ApplicationDbContext db, IHubContext<ChatHub> hubContext, IChatService chatService,
        IHandlerResponseFactory response
        )
    {
        _chatService = chatService;
        _hubContext = hubContext;
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateTicketReplyRequest request, CancellationToken cancellationToken)
    {
        var requestIn = request.In;

        var ticket = await _db.Tickets.Where(e => e.Id == requestIn.TicketId).FirstOrDefaultAsync(cancellationToken);
        if (ticket == null)
        {
            return _response.Error($"Ticket with Id {request.In.TicketId} could not be found.", AppStatusCodeError.Gone410);
        }

        var ticketReplies = new Data.Models.TicketReply()
        {
            Reply = requestIn.Reply,
            TicketId = requestIn.TicketId,
            IsActive = true,
            CreatedFrom = (byte?)request.UserType
        };

        ticket.Status = (byte?)TicketStatus.Assigned;

        await _db.TicketReplies.AddAsync(ticketReplies, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);

        Object dynamicResponse = null;
        if (ticket.CreatedFrom == (int)UserTypeEnum.Hospitio)
        {
            var userName = await _db.Users.Where(e => e.Id == ticket.CreatedBy).FirstOrDefaultAsync(cancellationToken);

            dynamicResponse = new
            {
                Id = ticketReplies.Id,
                Reply = ticketReplies.Reply,
                TicketId = ticketReplies.TicketId,
                CreatedBy = ticketReplies.CreatedBy,
                CreatedFrom = ticketReplies.CreatedFrom,
                CreatedAt = Convert.ToDateTime(Convert.ToDateTime(ticketReplies.CreatedAt).ToString("yyyy-MM-ddTHH:mm:ss.fff")),
                UserName = userName.FirstName + " " + userName.LastName,
            };
        }
        else if (ticket.CreatedFrom == (int)UserTypeEnum.Customer)
        {
            var userName = await _db.CustomerUsers.Where(e => e.Id == ticket.CreatedBy).FirstOrDefaultAsync(cancellationToken);

            dynamicResponse = new
            {
                Id = ticketReplies.Id,
                Reply = ticketReplies.Reply,
                TicketId = ticketReplies.TicketId,
                CreatedBy = ticketReplies.CreatedBy,
                CreatedFrom = ticketReplies.CreatedFrom,
                CreatedAt = Convert.ToDateTime(Convert.ToDateTime(ticketReplies.CreatedAt).ToString("yyyy-MM-ddTHH:mm:ss.fff")),
                UserName = userName.FirstName + " " + userName.LastName,
            };
        }
        var totalUnreadTicketCount = 0;
        var totalUnreadTicketCountResponse = new { Type="Ticket",Id=0 , Count =0};
        List<User> users = new List<User>();

        if (request.UserType == UserTypeEnum.Hospitio)
        {
            var customerUser = await _db.CustomerUsers.Where(e => e.CustomerId == ticket.CustomerId).FirstOrDefaultAsync(cancellationToken);
            await _hubContext.Clients.Group($"user-2-{customerUser.Id}").SendAsync("GetNewTicketReplay", dynamicResponse);
            await _hubContext.Clients.Group($"user-1-{request.UserId}").SendAsync("GetNewTicketReplay", dynamicResponse);

            totalUnreadTicketCount = await _chatService.GetTotalUnreadTicketCount(customerUser.Id.ToString(), "2",customerUser.CustomerId.ToString());
            totalUnreadTicketCountResponse = new { Type="Ticket",Id = ticket.Id, Count = totalUnreadTicketCount };
            await _hubContext.Clients.Group($"user-2-{customerUser.Id}").SendAsync("GetTotalUnreadCount", totalUnreadTicketCountResponse);
        }
        if (request.UserType == UserTypeEnum.Customer)
        {
            if (ticket.CSAgentId == null || ticket.CSAgentId == 0)
            {
                if (ticket.GroupId != null && ticket.GroupId != 0)
                {
                    users = await _db.Users.Where(e => e.GroupId == ticket.GroupId).ToListAsync(cancellationToken);
                }
                else
                {
                    users = await _db.Users.ToListAsync(cancellationToken);
                }
                foreach (var userItem in users)
                {
                    await _hubContext.Clients.Group($"user-1-{userItem.Id}").SendAsync("GetNewTicketReplay", dynamicResponse);
                    await _hubContext.Clients.Group($"user-2-{request.UserId}").SendAsync("GetNewTicketReplay", dynamicResponse);

                    totalUnreadTicketCount = await _chatService.GetTotalUnreadTicketCount(userItem.Id.ToString(), "1");
                    totalUnreadTicketCountResponse = new { Type="Ticket",Id = ticket.Id, Count = totalUnreadTicketCount };
                    await _hubContext.Clients.Group($"user-1-{userItem.Id}").SendAsync("GetTotalUnreadCount", totalUnreadTicketCountResponse);
                }
            }
            else
            {
                await _hubContext.Clients.Group($"user-1-{ticket.CSAgentId}").SendAsync("GetNewTicketReplay", dynamicResponse);
                await _hubContext.Clients.Group($"user-2-{request.UserId}").SendAsync("GetNewTicketReplay", dynamicResponse);

                totalUnreadTicketCount = await _chatService.GetTotalUnreadTicketCount(ticket.CSAgentId.ToString(), "1");
                totalUnreadTicketCountResponse = new { Type = "Ticket", Id = ticket.Id, Count = totalUnreadTicketCount };
                await _hubContext.Clients.Group($"user-1-{ticket.CSAgentId}").SendAsync("GetTotalUnreadCount", totalUnreadTicketCountResponse);
            }
        }
        return _response.Success(new CreateTicketReplyOut("Create ticket reply successful."));
    }
}

