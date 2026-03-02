using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleTicketCategories.Commands.UpdateTicketCategory;
public record UpdateTicketCategoryRequest(UpdateTicketCategoryIn In) : IRequest<AppHandlerResponse>;
public class UpdateTicketCategoryHandler : IRequestHandler<UpdateTicketCategoryRequest, AppHandlerResponse>
{
    private readonly IHandlerResponseFactory _response;
    private readonly ApplicationDbContext _db;
    public UpdateTicketCategoryHandler(IHandlerResponseFactory response, ApplicationDbContext db)
    {
        _response = response;
        _db = db;
    }
    public async Task<AppHandlerResponse> Handle(UpdateTicketCategoryRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.TicketCategorys.Where(e => e.CategoryName == request.In.CategoryName && e.Id != request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (checkExist != null)
        {
            return _response.Error($"The ticket category {request.In.CategoryName} already exists.", AppStatusCodeError.UnprocessableEntity422);
        }
        var ticketCategory = await _db.TicketCategorys.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (ticketCategory == null)
        {
            return _response.Error($"Ticket category with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
        }
        ticketCategory.CategoryName = request.In.CategoryName;
        ticketCategory.IsActive = request.In.IsActive;
        await _db.SaveChangesAsync(cancellationToken);
        var updateTicketategoryOut = new UpdatedTicketCategoryOut()
        {
            Id = request.In.Id,
            CategoryName = request.In.CategoryName,
            IsActive = request.In.IsActive,
        };
        return _response.Success(new UpdateTicketCategoryOut("Update ticket category successful.", updateTicketategoryOut));
    }
}
