using Dapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.Chat.Models;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerRoomNames.Queries.GetCustomerRoomNames;
public record GetCustomerRoomNamesRequest(string CustomerId) : IRequest<AppHandlerResponse>;
public class GetCustomerRoomNamesHandler : IRequestHandler<GetCustomerRoomNamesRequest,AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IChatService _chatService;
    private readonly ApplicationDbContext _db;

    public GetCustomerRoomNamesHandler(IDapperRepository dapper, IHandlerResponseFactory response, IHubContext<ChatHub> hubContext,IChatService chatService, ApplicationDbContext db)
    {
        _dapper = dapper;
        _response = response;
        _hubContext = hubContext;
        _chatService = chatService;
        _db = db;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerRoomNamesRequest request,CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();
        spParams.Add("CustomerId", request.CustomerId, DbType.Int32);

        var customerRoom = await _dapper.GetAll<CustomerAppBuilders>("[dbo].[GetCustomerRoomNames]", spParams, cancellationToken, CommandType.StoredProcedure);

        if (customerRoom == null || customerRoom.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }


        // SEND Guest Portal Status in GetCustomerOnboardingStatus Event

        var guestPoratalCustomerUsersList = await (from cu in _db.CustomerUsers join cup in _db.CustomerUsersPermissions on cu.Id equals cup.CustomerUserId into cups from cup in cups.DefaultIfEmpty() join cp in _db.CustomerPermissions on cup.CustomerPermissionId equals cp.Id into cps from cp in cps.DefaultIfEmpty() where cu.CustomerId == int.Parse(request.CustomerId) && cu.DeletedAt == null && cu.IsActive == true && (cu.CustomerLevelId == 1 || (cup != null && (cup.IsView == true || cup.IsEdit == true) && cp.NormalizedName == "GuestPortal")) select cu.Id).ToListAsync(cancellationToken);
        
        if (guestPoratalCustomerUsersList != null || guestPoratalCustomerUsersList.Count != 0)
        {
            List<CustomerOnboardingStatus> customerOnboardingStatuses = new List<CustomerOnboardingStatus>();

            var guestProtalStatus =await _chatService.GetGuestPortalStatus(customerRoom , _db , request.CustomerId);

            customerOnboardingStatuses.Add(guestProtalStatus);

            foreach (var user in guestPoratalCustomerUsersList)
            {
                await _hubContext.Clients.Group($"user-2-{user}").SendAsync("GetCustomerOnboardingStatus", customerOnboardingStatuses);
            }
        }
        return _response.Success(new GetCustomerRoomNamesOut("Get customer room name successful.", customerRoom));
    }
}
