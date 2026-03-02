using MediatR;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;

namespace HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.CreateCustomerEnhanceYourStay;
public record CreateCustomerEnhanceYourStayRequest(CreateCustomerEnhanceYourStayIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerEnhanceYourStayHandler : IRequestHandler<CreateCustomerEnhanceYourStayRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _commonRepository;

    public CreateCustomerEnhanceYourStayHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository)
    {
        _db = db;
        _response = response;
        _commonRepository = commonRepository;
    }

    public async Task<AppHandlerResponse> Handle(CreateCustomerEnhanceYourStayRequest request, CancellationToken cancellationToken)
    {
        var customerReceptionCategory = await _commonRepository.CustomerGuestAppEnhanceYourStay(request.In, _db, cancellationToken);

        var createCustomerEnhanceYourStayCategoryOut = new CreatedCustomerEnhanceYourStayOut
        {
            CustomerGuestAppBuilderId = request.In.CustomerGuestAppBuilderId,
            CustomerId = request.In.CustomerId
        };

        return _response.Success(new CreateCustomerEnhanceYourStayOut("Create customer enhance your stay category successful.", createCustomerEnhanceYourStayCategoryOut));
    }
}
