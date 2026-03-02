using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleLeads.Commands.EditLead
{
    public record EditLeadRequest(EditLeadIn In)
    : IRequest<AppHandlerResponse>;

    public class EditLeadHandler : IRequestHandler<EditLeadRequest, AppHandlerResponse>
    {
        private readonly ApplicationDbContext _db;
        private readonly IHandlerResponseFactory _response;
        public EditLeadHandler(
            ApplicationDbContext db,
            IHandlerResponseFactory response)
        {
            _db = db;
            _response = response;
        }
        public async Task<AppHandlerResponse> Handle(EditLeadRequest request, CancellationToken cancellationToken)
        {
            var checkLead = await _db.Leads.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
            if (checkLead == null)
            {
                return _response.Error($"Lead could not be found.", AppStatusCodeError.Gone410);
            }

            checkLead.FirstName = request.In.FirstName;
            checkLead.LastName = request.In.LastName;
            checkLead.Email = request.In.Email;
            checkLead.Company = request.In.Company;
            checkLead.Comment = request.In.Comment;
            checkLead.PhoneNumber = request.In.PhoneNumber;
            checkLead.PhoneCountry = request.In.PhoneCountry;
            checkLead.ContactFor = request.In.ContactFor;
            checkLead.IsActive = request.In.IsActive;

            await _db.SaveChangesAsync(cancellationToken);

            var editedLeadOut = new EditedLeadOut()
            {
                FirstName = request.In.FirstName,
                LastName = request.In.LastName,
                Email = request.In.Email,
                Company = request.In.Company,
                Comment = request.In.Comment,
                ContactFor = request.In.ContactFor,
                IsActive = request.In.IsActive,
                PhoneCountry = request.In.PhoneCountry,
                PhoneNumber = request.In.PhoneNumber
            };
            return _response.Success(new EditLeadOut("Lead edited successfully.", editedLeadOut));
        }
    }
}
