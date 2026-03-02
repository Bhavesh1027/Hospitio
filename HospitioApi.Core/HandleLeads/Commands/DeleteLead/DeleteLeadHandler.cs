using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleLeads.Commands.DeleteLead;

public record DeleteLeadRequest(DeleteLeadIn In)
    : IRequest<AppHandlerResponse>;

public class DeleteUnitHandler : IRequestHandler<DeleteLeadRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public DeleteUnitHandler(
        ApplicationDbContext db,
        IHandlerResponseFactory response
        )
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteLeadRequest request, CancellationToken cancellationToken)
    {


        var lead = await _db.Leads.Where(c => c.Id == request.In.LeadId).SingleOrDefaultAsync(cancellationToken);
        if (lead == null)
        {
            return _response.Error($"Lead with id {request.In.LeadId} not found or user doesn't have the rights to delete it", AppStatusCodeError.Gone410);
        }


        _db.Leads.Remove(lead);
        await _db.SaveChangesAsync(cancellationToken);

        RemoveLeadOut deleteLeadOut = new() { DeletedLeadId = request.In.LeadId };
        return _response.Success(new DeleteLeadOut("Delete lead successful.", deleteLeadOut));
    }
}