using MediatR;

using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;

namespace HospitioApi.Core.HandlePMS.Queries.GetPMS;
public record GetPMSRequest(int Id) : IRequest<AppHandlerResponse>;
public class GetPMSHandler : IRequestHandler<GetPMSRequest, AppHandlerResponse>
{
	private readonly IDapperRepository _dapper;
	private readonly IHandlerResponseFactory _response;
	private readonly ApplicationDbContext _db;
	public GetPMSHandler(IDapperRepository dapper, IHandlerResponseFactory response, ApplicationDbContext db)
	{
		_dapper = dapper;
		_response = response;
		_db = db;
	}
	public async Task<AppHandlerResponse> Handle(GetPMSRequest request, CancellationToken cancellationToken)
	{
		//var spParams = new DynamicParameters();


		//// SP Name is [GetBusinessTypes]
		//var businessType = await _dapper
		//    .GetAll<BusinessTypesOut>("[dbo].[GetBusinessTypes]", spParams, cancellationToken, commandType: System.Data.CommandType.StoredProcedure);

		var pms = new List<PMSOut>();

		foreach (PMS type in Enum.GetValues(typeof(PMS)))
		{
			var pMS = new PMSOut
			{
				Id = (int)type,
				PMS = type.ToString()
			};

			int? businesstypeid = await _db.Customers.Where(c => c.Id == request.Id).Select(c => c.BusinessTypeId).FirstOrDefaultAsync(cancellationToken);
			if (businesstypeid != 3 && ((int)type == 3 || (int)type == 6))
			{
				continue;
			}

			pms.Add(pMS);
		}


		if (pms == null || pms.Count == 0)
		{
			return _response.Error("Data not available", AppStatusCodeError.Gone410);
		}

		return _response.Success(new GetPMSOut("Get pms successful.", pms));

	}
}
