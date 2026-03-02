using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.CreateCustomerAppBuilder;
public record CreateCustomerGuestAppBuilderRequest(CreateCustomerGuestAppBuilderIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerGuestAppBuilderHandler : IRequestHandler<CreateCustomerGuestAppBuilderRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public CreateCustomerGuestAppBuilderHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateCustomerGuestAppBuilderRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.CustomerRoomNames.Where(e => e.Id == request.In.CustomerRoomNameId && e.CustomerId == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
        if (checkExist == null)
        {
            return _response.Error($"The customer room not found.", AppStatusCodeError.UnprocessableEntity422);
        }

        var checkDuplicate = await _db.CustomerGuestAppBuilders.Where(e => e.CustomerRoomNameId == request.In.CustomerRoomNameId).ToListAsync(cancellationToken);
        if (checkDuplicate.Count > 1)
        {
            return _response.Error($"The customer guest app builder alreday exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        var customerGuestAppBuilder = new CustomerGuestAppBuilder
        {
            CustomerId = request.In.CustomerId,
            CustomerRoomNameId = request.In.CustomerRoomNameId,
            Concierge = request.In.Concierge,
            Ekey = request.In.Ekey,
            EnhanceYourStay = request.In.EnhanceYourStay,
            Housekeeping = request.In.Housekeeping,
            LocalExperience = request.In.LocalExperience,
            Message = request.In.Message,
            PropertyInfo = request.In.PropertyInfo,
            Reception = request.In.Reception,
            RoomService = request.In.RoomService,
            SecondaryMessage = request.In.SecondaryMessage,
            TransferServices = request.In.TransferServices,
            IsActive = request.In.IsActive
        };

        await _db.CustomerGuestAppBuilders.AddAsync(customerGuestAppBuilder);
        await _db.SaveChangesAsync(cancellationToken);
        var createdCustomerGuestAppBuilderOut = new CreatedCustomerGuestAppBuilderOut
        {
            Id = customerGuestAppBuilder.Id,
            CustomerId = customerGuestAppBuilder.CustomerId,
            CustomerRoomNameId = customerGuestAppBuilder.CustomerRoomNameId,
            Concierge = customerGuestAppBuilder.Concierge,
            Ekey = customerGuestAppBuilder.Ekey,
            EnhanceYourStay = customerGuestAppBuilder.EnhanceYourStay,
            Housekeeping = customerGuestAppBuilder.Housekeeping,
            LocalExperience = customerGuestAppBuilder.LocalExperience,
            Message = customerGuestAppBuilder.Message,
            PropertyInfo = customerGuestAppBuilder.PropertyInfo,
            Reception = customerGuestAppBuilder.Reception,
            RoomService = customerGuestAppBuilder.RoomService,
            SecondaryMessage = customerGuestAppBuilder.SecondaryMessage,
            TransferServices = customerGuestAppBuilder.TransferServices,
            IsActive = customerGuestAppBuilder.IsActive
        };

        return _response.Success(new CreateCustomerGuestAppBuilderOut("Create customer guest app builder successful.", createdCustomerGuestAppBuilderOut));
    }
}
