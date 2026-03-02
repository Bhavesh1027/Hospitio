using MediatR;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.CreateCustomerPropertyExtras;
public record CreateCustomerPropertyExtrasRequest(CreateCustomerPropertyExtrasIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerPropertyExtrasHandler : IRequestHandler<CreateCustomerPropertyExtrasRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public CreateCustomerPropertyExtrasHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerPropertyExtrasRequest request, CancellationToken cancellationToken)
    {
        //var checkExist = await _db.CustomerPropertyExtras.Where(e => e.Name == request.In.Name).FirstOrDefaultAsync(cancellationToken);
        //if (checkExist != null)
        //{
        //    return _response.Error($"The Customer Property Extra {request.In.Name} already exists.", AppStatusCodeError.UnprocessableEntity422);
        //}

        var customerPropertyExtra = new CustomerPropertyExtra()
        {
            CustomerPropertyInformationId = request.In.CustomerPropertyInformationId,
            ExtraType = request.In.ExtraType,
            Name = request.In.Name,
            IsActive = request.In.IsActive
        };

        await _db.CustomerPropertyExtras.AddAsync(customerPropertyExtra, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new CreateCustomerPropertyExtrasOut("Create customer property extra successful.", new()
        {
            Id = customerPropertyExtra.Id
        }));

    }
}
