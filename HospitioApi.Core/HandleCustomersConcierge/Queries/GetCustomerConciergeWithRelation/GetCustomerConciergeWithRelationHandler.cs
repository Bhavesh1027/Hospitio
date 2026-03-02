using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;
using Vonage.Common.Monads;

namespace HospitioApi.Core.HandleCustomersConcierge.Queries.GetCustomerConciergeWithRelation;
public record GetCustomerConciergeWithRelationRequest(GetCustomerConciergeWithRelationIn In,string CustomerId,string UserType) : IRequest<AppHandlerResponse>;
public class GetCustomerConciergeWithRelationHandler : IRequestHandler<GetCustomerConciergeWithRelationRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerConciergeWithRelationHandler(IDapperRepository dapper, IHandlerResponseFactory response, ApplicationDbContext db)
    {
        _db = db;
        _dapper = dapper;
        _response = response;
    }
    public async Task<AppHandlerResponse> Handle(GetCustomerConciergeWithRelationRequest request, CancellationToken cancellationToken)
    {
        //List<CustomerConciergeWithRelationOut> customerConciergeWithRelationOuts = new List<CustomerConciergeWithRelationOut>();
        //CustomerConciergeWithRelationOut customerConciergeWithRelationOut = new CustomerConciergeWithRelationOut();

        var spParams = new DynamicParameters();
        spParams.Add("AppBuilderId", request.In.AppBuilderId, DbType.Int32);
        spParams.Add("UserType", request.UserType != null ? request.UserType: 3, DbType.Int32);
        //spParams.Add("CustomerId", request.CustomerId, DbType.Int32);

        var getCustomerConciergeWithRelationOuts = await _dapper.GetAllJsonData<CustomerConciergeWithRelationOut>("[dbo].[GetCustomerConciergeWithRelationSP]"
      , spParams, cancellationToken,
      commandType: CommandType.StoredProcedure);

        var getCustomerConciergeWithRelationOutsFinal = getCustomerConciergeWithRelationOuts.Where(e => e.IsDeleted == false).ToList();

        if (getCustomerConciergeWithRelationOutsFinal == null || getCustomerConciergeWithRelationOutsFinal.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Gone410);
        }

        return _response.Success(new GetCustomerConciergeWithRelationOut("Get customer concierge successful.", getCustomerConciergeWithRelationOutsFinal));
    }
}


