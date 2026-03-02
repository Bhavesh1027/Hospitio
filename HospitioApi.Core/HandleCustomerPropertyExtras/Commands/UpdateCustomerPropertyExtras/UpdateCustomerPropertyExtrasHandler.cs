using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerPropertyExtras.Commands.UpdateCustomerPropertyExtras;
public record UpdateCustomerPropertyExtrasRequest(UpdateCustomerPropertyExtrasIn In) : IRequest<AppHandlerResponse>;
public class UpdateCustomerPropertyExtrasHandler : IRequestHandler<UpdateCustomerPropertyExtrasRequest, AppHandlerResponse>
{
    private readonly ICommonDataBaseOprationService _common;
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;

    public UpdateCustomerPropertyExtrasHandler(ApplicationDbContext db, ICommonDataBaseOprationService common, IHandlerResponseFactory response)
    {
        _common = common;
        _db = db;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(UpdateCustomerPropertyExtrasRequest request, CancellationToken cancellationToken)
    {
        foreach(var item in request.In.CustomerPropertyExtrasIns)
        {
            var customerPropertyExtrasInResponse = await _common.CustomerPropertyExtraAddEdit(item, _db, cancellationToken);

            if (item.CustomerPropertyExtraDetailsOuts.Any())
            {
                var customerPropertyExtraDetailsResponse = await _common.CustomerPropertyExtraDetailsAddEdit(item.CustomerPropertyExtraDetailsOuts,customerPropertyExtrasInResponse.Id,_db, cancellationToken);
            }
        }
        await _db.SaveChangesAsync(cancellationToken);
        return _response.Success(new UpdateCustomerPropertyExtrasOut("Update customer property extra successful."));
    }
}
