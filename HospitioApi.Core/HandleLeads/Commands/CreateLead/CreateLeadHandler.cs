using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleLeads.Commands.CreateLead
{
    public record CreateLeadRequest(CreateLeadIn In)
    : IRequest<AppHandlerResponse>;

    public class CreateLeadHandler : IRequestHandler<CreateLeadRequest, AppHandlerResponse>
    {
        private readonly ApplicationDbContext _db;
        private readonly IHandlerResponseFactory _response;
        public CreateLeadHandler(
            ApplicationDbContext db,
            IHandlerResponseFactory response)
        {
            _db = db;
            _response = response;
        }
        public async Task<AppHandlerResponse> Handle(CreateLeadRequest request, CancellationToken cancellationToken)
        {
            if (request.In == null)
            {
                return _response.Error($"Request cannot be null.", AppStatusCodeError.Forbidden403);
            }
            var lead = new Lead();
            lead.FirstName = request.In.FirstName;
            lead.LastName = request.In.LastName;
            lead.Company = request.In.Company;
            lead.Email = request.In.Email;
            lead.Comment = request.In.Comment;
            lead.PhoneNumber = request.In.PhoneNumber;
            lead.PhoneCountry = request.In.PhoneCountry;
            lead.ContactFor = request.In.ContactFor;
            lead.IsActive = request.In.IsActive;

            await _db.Leads.AddAsync(lead, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            var createdLeadOut = new CreatedLeadOut()
            {
                FirstName = request.In.FirstName,
                LastName = request.In.LastName,
                Company = request.In.Company,
                Email = request.In.Email,
                Comment = request.In.Comment,
                ContactFor = request.In.ContactFor,
                IsActive = request.In.IsActive,
                PhoneCountry = request.In.PhoneCountry,
                PhoneNumber = request.In.PhoneNumber
            };
            return _response.Success(new CreateLeadOut("Lead created successfully.", createdLeadOut));
        }
    }
}
