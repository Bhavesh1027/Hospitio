using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.Services.SendEmail;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandleCustomers.Commands.UpdateCustomer;
public record UpdateCustomerRequest(UpdateCustomerIn In, UserTypeEnum UserType, string CustomerId) : IRequest<AppHandlerResponse>;
public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerRequest, AppHandlerResponse>
{
	private readonly ApplicationDbContext _db;
	private readonly IHandlerResponseFactory _response;
	private readonly ICommonDataBaseOprationService _commonRepository;
	private readonly ISendEmail _mail;

	public UpdateCustomerHandler(ApplicationDbContext db, IHandlerResponseFactory response, ICommonDataBaseOprationService commonRepository, ISendEmail mail)
	{
		_db = db;
		_response = response;
		_commonRepository = commonRepository;
		_mail = mail;
	}

	public async Task<AppHandlerResponse> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
	{
		var customer = new Customer();
		var customerFinal = new Customer();
		if (request.UserType == UserTypeEnum.Customer)
		{
			customer = await _db.Customers.Where(e => e.Id == Int32.Parse(request.CustomerId)).FirstOrDefaultAsync(cancellationToken);
		}
		else if (request.UserType == UserTypeEnum.Hospitio)
		{
			customer = await _db.Customers.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
		}

		if (customer == null)
		{
			return _response.Error($"Customer with Id {request.In.Id} could not be found.", AppStatusCodeError.Gone410);
		}
		if (request.UserType == UserTypeEnum.Hospitio)
		{
			if (customer.PhoneNumber != request.In.PhoneNumber)
			{
				request.In.IsTwoWayComunication = false;
			}
			customerFinal = await _commonRepository.CustomersUpdate(request.In, customer, _db, cancellationToken, _mail);

		}
		else
		{
			customerFinal = await _commonRepository.CustomersUpdate(request.In, customer, _db, cancellationToken, _mail);
		}
		if (customerFinal != null)
		{
			var PropertyInfoData = await _db.CustomerPropertyInformations.Where(i => i.CustomerId == customerFinal.Id).ToListAsync();
			if (PropertyInfoData != null || PropertyInfoData!.Count > 0)
			{

				foreach (var item in PropertyInfoData)
				{
					item.Latitude = customerFinal.Latitude;
					item.Longitude = customerFinal.Longitude;

					await _db.SaveChangesAsync(cancellationToken);
				}

			}
		}
		if (request.UserType == UserTypeEnum.Customer)
		{
			if (request.In.UpdateCustomerRoomNamesIns != null)
			{
				bool roomExistsInTable = request.In.UpdateCustomerRoomNamesIns.Any(req => _db.CustomerRoomNames.Any(room => room.Name == req.Name && room.CustomerId == Int32.Parse(request.CustomerId) && room.Id != req.Id));
				if (roomExistsInTable)
					return _response.Error($"Room name already exists.", AppStatusCodeError.UnprocessableEntity422);
				bool areAllLocationCodesValid = request.In.UpdateCustomerRoomNamesIns.Any(customer =>
				(customer.CenturionLocationCode == null || Guid.TryParse(customer.CenturionLocationCode.ToString(), out _)));
				if (!areAllLocationCodesValid)
					return _response.Error($"location ID is not a valid GUID.", AppStatusCodeError.UnprocessableEntity422);
				bool locationCodeExistInTable = request.In.UpdateCustomerRoomNamesIns.Any(req => req.CenturionLocationCode != null && req.CenturionLocationCode != "" && _db.CustomerRoomNames.Any(room => room.Guid == Guid.Parse(req.CenturionLocationCode) && room.Id != req.Id));
				if (locationCodeExistInTable)
					return _response.Error($"Location ID already exists.", AppStatusCodeError.UnprocessableEntity422);
				bool ReferenceAccommodationcodeExistInTable = request.In.UpdateCustomerRoomNamesIns.Any(req => req.Gui != null && req.Gui != "" && _db.CustomerRoomNames.Any(room => room.AvantioAccomodationRefrence == req.Gui && room.Id != req.Id));
				if (ReferenceAccommodationcodeExistInTable)
					return _response.Error($"Reference Accomodation Code already exists.", AppStatusCodeError.UnprocessableEntity422);
				var customerRoomName = await _commonRepository.CustomerRoomNamesUpdate(request.In.UpdateCustomerRoomNamesIns, customer.Id, _db, cancellationToken, request.UserType);
			}
		}

		return _response.Success(new UpdateCustomerOut("Customer update successful.", new() { CustomerId = customer.Id }));
	}
}
