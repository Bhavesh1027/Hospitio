using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleTicketCategories.Commands.CreateTicketCategory;
public record CreateTicketCategoryRequest(CreateTicketCategoryIn In) : IRequest<AppHandlerResponse>;

public class CreateTicketCategoryHandler : IRequestHandler<CreateTicketCategoryRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public CreateTicketCategoryHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateTicketCategoryRequest request, CancellationToken cancellationToken)
    {
        var checkExits = await _db.TicketCategorys.Where(e => e.CategoryName == request.In.Name).FirstOrDefaultAsync(cancellationToken);
        if (checkExits != null)
        {
            return _response.Error($"The ticket category {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
        }
        var ticketCategory = new TicketCategory
        {
            CategoryName = request.In.Name,
            IsActive = request.In.IsActive

        };
        await _db.TicketCategorys.AddAsync(ticketCategory, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        var createTicketCategoryOut = new CreatedTicketCategoryOut
        {
            Id = ticketCategory.Id,
            Name = ticketCategory.CategoryName
        };
        return _response.Success(new CreateTicketCategoryOut("Create ticket category successful.", createTicketCategoryOut));
    }
}
