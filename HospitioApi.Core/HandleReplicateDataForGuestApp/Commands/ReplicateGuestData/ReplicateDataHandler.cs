using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Common;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;
using static Azure.Core.HttpHeader;

namespace HospitioApi.Core.HandleReplicateDateForGuestApp.Commands.ReplicateGuestData;

public record ReplicateDataRequest(ReplicateDataIn In) : IRequest<AppHandlerResponse>;
public class ReplicateDataHandler : IRequestHandler<ReplicateDataRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly IHandlerResponseFactory _response;
    private readonly ApplicationDbContext _db;
    private readonly ICommonDataBaseOprationService _common;


    public ReplicateDataHandler(IDapperRepository dapper, IHandlerResponseFactory response, ApplicationDbContext applicationDbContext, ICommonDataBaseOprationService common)
    {
        _dapper = dapper;
        _response = response;
        _db = applicationDbContext;
        _common = common;

    }

    public async Task<AppHandlerResponse> Handle(ReplicateDataRequest request, CancellationToken cancellationToken)
    {
        
        var OldDataForBuilder = await _common.GetOldGuestAppBuilderData(request.In.NewBuilderId, cancellationToken, _dapper);
        var ReplicatedDataOuts = await _common.GetNewGusestAppBuilderData(request.In.AppBuilderId, cancellationToken, _dapper);
        bool DeleteGuestAppBuilderData = false;
        bool AddGuestData = false;
        if (OldDataForBuilder.Count > 0)
        {
            DeleteGuestAppBuilderData = await _common.DeleteGuestAppBuilderData(OldDataForBuilder, _db, cancellationToken);
        }

        if (DeleteGuestAppBuilderData)
        {
            if (ReplicatedDataOuts.Count > 0)
            {
                AddGuestData = await _common.AddGuestAppBuilderData(ReplicatedDataOuts, _db, cancellationToken, request.In);
            }
        }

        if (AddGuestData)
        {
            return _response.Success(new ReplicateDataOut("ReplicateData SuccessFully.."));
        }
       
        return _response.Error("Unexpected Error..", AppStatusCodeError.InternalServerError500);

    }
}

