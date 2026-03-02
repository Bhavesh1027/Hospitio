using MediatR;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomersConcierge.Commands.UpdateCustomerConcierge;
public record UpdateCustomerConciergeRequest(UpdateCustomerConciergeIn In,string CustomerId) : IRequest<AppHandlerResponse>;
public class UpdateCustomerConciergeHandler : IRequestHandler<UpdateCustomerConciergeRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;
    public UpdateCustomerConciergeHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomerConciergeRequest request, CancellationToken cancellationToken)
    {
        var customerConciergeCategory = await _commonRepository.CustomersConciergeMultipleUpdateWithItems(request.In.CustomerConciergeCategories, Convert.ToInt32(request.CustomerId),_db, cancellationToken);

        //var updatedCustomerConciergeOutList = new List<UpdatedCustomerConciergeOut>();
        //foreach (var category in customerConciergeCategory)
        //{
            var updatedCustomerConciergeOut = new UpdatedCustomerConciergeOut()
            {

                Id = customerConciergeCategory.Id,
                CustomerGuestAppBuilderId = customerConciergeCategory.CustomerGuestAppBuilderId,
                CategoryName = customerConciergeCategory.CategoryName,
                DisplayOrder = customerConciergeCategory.DisplayOrder,
            };
        //    updatedCustomerConciergeOutList.Add(updatedCustomerConciergeOut);
        //}


        return _response.Success(new UpdateCustomerConciergeOut("Update customer concierge category successful.", updatedCustomerConciergeOut));
    }
}
