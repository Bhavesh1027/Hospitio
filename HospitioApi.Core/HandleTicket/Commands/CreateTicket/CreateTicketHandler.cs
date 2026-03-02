using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleTicket.Commands.CreateTicket;

public record CreateTicketRequest(CreateTicketIn In, UserTypeEnum UserType, string UserId)
    : IRequest<AppHandlerResponse>;

public class CreateTicketHandler : IRequestHandler<CreateTicketRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IHandlerResponseFactory _response;

    public CreateTicketHandler(
        ApplicationDbContext db, IHubContext<ChatHub> hubContext,
        IHandlerResponseFactory response
        )
    {
        _hubContext = hubContext;
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateTicketRequest request, CancellationToken cancellationToken)
    {
        var requestIn = request.In;
        var ticket = new Ticket()
        {
            CSAgentId = requestIn.CSAgentId,
            CustomerId = requestIn.CustomerId,
            Details = requestIn.Details,
            Duedate = requestIn.Duedate,
            Priority = requestIn.Priority,
            Status = requestIn.Status,
            Title = requestIn.Title,
            CreatedFrom = (byte?)request.UserType,
            IsActive = true,
        };

        await _db.Tickets.AddAsync(ticket, cancellationToken);

        await _db.SaveChangesAsync(cancellationToken);

        var customerCheckIn = await _db.CustomerGuestsCheckInFormBuilders.Where(e => e.CustomerId == ticket.CustomerId && e.IsActive == true).FirstOrDefaultAsync(cancellationToken);
        //var customerCheckIn = await _db.CustomerUsers.Join(_db.CustomerGuestsCheckInFormBuilders, e => e.CustomerId, c => c.CustomerId,(e,c) => e.CustomerId == ticket.CustomerId && e.IsActive == true && c.IsActive == true).FirstOrDefaultAsync(cancellationToken)    

        List<User> users = new List<User>();
        ticket.CreatedAt = Convert.ToDateTime(Convert.ToDateTime(ticket.CreatedAt).ToString("yyyy-MM-ddTHH:mm:ss.fff"));
        if (request.UserType == UserTypeEnum.Hospitio)
        {
            var customerUser = await _db.CustomerUsers.Where(e => e.CustomerId == ticket.CustomerId).FirstOrDefaultAsync(cancellationToken);
            var objticket = new
            {
                CSAgentId = ticket.CSAgentId,
                CloseDate = ticket.CloseDate,
                CreatedAt = ticket.CreatedAt,
                CreatedBy = ticket.CreatedBy,
                CreatedFrom = ticket.CreatedFrom,
                Csagent = ticket.Csagent,
                Customer = ticket.Customer,
                CustomerId = ticket.CustomerId,
                DeletedAt = ticket.DeletedAt,
                Details = ticket.Details,
                Duedate = ticket.Duedate,
                Group = ticket.Group,
                GroupId = ticket.GroupId,
                Id = ticket.Id,
                IsActive = ticket.IsActive,
                Priority = ticket.Priority,
                Status = ticket.Status,
                TicketCategory = ticket.TicketCategory,
                TicketCategoryId = ticket.TicketCategoryId,
                Title = ticket.Title,
                UpdatedAt = ticket.UpdateAt,
                ProfilePicture = customerCheckIn?.Logo,
                CustomerName = string.Format("{0} {1}", customerUser.FirstName, customerUser.LastName),
                Email = customerUser.Email,
            };
            await _hubContext.Clients.Group($"user-2-{customerUser.Id}").SendAsync("GetNewTicket", objticket);
            await _hubContext.Clients.Group($"user-1-{request.UserId}").SendAsync("GetNewTicket", objticket);
        }
        if (request.UserType == UserTypeEnum.Customer)
        {
            users = await _db.Users.ToListAsync(cancellationToken);
            foreach (var userItem in users)
            {
                var objtickets = new
                {
                    CSAgentId = ticket.CSAgentId,
                    CloseDate = ticket.CloseDate,
                    CreatedAt = ticket.CreatedAt,
                    CreatedBy = ticket.CreatedBy,
                    CreatedFrom = ticket.CreatedFrom,
                    Csagent = ticket.Csagent,
                    Customer = ticket.Customer,
                    CustomerId = ticket.CustomerId,
                    DeletedAt = ticket.DeletedAt,
                    Details = ticket.Details,
                    Duedate = ticket.Duedate,
                    Group = ticket.Group,
                    GroupId = ticket.GroupId,
                    Id = ticket.Id,
                    IsActive = ticket.IsActive,
                    Priority = ticket.Priority,
                    Status = ticket.Status,
                    TicketCategory = ticket.TicketCategory,
                    TicketCategoryId = ticket.TicketCategoryId,
                    Title = ticket.Title,
                    UpdatedAt = ticket.UpdateAt,
                    ProfilePicture = customerCheckIn?.Logo,
                    CustomerName = string.Format("{0} {1}", userItem.FirstName, userItem.LastName),
                    Email = userItem.Email,
                };
                await _hubContext.Clients.Group($"user-1-{userItem.Id}").SendAsync("GetNewTicket", objtickets);
                await _hubContext.Clients.Group($"user-2-{request.UserId}").SendAsync("GetNewTicket", objtickets);
            }
        }

        return _response.Success(new CreateTicketOut("Create ticket successful."));
    }
}

