using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomerPropertyService.Commands.CreateCustomerPropertyService;
public record CreateCustomerPropertyServiceRequest(CreateCustomerPropertyServiceIn In) : IRequest<AppHandlerResponse>;
public class CreateCustomerPropertyServiceHandler : IRequestHandler<CreateCustomerPropertyServiceRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly ICommonDataBaseOprationService _comman;
    public CreateCustomerPropertyServiceHandler(ApplicationDbContext db, IHandlerResponseFactory response,ICommonDataBaseOprationService common)
    {
        _db = db;
        _comman = common;
         _response = response;
    }
    public async Task<AppHandlerResponse> Handle(CreateCustomerPropertyServiceRequest request, CancellationToken cancellationToken)
    {
        var checkExist = await _db.CustomerPropertyInformations.Where(e => e.Id == request.In.CustomerPropertyInformationId).FirstOrDefaultAsync(cancellationToken);
        if (checkExist == null)
        {
            return _response.Error($"The customer property information not found.", AppStatusCodeError.UnprocessableEntity422);
        }

        //var checkDuplicate = await _db.CustomerPropertyServices.Where(e => e.CustomerPropertyInformationId == request.In.CustomerPropertyInformationId).FirstOrDefaultAsync(cancellationToken);
        //if (checkDuplicate != null)
        //{
        //    return _response.Error($"The customer property service already exists.", AppStatusCodeError.UnprocessableEntity422);
        //}

        var customerPropertService = await _comman.CustomerPropertyServiceAdd(request.In, _db, cancellationToken);
       
        if(request.In.CustomerPropertyInfoServiceImagesOuts.Any())
        {
            var customerPropertyserviceImageList = await _comman.CustomerPropertyServiceImageAdd(request.In.CustomerPropertyInfoServiceImagesOuts, customerPropertService.Id,_db, cancellationToken);
        }

        var createdCustomerPropertyServiceOut = new CreatedCustomerPropertyServiceOut
        {
            Id = customerPropertService.Id,
            CustomerPropertyInformationId = request.In.CustomerPropertyInformationId,
            Name = request.In.Name
        };

        return _response.Success(new CreateCustomerPropertyServiceOut("Create customer property service successful.", createdCustomerPropertyServiceOut));
    }
}
