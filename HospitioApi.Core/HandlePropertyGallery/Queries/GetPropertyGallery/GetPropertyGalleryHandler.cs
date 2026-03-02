using Dapper;
using MediatR;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;
using static HospitioApi.Core.HandlePropertyGallery.Queries.GetPropertyGallery.GetPropertyGalleryOut;

namespace HospitioApi.Core.HandlePropertyGallery.Queries.GetPropertyGallery;
public record GetPropertyGalleryRequest(GetPropertyGalleryIn In)
    : IRequest<AppHandlerResponse>;

public class GetPropertyGalleryHandler : IRequestHandler<GetPropertyGalleryRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IDapperRepository _dapper;
    public GetPropertyGalleryHandler(IDapperRepository dapper,
        ApplicationDbContext db,
        IHandlerResponseFactory response)
    {
        _db = db;
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetPropertyGalleryRequest request, CancellationToken cancellationToken)
    {
        var spParams = new DynamicParameters();

        spParams.Add("CustomerPropertyInformationId", request.In.CustomerPropertyInformationId, DbType.Int32);

        var dapperOuts = await _dapper.GetAll<PropertyGalleryOut>("[dbo].[GetCustPropGalleryById]"
                                    , spParams, cancellationToken,
        commandType: CommandType.StoredProcedure);

       dapperOuts = dapperOuts.Where(e => e.IsDeleted == false).ToList();

        if (dapperOuts == null || dapperOuts.Count == 0)
        {
            return _response.Error("Data not available", AppStatusCodeError.Forbidden403);
        }

        return _response.Success(new GetPropertyGalleryOut("Get property images successfully.", dapperOuts!));
    }
}