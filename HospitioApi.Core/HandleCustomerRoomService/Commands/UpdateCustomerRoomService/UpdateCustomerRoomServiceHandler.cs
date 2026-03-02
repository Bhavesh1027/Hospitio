using MediatR;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerRoomService.Commands.UpdateCustomerRoomService;
public record UpdateCustomerRoomServiceRequest(UpdateCustomerRoomServiceIn In,string CustomerId) : IRequest<AppHandlerResponse>;
public class UpdateCustomerRoomServiceHandler : IRequestHandler<UpdateCustomerRoomServiceRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;
    public UpdateCustomerRoomServiceHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(UpdateCustomerRoomServiceRequest request, CancellationToken cancellationToken)
    {
        var customerConciergeCategory = await _commonRepository.CustomersRoomServiceMultipleUpdateWithItems(request.In.UpdateCustomerRoomServiceCategoryIn, Convert.ToInt32(request.CustomerId),_db, cancellationToken);


        //var updatedCustRoomServiceListOut = new List<UpdatedCustomerRoomServiceOut>();
        //foreach (var category in customerConciergeCategory)
        //{
            var updatedCustomerRoomServiceOut = new UpdatedCustomerRoomServiceOut()
            {
                Id = customerConciergeCategory.Id,
                CustomerGuestAppBuilderId = customerConciergeCategory.CustomerGuestAppBuilderId,
                CategoryName = customerConciergeCategory.CategoryName,
                DisplayOrder = customerConciergeCategory.DisplayOrder,
            };
        //    updatedCustRoomServiceListOut.Add(updatedCustomerRoomServiceOut);
        //}
        return _response.Success(new UpdateCustomerRoomServiceOut("Create customer room service categories successful.", updatedCustomerRoomServiceOut));
    }
}
