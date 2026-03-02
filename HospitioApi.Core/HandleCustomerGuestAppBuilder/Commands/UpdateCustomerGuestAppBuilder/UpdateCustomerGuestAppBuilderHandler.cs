using MediatR;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using HospitioApi.Core.HandleCustomerEnhanceYourStay.Commands.CreateCustomerEnhanceYourStay;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.CreateCustomerEnhanceYourStayItem;
using HospitioApi.Core.HandleCustomerEnhanceYourStayItems.Commands.UpdateCustomerEnhanceYourStayItem;
using HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.CreateCustomerAppBuilder;
using HospitioApi.Core.HandleCustomerPropertyEmergencyNumbers.Commands.UpdateCustomerPropertyEmergencyNumber;
using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.DeleteCustomerPropertyExtraDetail;
using HospitioApi.Core.HandleCustomerPropertyExtras.Commands.UpdateCustomerPropertyExtras;
using HospitioApi.Core.HandleCustomerPropertyService.Commands.CreateCustomerPropertyService;
using HospitioApi.Core.HandleCustomerPropertyService.Commands.DeleteCustomerPropertyService;
using HospitioApi.Core.HandleCustomersConcierge.Commands.UpdateCustomerConcierge;
using HospitioApi.Core.HandleCustomersPropertiesInfo.Commands.UpdateCustomersPropertiesInfo;
using HospitioApi.Core.HandlePropertyGallery.Commands.CreatePropertyGallery;
using HospitioApi.Core.Services.Chat;
using HospitioApi.Core.Services.HandlerResponse;
using HospitioApi.Core.SignalR.Hubs;
using HospitioApi.Data;
using HospitioApi.Data.Models;
using HospitioApi.Shared;
using HospitioApi.Shared.Enums;
using System.Reflection;

namespace HospitioApi.Core.HandleCustomerGuestAppBuilder.Commands.UpdateCustomerAppBuilder;
public record UpdateCustomerGuestAppBuilderRequest(UpdateCustomerGuestAppBuilderIn In, string CustomerId) : IRequest<AppHandlerResponse>;
public class UpdateCustomerGuestAppBuilderHandler : IRequestHandler<UpdateCustomerGuestAppBuilderRequest, AppHandlerResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IHandlerResponseFactory _response;
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IChatService _chatService;
    public UpdateCustomerGuestAppBuilderHandler(ApplicationDbContext db, IHandlerResponseFactory response, IHubContext<ChatHub> hubContext,
        IChatService chatService)
    {
        _db = db;
        _response = response;
        _hubContext = hubContext;
        _chatService = chatService;
    }
    public async Task<AppHandlerResponse> Handle(UpdateCustomerGuestAppBuilderRequest request, CancellationToken cancellationToken)
        {
        var checkExist = await _db.CustomerRoomNames.Where(e => e.Id == request.In.CustomerRoomNameId && e.CustomerId == Convert.ToInt32(request.CustomerId)).FirstOrDefaultAsync(cancellationToken);
        if (checkExist == null)
        {
            return _response.Error($"The customer room not found.", AppStatusCodeError.UnprocessableEntity422);
        }

        var checkDuplicate = await _db.CustomerGuestAppBuilders.Where(e => e.CustomerRoomNameId == request.In.CustomerRoomNameId && e.Id != request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (checkDuplicate != null)
        {
            return _response.Error($"The customer guest app builder already exists.", AppStatusCodeError.UnprocessableEntity422);
        }

        var customerGuestAppBuilder = await _db.CustomerGuestAppBuilders.Where(e => e.Id == request.In.Id).FirstOrDefaultAsync(cancellationToken);
        if (customerGuestAppBuilder == null)
        {
            return _response.Error($"Customer guest app builder could not be found.", AppStatusCodeError.Gone410);
        }

        var displayOrderJson = await _db.ScreenDisplayOrderAndStatuses.Where(e => e.RefrenceId == request.In.Id && e.ScreenName == Convert.ToInt32(ScreenDisplayOrder.GuestPortalBuilder)).FirstOrDefaultAsync(cancellationToken);
        if (request.In.ActiveKey == true)
        {
            if (customerGuestAppBuilder.JsonData != null)
            {
                customerGuestAppBuilder.CustomerId = Convert.ToInt32(request.CustomerId);
                customerGuestAppBuilder.CustomerRoomNameId = request.In.CustomerRoomNameId;
                customerGuestAppBuilder.Concierge = request.In.Concierge;
                customerGuestAppBuilder.Ekey = request.In.Ekey;
                customerGuestAppBuilder.EnhanceYourStay = request.In.EnhanceYourStay;
                customerGuestAppBuilder.Housekeeping = request.In.Housekeeping;
                customerGuestAppBuilder.LocalExperience = request.In.LocalExperience;
                customerGuestAppBuilder.Message = request.In.Message;
                customerGuestAppBuilder.PropertyInfo = request.In.PropertyInfo;
                customerGuestAppBuilder.Reception = request.In.Reception;
                customerGuestAppBuilder.RoomService = request.In.RoomService;
                customerGuestAppBuilder.SecondaryMessage = request.In.SecondaryMessage;
                customerGuestAppBuilder.TransferServices = request.In.TransferServices;
                customerGuestAppBuilder.IsActive = request.In.IsActive;
                customerGuestAppBuilder.IsWork = request.In.IsWork;
                customerGuestAppBuilder.OnlineCheckIn = request.In.OnlineCheckIn;
                customerGuestAppBuilder.IsPublish = true;
                customerGuestAppBuilder.JsonData = null;
                displayOrderJson.IsPublish = true;
                displayOrderJson.JsonData = request.In.JsonData;
                displayOrderJson.ScreenJsonData = null;
            }

        }
        else
        {
                var jsonObj = new CreateCustomerGuestAppBuilderIn()
                {
                    CustomerId = Convert.ToInt32(request.CustomerId),
                    CustomerRoomNameId = request.In.CustomerRoomNameId,
                    Message = request.In.Message,
                    SecondaryMessage = request.In.SecondaryMessage,
                    LocalExperience = request.In.LocalExperience,
                    Ekey = request.In.Ekey,
                    PropertyInfo = request.In.PropertyInfo,
                    EnhanceYourStay = request.In.EnhanceYourStay,
                    Reception = request.In.Reception,
                    Housekeeping = request.In.Housekeeping,
                    RoomService = request.In.RoomService,
                    Concierge = request.In.Concierge,
                    OnlineCheckIn=request.In.OnlineCheckIn,
                    TransferServices = request.In.TransferServices,
                    IsActive = request.In.IsActive,
                };

            customerGuestAppBuilder.JsonData = JsonConvert.SerializeObject(jsonObj);
            displayOrderJson.ScreenJsonData = request.In.JsonData;
        }
        await _db.SaveChangesAsync(cancellationToken);


        #region concierge publish

        var conciergeCategory = await _db.CustomerGuestAppConciergeCategories.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppBuilderId == request.In.Id).ToListAsync();

        foreach (var item in conciergeCategory)
        {
            if (item.JsonData != null)
            {
                var deserializedObject = JsonConvert.DeserializeObject<UpdateCustomerConciergeCategoryIn>(item.JsonData);

                if (deserializedObject.IsDeleted == true)
                {
                    var DeleteconciergeCategory = await _db.CustomerGuestAppConciergeCategories.FindAsync(deserializedObject.Id);

                    var FindconciergeItems = await _db.CustomerGuestAppConciergeItems.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppConciergeCategoryId == deserializedObject.Id).ToListAsync();

                    foreach(var conciergeItemDatas in FindconciergeItems)
                    {
                        var DeleteconciergeItems = await _db.CustomerGuestAppConciergeItems.Where(s => s.Id == conciergeItemDatas.Id).FirstOrDefaultAsync();

                        _db.CustomerGuestAppConciergeItems.Remove(DeleteconciergeItems);
                    }

                    _db.CustomerGuestAppConciergeCategories.Remove(DeleteconciergeCategory);
                    await _db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    item.CustomerId = Convert.ToInt32(request.CustomerId);
                    item.CustomerGuestAppBuilderId = deserializedObject.CustomerGuestAppBuilderId;
                    item.CategoryName = deserializedObject.CategoryName;
                    item.DisplayOrder = deserializedObject.DisplayOrder;
                    item.IsActive = deserializedObject.IsActive;
                    item.IsPublish = true;
                    item.JsonData = null;
                    await _db.SaveChangesAsync(cancellationToken);
                }
            } else
            {
                item.IsPublish = true;
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        var conciergeItems = await _db.CustomerGuestAppConciergeItems.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppBuilderId == request.In.Id).ToListAsync();

        foreach(var item in conciergeItems)
        {
            if(item.JsonData != null)
            {
                var deserializedObject = JsonConvert.DeserializeObject<UpdateCustomerConciergeItemIn>(item.JsonData);

                if (deserializedObject.IsDeleted == true)
                {
                    var DeleteconciergeItems = await _db.CustomerGuestAppConciergeItems.Where(s => s.Id == deserializedObject.Id).FirstOrDefaultAsync();

                    _db.CustomerGuestAppConciergeItems.Remove(DeleteconciergeItems);
                    await _db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    item.CustomerId = Convert.ToInt32(request.CustomerId);
                    item.CustomerGuestAppBuilderId = deserializedObject.CustomerGuestAppBuilderId;
                    item.CustomerGuestAppConciergeCategoryId = item.CustomerGuestAppConciergeCategoryId;
                    item.Name = deserializedObject.Name;
                    item.ItemsMonth = deserializedObject.ItemsMonth;
                    item.ItemsDay = deserializedObject.ItemsDay;
                    item.ItemsMinute = deserializedObject.ItemsMinute;
                    item.ItemsHour = deserializedObject.ItemsHour;
                    item.QuantityBar = deserializedObject.QuantityBar;
                    item.ItemLocation = deserializedObject.ItemLocation;
                    item.Comment = deserializedObject.Comment;
                    item.IsPriceEnable = deserializedObject.IsPriceEnable;
                    item.Price = deserializedObject.Price;
                    item.Currency = deserializedObject.Currency;
                    item.DisplayOrder = deserializedObject.DisplayOrder;
                    item.IsActive = deserializedObject.IsActive;
                    item.IsPublish = true;
                    item.JsonData = null;
                    await _db.SaveChangesAsync(cancellationToken);
                }
            } else
            {
                item.IsPublish = true;
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
        #endregion

        #region house keeping publish 
        var housekeepingCategory = await _db.CustomerGuestAppHousekeepingCategories.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppBuilderId == request.In.Id).ToListAsync();

        foreach (var item in housekeepingCategory)
        {
            if (item.JsonData != null)
            {
                var deserializedObject = JsonConvert.DeserializeObject<UpdateCustomerConciergeCategoryIn>(item.JsonData);

                if (deserializedObject.IsDeleted == true)
                {
                    var DeletehousekeepingCategory = await _db.CustomerGuestAppHousekeepingCategories.FindAsync(deserializedObject.Id);

                    var FindhousekeepingItems = await _db.CustomerGuestAppHousekeepingItems.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppHousekeepingCategoryId == deserializedObject.Id).ToListAsync();

                    foreach (var housekeepingItemDatas in FindhousekeepingItems)
                    {
                        var DeletehousekeepingItems = await _db.CustomerGuestAppHousekeepingItems.Where(s => s.Id == housekeepingItemDatas.Id).FirstOrDefaultAsync();

                        _db.CustomerGuestAppHousekeepingItems.Remove(DeletehousekeepingItems);
                    }

                    _db.CustomerGuestAppHousekeepingCategories.Remove(DeletehousekeepingCategory);
                    await _db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    item.CustomerId = Convert.ToInt32(request.CustomerId);
                    item.CustomerGuestAppBuilderId = deserializedObject.CustomerGuestAppBuilderId;
                    item.CategoryName = deserializedObject.CategoryName;
                    item.DisplayOrder = deserializedObject.DisplayOrder;
                    item.IsActive = deserializedObject.IsActive;
                    item.IsPublish = true;
                    item.JsonData = null;
                    await _db.SaveChangesAsync(cancellationToken);
                }
            }
            else
            {
                item.IsPublish = true;
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        var housekeepingItems = await _db.CustomerGuestAppHousekeepingItems.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppBuilderId == request.In.Id).ToListAsync();

        foreach (var item in housekeepingItems)
        {
            if (item.JsonData != null)
            {
                var deserializedObject = JsonConvert.DeserializeObject<UpdateCustomerConciergeItemIn>(item.JsonData);

                if (deserializedObject.IsDeleted == true)
                {
                    var DeletehousekeepingItems = await _db.CustomerGuestAppHousekeepingItems.Where(s => s.Id == deserializedObject.Id).FirstOrDefaultAsync();

                    _db.CustomerGuestAppHousekeepingItems.Remove(DeletehousekeepingItems);
                    await _db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    item.CustomerId = Convert.ToInt32(request.CustomerId);
                    item.CustomerGuestAppBuilderId = deserializedObject.CustomerGuestAppBuilderId;
                    item.CustomerGuestAppHousekeepingCategoryId = item.CustomerGuestAppHousekeepingCategoryId;
                    item.Name = deserializedObject.Name;
                    item.ItemsMonth = deserializedObject.ItemsMonth;
                    item.ItemsDay = deserializedObject.ItemsDay;
                    item.ItemsMinute = deserializedObject.ItemsMinute;
                    item.ItemsHour = deserializedObject.ItemsHour;
                    item.QuantityBar = deserializedObject.QuantityBar;
                    item.ItemLocation = deserializedObject.ItemLocation;
                    item.Comment = deserializedObject.Comment;
                    item.IsPriceEnable = deserializedObject.IsPriceEnable;
                    item.Price = deserializedObject.Price;
                    item.Currency = deserializedObject.Currency;
                    item.DisplayOrder = deserializedObject.DisplayOrder;
                    item.IsActive = deserializedObject.IsActive;
                    item.IsPublish = true;
                    item.JsonData = null;
                    await _db.SaveChangesAsync(cancellationToken);
                }
            }
            else
            {
                item.IsPublish = true;
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
        #endregion

        #region reception publish 
        var receptionCategory = await _db.CustomerGuestAppReceptionCategories.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppBuilderId == request.In.Id).ToListAsync();

        foreach (var item in receptionCategory)
        {
            if (item.JsonData != null)
            {
                var deserializedObject = JsonConvert.DeserializeObject<UpdateCustomerConciergeCategoryIn>(item.JsonData);

                if (deserializedObject.IsDeleted == true)
                {
                    var DeletereceptionCategory = await _db.CustomerGuestAppReceptionCategories.FindAsync(deserializedObject.Id);

                    var FindreceptionItems = await _db.CustomerGuestAppReceptionItems.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppReceptionCategoryId == deserializedObject.Id).ToListAsync();

                    foreach (var DeletereceptionItemData in FindreceptionItems)
                    {
                        var DeletereceptionItems = await _db.CustomerGuestAppReceptionItems.Where(s => s.Id == DeletereceptionItemData.Id).FirstOrDefaultAsync();

                        _db.CustomerGuestAppReceptionItems.Remove(DeletereceptionItems);
                    }

                    _db.CustomerGuestAppReceptionCategories.Remove(DeletereceptionCategory);
                    await _db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    item.CustomerId = Convert.ToInt32(request.CustomerId);
                    item.CustomerGuestAppBuilderId = deserializedObject.CustomerGuestAppBuilderId;
                    item.CategoryName = deserializedObject.CategoryName;
                    item.DisplayOrder = deserializedObject.DisplayOrder;
                    item.IsActive = deserializedObject.IsActive;
                    item.IsPublish = true;
                    item.JsonData = null;
                    await _db.SaveChangesAsync(cancellationToken);
                }
            }
            else
            {
                item.IsPublish = true;
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        var receptionItems = await _db.CustomerGuestAppReceptionItems.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppBuilderId == request.In.Id).ToListAsync();

        foreach (var item in receptionItems)
        {
            if (item.JsonData != null)
            {
                var deserializedObject = JsonConvert.DeserializeObject<UpdateCustomerConciergeItemIn>(item.JsonData);

                if (deserializedObject.IsDeleted == true)
                {
                    var DeletereceptionItems = await _db.CustomerGuestAppReceptionItems.Where(s => s.Id == deserializedObject.Id).FirstOrDefaultAsync();

                    _db.CustomerGuestAppReceptionItems.Remove(DeletereceptionItems);
                    await _db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    item.CustomerId = Convert.ToInt32(request.CustomerId);
                    item.CustomerGuestAppBuilderId = deserializedObject.CustomerGuestAppBuilderId;
                    item.CustomerGuestAppReceptionCategoryId = item.CustomerGuestAppReceptionCategoryId;
                    item.Name = deserializedObject.Name;
                    item.ItemsMonth = deserializedObject.ItemsMonth;
                    item.ItemsDay = deserializedObject.ItemsDay;
                    item.ItemsMinute = deserializedObject.ItemsMinute;
                    item.ItemsHour = deserializedObject.ItemsHour;
                    item.QuantityBar = deserializedObject.QuantityBar;
                    item.ItemLocation = deserializedObject.ItemLocation;
                    item.Comment = deserializedObject.Comment;
                    item.IsPriceEnable = deserializedObject.IsPriceEnable;
                    item.Price = deserializedObject.Price;
                    item.Currency = deserializedObject.Currency;
                    item.DisplayOrder = deserializedObject.DisplayOrder;
                    item.IsActive = deserializedObject.IsActive;
                    item.IsPublish = true;
                    item.JsonData = null;
                    await _db.SaveChangesAsync(cancellationToken);
                }
            }
            else
            {
                item.IsPublish = true;
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
        #endregion

        #region roomservice publish 

        var roomserviceCategory = await _db.CustomerGuestAppRoomServiceCategories.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppBuilderId == request.In.Id).ToListAsync();

        foreach (var item in roomserviceCategory)
        {
            if (item.JsonData != null)
            {
                var deserializedObject = JsonConvert.DeserializeObject<UpdateCustomerConciergeCategoryIn>(item.JsonData);

                if (deserializedObject.IsDeleted == true)
                {
                    var DeleteroomserviceCategory = await _db.CustomerGuestAppRoomServiceCategories.FindAsync(deserializedObject.Id);

                    var FindroomserviceItems = await _db.CustomerGuestAppRoomServiceItems.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppRoomServiceCategoryId == deserializedObject.Id).ToListAsync();

                    foreach (var roomServiceItemDatas in FindroomserviceItems)
                    {
                        var DeleteroomServiceItemDatas = await _db.CustomerGuestAppRoomServiceItems.Where(s => s.Id == roomServiceItemDatas.Id).FirstOrDefaultAsync();

                        _db.CustomerGuestAppRoomServiceItems.Remove(DeleteroomServiceItemDatas);
                    }

                    _db.CustomerGuestAppRoomServiceCategories.Remove(DeleteroomserviceCategory);
                    await _db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    item.CustomerId = Convert.ToInt32(request.CustomerId);
                    item.CustomerGuestAppBuilderId = deserializedObject.CustomerGuestAppBuilderId;
                    item.CategoryName = deserializedObject.CategoryName;
                    item.DisplayOrder = deserializedObject.DisplayOrder;
                    item.IsActive = deserializedObject.IsActive;
                    item.IsPublish = true;
                    item.JsonData = null;
                    await _db.SaveChangesAsync(cancellationToken);
                }
            }
            else
            {
                item.IsPublish = true;
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        var roomserviceItems = await _db.CustomerGuestAppRoomServiceItems.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppBuilderId == request.In.Id).ToListAsync();

        foreach (var item in roomserviceItems)
        {
            if (item.JsonData != null)
            {
                var deserializedObject = JsonConvert.DeserializeObject<UpdateCustomerConciergeItemIn>(item.JsonData);

                if (deserializedObject.IsDeleted == true)
                {
                    var DeleteroomserviceItems = await _db.CustomerGuestAppRoomServiceItems.Where(s => s.Id == deserializedObject.Id).FirstOrDefaultAsync();

                    _db.CustomerGuestAppRoomServiceItems.Remove(DeleteroomserviceItems);
                    await _db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    item.CustomerId = Convert.ToInt32(request.CustomerId);
                    item.CustomerGuestAppBuilderId = deserializedObject.CustomerGuestAppBuilderId;
                    item.CustomerGuestAppRoomServiceCategoryId = item.CustomerGuestAppRoomServiceCategoryId;
                    item.Name = deserializedObject.Name;
                    item.ItemsMonth = deserializedObject.ItemsMonth;
                    item.ItemsDay = deserializedObject.ItemsDay;
                    item.ItemsMinute = deserializedObject.ItemsMinute;
                    item.ItemsHour = deserializedObject.ItemsHour;
                    item.QuantityBar = deserializedObject.QuantityBar;
                    item.ItemLocation = deserializedObject.ItemLocation;
                    item.Comment = deserializedObject.Comment;
                    item.IsPriceEnable = deserializedObject.IsPriceEnable;
                    item.Price = deserializedObject.Price;
                    item.Currency = deserializedObject.Currency;
                    item.DisplayOrder = deserializedObject.DisplayOrder;
                    item.IsActive = deserializedObject.IsActive;
                    item.IsPublish = true;
                    item.JsonData = null;
                    await _db.SaveChangesAsync(cancellationToken);
                }
            }
            else
            {
                item.IsPublish = true;
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
        #endregion

        #region EnhanceYouStay publish

        var enhanceYouStayItem = await _db.CustomerGuestAppEnhanceYourStayItems.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppBuilderId == request.In.Id).ToListAsync();

        var enhanceYouStayCategory = await _db.CustomerGuestAppEnhanceYourStayCategories.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppBuilderId == request.In.Id).ToListAsync();

        foreach (var item in enhanceYouStayCategory)
        {
            if (item.JsonData != null)
            {
                var deserializedObject = JsonConvert.DeserializeObject<CategoryName>(item.JsonData);

                if (deserializedObject.IsDeleted == true)
                {
                    var DeleteenhanceYouStayCategory = enhanceYouStayCategory.Where(e => e.Id == deserializedObject.CategoryId).FirstOrDefault();

                    var FindenhanceYouStayItem = enhanceYouStayItem.Where(e => e.CustomerGuestAppBuilderCategoryId == deserializedObject.CategoryId).ToList();

                    foreach (var enhanceYouStayItemDatas in FindenhanceYouStayItem)
                    {
                        var DeleteenhanceYouStayItems = FindenhanceYouStayItem.Where(e => e.Id == enhanceYouStayItemDatas.Id).FirstOrDefault();

                        _db.CustomerGuestAppEnhanceYourStayItems.Remove(DeleteenhanceYouStayItems);
                    }

                    _db.CustomerGuestAppEnhanceYourStayCategories.Remove(DeleteenhanceYouStayCategory);
                    await _db.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    item.CategoryName = deserializedObject.Name;
                    item.DisplayOrder = deserializedObject.CategoryDisplayOrder;
                    item.IsPublish = true;
                    item.JsonData = null;
                    await _db.SaveChangesAsync(cancellationToken);
                    if (deserializedObject.categoryItems.Any())
                    {
                        enhanceYouStayItem = enhanceYouStayItem.Where(s => s.DeletedAt == null).ToList();

                        foreach (var eysItem in enhanceYouStayItem)
                        {
                            if (eysItem != null && eysItem.JsonData != null)
                            {
                                var eysItemObject = JsonConvert.DeserializeObject<UpdateCustomerEnhanceYourStayItemIn>(eysItem.JsonData);

                                if (eysItemObject.IsDeleted == true)
                                {
                                    var DeleteenhanceYouStayItem = enhanceYouStayItem.Where(e => e.Id == eysItemObject.Id).FirstOrDefault();

                                    _db.CustomerGuestAppEnhanceYourStayItems.Remove(DeleteenhanceYouStayItem);
                                    await _db.SaveChangesAsync(cancellationToken);
                                }
                                else
                                {
                                    eysItem.IsActive = eysItemObject.IsActive;
                                    eysItem.DisplayOrder = eysItemObject.DisplayOrder;
                                    await _db.SaveChangesAsync(cancellationToken);
                                }
                            }
                            else
                            {
                                eysItem.IsPublish = true;
                                await _db.SaveChangesAsync(cancellationToken);
                            }
                        }
                    }
                }
            }
            else
            {
                item.IsPublish = true;
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        enhanceYouStayItem = enhanceYouStayItem.Where(s => s.DeletedAt == null).ToList();

        foreach (var item in enhanceYouStayItem)
        {
            if (item.JsonData != null)
            {
                var deserializedObject = JsonConvert.DeserializeObject<UpdateCustomerEnhanceYourStayItemIn>(item.JsonData);
                item.CustomerId = Convert.ToInt32(request.CustomerId);
                item.CustomerGuestAppBuilderId = deserializedObject.CustomerGuestAppBuilderId;
                item.CustomerGuestAppBuilderCategoryId = item.CustomerGuestAppBuilderCategoryId;
                item.Badge = deserializedObject.Badge;
                item.ShortDescription = deserializedObject.ShortDescription;
                item.LongDescription = deserializedObject.LongDescription;
                item.ButtonType = deserializedObject.ButtonType;
                item.ButtonText = deserializedObject.ButtonText;
                item.ChargeType = deserializedObject.ChargeType;
                item.Discount = deserializedObject.Discount;
                item.Price = deserializedObject.Price;
                item.Currency = deserializedObject.Currency;
                item.DisplayOrder = deserializedObject.DisplayOrder;
                item.IsPublish = true;
                item.JsonData = null;
                await _db.SaveChangesAsync(cancellationToken);
                if (deserializedObject.ItemsImages.Any())
                {
                    var enhanceYouStayItemImage = await _db.CustomerGuestAppEnhanceYourStayItemsImages.Where(e => e.CustomerGuestAppEnhanceYourStayItemId == item.Id).ToListAsync();
                    foreach (var image in enhanceYouStayItemImage)
                    {
                        if (image.JsonData != null)
                        {
                            var deserializedObjectImage = JsonConvert.DeserializeObject<UpdatedEnhanceYourStayItemImageOut>(image.JsonData);
                            image.CustomerGuestAppEnhanceYourStayItemId = deserializedObjectImage.CustomerGuestAppEnhanceYourStayItemId;
                            image.ItemsImages = deserializedObjectImage.ItemsImages;
                            image.DisaplayOrder = deserializedObjectImage.DisaplayOrder;
                            image.IsPublish = true;
                            image.JsonData = null;
                            await _db.SaveChangesAsync(cancellationToken);
                        }
                        else
                        {
                            image.IsPublish = true;
                            await _db.SaveChangesAsync(cancellationToken);
                        }
                    }
                }
                if (deserializedObject.CustomerEnhanceYourStayCategoryItemExtra.Any())
                {
                    var enhanceYouStayItemExtra = await _db.CustomerGuestAppEnhanceYourStayCategoryItemsExtras.Where(e => e.CustomerGuestAppEnhanceYourStayItemId == item.Id).ToListAsync();
                    foreach (var extra in enhanceYouStayItemExtra)
                    {
                        if (extra.JsonData != null)
                        {
                            var deserializedObjectImage = JsonConvert.DeserializeObject<UpdatedEnhanceYourStayCategoryItemExtraOut>(extra.JsonData);
                            extra.CustomerGuestAppEnhanceYourStayItemId = item.Id;
                            extra.QueType = deserializedObjectImage.QueType;
                            extra.Questions = deserializedObjectImage.Questions;
                            extra.OptionValues = deserializedObjectImage.OptionValues;
                            extra.IsPublish = true;
                            extra.JsonData = null;
                            await _db.SaveChangesAsync(cancellationToken);
                        }
                        else
                        {
                            extra.IsPublish = true;
                            await _db.SaveChangesAsync(cancellationToken);
                        }
                    }
                }

            }
            else
            {
                item.IsPublish = true;
                await _db.SaveChangesAsync(cancellationToken);
            }
        }

        #endregion

        #region PropertyInfo publish
        var customersPropertiesInfo = await _db.CustomerPropertyInformations.Where(e => e.CustomerId == Convert.ToInt32(request.CustomerId) && e.CustomerGuestAppBuilderId == request.In.Id).ToListAsync();

        foreach (var propertyInformation in customersPropertiesInfo)
        {

            if (propertyInformation.JsonData != null)
            {
                var deserializedObject = JsonConvert.DeserializeObject<UpdateCustomersPropertiesInfoIn>(propertyInformation.JsonData);
                propertyInformation.WifiUsername = deserializedObject.WifiUsername;
                propertyInformation.WifiPassword = deserializedObject.WifiPassword;
                propertyInformation.Overview = deserializedObject.Overview;
                propertyInformation.CheckInPolicy = deserializedObject.CheckInPolicy;
                propertyInformation.TermsAndConditions = deserializedObject.TermsAndConditions;
                propertyInformation.Street = deserializedObject.Street;
                propertyInformation.StreetNumber = deserializedObject.StreetNumber;
                propertyInformation.City = deserializedObject.City;
                propertyInformation.Country = deserializedObject.Country;
                propertyInformation.Postalcode = deserializedObject.Postalcode;
                propertyInformation.Latitude = deserializedObject.Latitude;
                propertyInformation.Longitude = deserializedObject.Longitude;
                propertyInformation.IsPublish = true;
                propertyInformation.JsonData = null;
                await _db.SaveChangesAsync(cancellationToken);
            }
            else
            {
                propertyInformation.IsPublish = true;
                await _db.SaveChangesAsync(cancellationToken);
            }

            var emergenceyNumber = await _db.CustomerPropertyEmergencyNumbers.Where(e => e.CustomerPropertyInformationId == propertyInformation.Id).ToListAsync();

            foreach (var customerPropertyEmergencyNumber in emergenceyNumber)
            {
                if (customerPropertyEmergencyNumber.JsonData != null)
                {
                    var deserializedObject = JsonConvert.DeserializeObject<UpdateCustomerPropertyEmergencyNumberIn>(customerPropertyEmergencyNumber.JsonData);

                    if (deserializedObject.IsDeleted)
                    {
                       var getCustomerPropertyEmergencyNumber = await _db.CustomerPropertyEmergencyNumbers.Where(x => x.Id == deserializedObject.Id).FirstOrDefaultAsync();
                        _db.CustomerPropertyEmergencyNumbers.Remove(getCustomerPropertyEmergencyNumber);
                        await _db.SaveChangesAsync(cancellationToken);

                    }
                    else
                    {
                        customerPropertyEmergencyNumber.Name = deserializedObject.Name;
                        customerPropertyEmergencyNumber.CustomerPropertyInformationId = deserializedObject.CustomerPropertyInformationId;
                        customerPropertyEmergencyNumber.PhoneCountry = deserializedObject.PhoneCountry;
                        customerPropertyEmergencyNumber.PhoneNumber = deserializedObject.PhoneNumber;
                        customerPropertyEmergencyNumber.IsActive = true;
                        customerPropertyEmergencyNumber.IsPublish = true;
                        customerPropertyEmergencyNumber.DisplayOrder = deserializedObject.DisplayOrder;
                        customerPropertyEmergencyNumber.JsonData = null;
                        await _db.SaveChangesAsync(cancellationToken);
                    }
                }
                else
                {
                    customerPropertyEmergencyNumber.IsPublish = true;
                    await _db.SaveChangesAsync(cancellationToken);
                }
            }

            var galleries = await _db.CustomerPropertyGalleries.Where(e => e.CustomerPropertyInformationId == propertyInformation.Id).ToListAsync();

            foreach(var CustomerPropGallery in galleries)
            {
                if(CustomerPropGallery.JsonData != null)
                {
                    var deserializedObject = JsonConvert.DeserializeObject<CreatePropertyGalleryImagesIn>(CustomerPropGallery.JsonData);
                    if (deserializedObject.IsDeleted)
                    {
                        var PropertyGallery = await _db.CustomerPropertyGalleries.Where(x => x.Id == deserializedObject.Id).FirstOrDefaultAsync(cancellationToken);
                        _db.CustomerPropertyGalleries.Remove(PropertyGallery);
                        await _db.SaveChangesAsync(cancellationToken);
                    }
                    else
                    {
                        
                        CustomerPropGallery.PropertyImage = deserializedObject.PropertyImage;
                        CustomerPropGallery.IsActive = deserializedObject.IsActive;
                        CustomerPropGallery.IsPublish = true;
                        CustomerPropGallery.JsonData = null;
                        await _db.SaveChangesAsync(cancellationToken);
                    }
                }
                else
                {
                    CustomerPropGallery.IsPublish = true;
                    await _db.SaveChangesAsync(cancellationToken);
                }
            }

            var extra = await _db.CustomerPropertyExtras.Where(e => e.CustomerPropertyInformationId == propertyInformation.Id).ToListAsync();

            foreach (var customerPropertyExtra in extra)
            {
                if (customerPropertyExtra.JsonData != null)
                {
                    var deserializedObject = JsonConvert.DeserializeObject<CustomerPropertyExtrasIn>(customerPropertyExtra.JsonData);

                    if (deserializedObject.IsDeleted)
                    {
                       var CustomerPropExtra = await _db.CustomerPropertyExtras.Where(x => x.Id == deserializedObject.Id).FirstOrDefaultAsync();

                        var CustomerPropExtraDetails = await _db.CustomerPropertyExtraDetails.Where(x => x.CustomerPropertyExtraId == CustomerPropExtra.Id).ToListAsync(cancellationToken);

                        foreach(var item in CustomerPropExtraDetails)
                        {
                           var CustomerPropExtraDetail =  await _db.CustomerPropertyExtraDetails.Where(x => x.Id ==  item.Id).FirstOrDefaultAsync(cancellationToken);
                           _db.CustomerPropertyExtraDetails.Remove(CustomerPropExtraDetail);
                            await _db.SaveChangesAsync(cancellationToken);

                        }
                        _db.CustomerPropertyExtras.Remove(CustomerPropExtra);
                        await _db.SaveChangesAsync(cancellationToken);
                    }
                    else
                    {
                        customerPropertyExtra.CustomerPropertyInformationId = deserializedObject.CustomerPropertyInformationId;
                        customerPropertyExtra.ExtraType = deserializedObject.ExtraType;
                        customerPropertyExtra.Name = deserializedObject.Name;
                        customerPropertyExtra.DisplayOrder = deserializedObject.DisplayOrder;
                        customerPropertyExtra.IsPublish = true;
                        customerPropertyExtra.JsonData = null;
                        await _db.SaveChangesAsync(cancellationToken);
                    }
                   
                }
                else
                {
                    customerPropertyExtra.IsPublish = true;
                    await _db.SaveChangesAsync(cancellationToken);
                }

                var extraDetail = await _db.CustomerPropertyExtraDetails.Where(e => e.CustomerPropertyExtraId == customerPropertyExtra.Id).ToListAsync();

                foreach (var detail in extraDetail)
                {
                    if (detail.JsonData != null)
                    {
                        var deserializedObject = JsonConvert.DeserializeObject<CustomerPropertyExtraDetailsIn>(detail.JsonData);
                        if (deserializedObject.IsDeleted)
                        {
                            var customerPropExtra =  await _db.CustomerPropertyExtraDetails.Where(x => x.Id == deserializedObject.Id).FirstOrDefaultAsync(cancellationToken);
                            _db.CustomerPropertyExtraDetails.Remove(customerPropExtra);
                            await _db.SaveChangesAsync(cancellationToken);

                        }
                        else
                        {
                            detail.Description = deserializedObject.Description;
                            detail.Latitude = deserializedObject.Latitude;
                            detail.Longitude = deserializedObject.Longitude;
                            detail.IsPublish = true;
                            detail.JsonData = null;
                            await _db.SaveChangesAsync(cancellationToken);
                        }
                       
                    }
                    else
                    {
                        detail.IsPublish = true;
                        await _db.SaveChangesAsync(cancellationToken);
                    }
                }
            }

            var services = await _db.CustomerPropertyServices.Where(e => e.CustomerPropertyInformationId == propertyInformation.Id).ToListAsync();

            foreach (var customerPropertService in services)
            {
                if (customerPropertService.JsonData != null)
                {
                    
                    var deserializedObject = JsonConvert.DeserializeObject<CreateCustomerPropertyServiceIn>(customerPropertService.JsonData);
                    if(deserializedObject.IsDeleted)
                    {
                       var CustomerPropService =  await  _db.CustomerPropertyServices.Where(x => x.Id == deserializedObject.Id).FirstOrDefaultAsync(cancellationToken);

                        var CustomerPropServiceImages  = await _db.CustomerPropertyServiceImages.Where(x => x.CustomerPropertyServiceId == CustomerPropService.Id).ToListAsync(cancellationToken);

                        foreach(var item in CustomerPropServiceImages)
                        {
                            var CustomerPropServiceImage  = await _db.CustomerPropertyServiceImages.Where(x => x.Id == item.Id).FirstOrDefaultAsync(cancellationToken);                            
                         
                            _db.CustomerPropertyServiceImages.Remove(CustomerPropServiceImage);
                           await _db.SaveChangesAsync(cancellationToken);
                        }

                        _db.CustomerPropertyServices.Remove(CustomerPropService);
                        await _db.SaveChangesAsync(cancellationToken);
                    }
                    else
                    {
                        customerPropertService.Name = deserializedObject.Name;
                        customerPropertService.Icon = deserializedObject.Icon;
                        customerPropertService.Description = deserializedObject.Description;
                        customerPropertService.IsActive = deserializedObject.IsActive;
                        customerPropertService.IsPublish = true;
                        customerPropertService.JsonData = null;
                        await _db.SaveChangesAsync(cancellationToken);
                    }
                   
                }
                else
                {
                    customerPropertService.IsPublish = true;
                    await _db.SaveChangesAsync(cancellationToken);
                }

                var CustomerPropertyServiceImages =  await _db.CustomerPropertyServiceImages.Where(x => x.CustomerPropertyServiceId == customerPropertService.Id).ToListAsync();
                foreach(var serviceImage in CustomerPropertyServiceImages)
                {
                    if(serviceImage.JsonData != null) 
                    {
                      var deserializedObject =  JsonConvert.DeserializeObject<CustomerPropertyInfoServiceImagesOuts>(serviceImage.JsonData);

                        if (deserializedObject.IsDeleted)
                        {
                            var CustomerPropServiceImage = await _db.CustomerPropertyServiceImages.Where(x => x.Id == deserializedObject.Id).FirstOrDefaultAsync();
                            _db.CustomerPropertyServiceImages.Remove(CustomerPropServiceImage);
                            await _db.SaveChangesAsync(cancellationToken);
                        }
                        else
                        {
                            serviceImage.ServiceImages = deserializedObject.ServiceImages;
                            serviceImage.IsPublish = true;
                            serviceImage.JsonData = null;
                            await _db.SaveChangesAsync(cancellationToken);
                        }
                    }
                    else
                    {
                        serviceImage.IsPublish = true;
                        await _db.SaveChangesAsync(cancellationToken);
                    }
                }
            }
        }
        #endregion

        var updateCustomerGuestAlertsOut = new UpdatedCustomerGuestAppBuilderOut()
        {
            Id = customerGuestAppBuilder.Id,
            CustomerRoomNameId = customerGuestAppBuilder.CustomerRoomNameId,
            Concierge = customerGuestAppBuilder.Concierge,
            Ekey = customerGuestAppBuilder.Ekey,
            EnhanceYourStay = customerGuestAppBuilder.EnhanceYourStay,
            Housekeeping = customerGuestAppBuilder.Housekeeping,
            LocalExperience = customerGuestAppBuilder.LocalExperience,
            Message = customerGuestAppBuilder.Message,
            PropertyInfo = customerGuestAppBuilder.PropertyInfo,
            Reception = customerGuestAppBuilder.Reception,
            RoomService = customerGuestAppBuilder.RoomService,
            SecondaryMessage = customerGuestAppBuilder.SecondaryMessage,
            //TransferServices = customerGuestAppBuilder.TransferServices,
            TransferServices = true,
            OnlineCheckIn = customerGuestAppBuilder.OnlineCheckIn,
            IsActive = customerGuestAppBuilder.IsActive
        };


        //Send Online-check-in Status 
        var guestPoratalCustomerUsersList = await (from cu in _db.CustomerUsers join cup in _db.CustomerUsersPermissions on cu.Id equals cup.CustomerUserId into cups from cup in cups.DefaultIfEmpty() join cp in _db.CustomerPermissions on cup.CustomerPermissionId equals cp.Id into cps from cp in cps.DefaultIfEmpty() where cu.CustomerId == int.Parse(request.CustomerId) && cu.DeletedAt == null && cu.IsActive == true && (cu.CustomerLevelId == 1 || (cup != null && (cup.IsView == true || cup.IsEdit == true) && cp.NormalizedName == "OnlineCheckin")) select cu.Id).ToListAsync(cancellationToken);

        if (guestPoratalCustomerUsersList != null || guestPoratalCustomerUsersList.Count != 0)
        {
            var guestProtalStatus =await _chatService.GetCustomerOnboardingStatus(_db, request.CustomerId);
            foreach (var user in guestPoratalCustomerUsersList)
            {
                await _hubContext.Clients.Group($"user-2-{user}").SendAsync("GetCustomerOnboardingStatus", guestProtalStatus);
            }
        }

        return _response.Success(new UpdateCustomerGuestAppBuilderOut("Update customer guest app builder successful.", updateCustomerGuestAlertsOut));
    }
}
