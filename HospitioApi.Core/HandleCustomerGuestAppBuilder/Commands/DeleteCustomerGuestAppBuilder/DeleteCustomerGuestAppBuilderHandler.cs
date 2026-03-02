using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.DeleteCustomerAppBuilder;
public record DeleteCustomerGuestAppBuilderRequest(DeleteCustomerGuestAppBuilderIn In) : IRequest<AppHandlerResponse>;
public class DeleteCustomerGuestAppBuilderHandler : IRequestHandler<DeleteCustomerGuestAppBuilderRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public DeleteCustomerGuestAppBuilderHandler(ApplicationDbContext db, IHandlerResponseFactory response)
    {
        _db = db;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(DeleteCustomerGuestAppBuilderRequest request, CancellationToken cancellationToken)
    {
        var customerGuestAppBuilder = await _db.CustomerGuestAppBuilders.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerGuestAppBuilder == null)
        {
            return _response.Error($"Customer guest app builder could not be found.", AppStatusCodeError.Gone410);
        }

        customerGuestAppBuilder.DeletedAt = DateTime.UtcNow;
        _db.CustomerGuestAppBuilders.Update(customerGuestAppBuilder);
        await _db.SaveChangesAsync(cancellationToken);

        return _response.Success(new DeleteCustomerGuestAppBuilderOut("Delete customer guest app builder successful.", new() { Id = request.In.Id }));
    }
}
