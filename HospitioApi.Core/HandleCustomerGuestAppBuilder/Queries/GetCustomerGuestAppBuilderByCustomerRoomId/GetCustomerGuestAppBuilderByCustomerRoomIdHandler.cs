using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using HospitioApi.Core.Services.Dapper;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Data;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Data;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Queries.GetCustomerGuestAppBuilderByCustomerRoomId;
public record GetCustomerGuestAppBuilderByCustomerRoomIdRequest(GetCustomerGuestAppBuilderByCustomerRoomIdIn In) : IRequest<AppHandlerResponse>;
public class GetCustomerGuestAppBuilderByCustomerRoomIdHandler : IRequestHandler<GetCustomerGuestAppBuilderByCustomerRoomIdRequest, AppHandlerResponse>
{
    private readonly IDapperRepository _dapper;
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    public GetCustomerGuestAppBuilderByCustomerRoomIdHandler(IDapperRepository dapper, IHandlerResponseFactory response, ApplicationDbContext db)
    {
        _db = db;
        _dapper = dapper;
        _response = response;
    }

    public async Task<AppHandlerResponse> Handle(GetCustomerGuestAppBuilderByCustomerRoomIdRequest request, CancellationToken cancellationToken)
    {
        bool isLocalEperienceActive = false;
        var customer = await _db.Customers.Where(c => c.Id == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
        if (customer != null)
        {
            int? productId = customer.ProductId;
            var moduleService = await _db.ModuleServices.Where(p => p.Name == "Local Experiences").FirstOrDefaultAsync(cancellationToken);

            if (productId.HasValue)
            {
                var productModuleService = await _db.ProductModuleServices.Where(p => p.ProductId == productId && p.ModuleServiceId == moduleService.Id).FirstOrDefaultAsync(cancellationToken);
                isLocalEperienceActive = productModuleService.IsActive ?? false;
            }

            if (!productId.HasValue)
            {
                return _response.Error($"You have not allocated any service. Please contact administrator.", AppStatusCodeError.PaymentRequired402);
            }
        }

        var roomId = await _db.CustomerRoomNames.Where(e => e.Id == request.In.RoomId && e.CustomerId == request.In.CustomerId).FirstOrDefaultAsync(cancellationToken);
        if (roomId == null)
        {
            return _response.Error($"The customer room not found.", AppStatusCodeError.UnprocessableEntity422);
        }

        #region Assigne Product Data
        var spParams2 = new DynamicParameters();
        spParams2.Add("RoomId", request.In.RoomId, DbType.Int32);
        spParams2.Add("CustomerId", request.In.CustomerId, DbType.Int32);
        var servicefor = await _dapper.GetAll<ModuleServiceOut>("[dbo].[AddAppBulider]", spParams2, cancellationToken, CommandType.StoredProcedure);
        #endregion

        var refrenceId = servicefor.Select(e => e.customerAppBuliderId).FirstOrDefault();

        var displayOrder = await _db.ScreenDisplayOrderAndStatuses.Where(e => e.ScreenName == Convert.ToInt32(ScreenDisplayOrder.GuestPortalBuilder) && e.RefrenceId == refrenceId).FirstOrDefaultAsync(cancellationToken);
        dynamic result = new object();

        if (request.In.UserType == "Guest")
        {

            result = System.Text.Json.JsonSerializer.Deserialize<List<ModuleServiceOut>>(displayOrder!.JsonData!);
        }
        else
        {
            if (displayOrder.ScreenJsonData != null)
            {
                result = System.Text.Json.JsonSerializer.Deserialize<List<ModuleServiceOut>>(displayOrder!.ScreenJsonData!);
            }
            else
            {
                result = System.Text.Json.JsonSerializer.Deserialize<List<ModuleServiceOut>>(displayOrder!.JsonData!);
            }
        }

        List<ModuleServiceOut> moduleServiceOut = new List<ModuleServiceOut>();

        foreach (var data in result!)
        {
            var spParams = new DynamicParameters();
            spParams.Add("CustomerAppBuliderId", refrenceId, DbType.Int32);
            spParams.Add("GuestService", data.name, DbType.String);
            spParams.Add("CustomerId", request.In.CustomerId, DbType.Int32);

            var extra = await _dapper.GetSingle<ModuleServiceOut>("[dbo].[GetExtradetails]", spParams, cancellationToken, CommandType.StoredProcedure);
            var serviceDeatils = servicefor.Select(e =>
            {
                if (data.name == "e-Keys")
                {
                    e.name = "e-Keys";
                }
                else if (data.name == "Online Check-In")
                {
                    e.name = "Online Check-In";
                }
                return e;
            }).FirstOrDefault(e => e.name == data.name);

            if (serviceDeatils != null)
            {
                bool isdisabled;
                if (data.name == "Local Experiences" && request.In.UserType == UserTypeEnum.Guest.ToString())
                {
                    isdisabled = false;
                }
                else if (data.name == "Local Experiences" && request.In.UserType == UserTypeEnum.Customer.ToString())
                {
                    isdisabled = true;
                }
                else
                {
                    isdisabled = false;
                }

                var moduleServiceOutIsEnable = new ModuleServiceOut()
                {
                    name = data.name,
                    displayOrder = data.displayOrder,
                    isDisable = isdisabled,
                    image = data.image,
                    items = extra.items,
                    categories = extra.categories,
                    customerAppBuliderId = serviceDeatils.customerAppBuliderId,
                };
                moduleServiceOut.Add(moduleServiceOutIsEnable);
            }
            else
            {
                bool isdisabled;
                if (data.name == "Local Experiences" && request.In.UserType == UserTypeEnum.Guest.ToString())
                {
                    isdisabled = false;
                }
                else if (data.name == "Local Experiences" && request.In.UserType == UserTypeEnum.Customer.ToString())
                {
                    isdisabled = true;
                }
                else
                {
                    isdisabled = true;
                }

                var moduleServiceOutIsEnable = new ModuleServiceOut()
                {
                    name = data.name,
                    displayOrder = data.displayOrder,
                    //isDisable = isdisabled,
                    isDisable = false,
                    image = data.image,
                    items = extra.items,
                    categories = extra.categories,
                    customerAppBuliderId = refrenceId,
                };
                moduleServiceOut.Add(moduleServiceOutIsEnable);
            }
        }

        var spParams3 = new DynamicParameters();
        spParams3.Add("RoomId", request.In.RoomId, DbType.Int32);
        spParams3.Add("CustomerId", request.In.CustomerId, DbType.Int32);
        spParams3.Add("UserType", request.In.UserType, DbType.String);
        var appbuilder = await _dapper.GetSingle<CustomerGuestAppBuilderByCustomerRoomIdOut>("[dbo].[GetAPPBuilderBasic]", spParams3, cancellationToken, CommandType.StoredProcedure);

        var customerGuestAppBuilderByCustomerRoomIdOut = new CustomerGuestAppBuilderByCustomerRoomIdOut()
        {
            Id = appbuilder.Id,
            Message = appbuilder.Message,
            SecondaryMessage = appbuilder.SecondaryMessage,
            //LocalExperience = appbuilder.LocalExperience,
            LocalExperience = isLocalEperienceActive,
            //LocalExperience = true,
            Ekey = appbuilder.Ekey,
            PropertyInfo = appbuilder.PropertyInfo,
            EnhanceYourStay = appbuilder.EnhanceYourStay,
            Reception = appbuilder.Reception,
            Housekeeping = appbuilder.Housekeeping,
            RoomService = appbuilder.RoomService,
            Concierge = appbuilder.Concierge,
            //TransferServices = appbuilder.TransferServices,
            TransferServices = true,
            OnlineCheckIn = appbuilder.OnlineCheckIn,
            IsActive = appbuilder.IsActive,
            ModuleServiceOut = moduleServiceOut
        };

        return _response.Success(new GetCustomerGuestAppBuilderByCustomerRoomIdOut("Get customer guest app builder successful.", customerGuestAppBuilderByCustomerRoomIdOut));
    }
}

